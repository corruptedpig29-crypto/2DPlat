

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Presets;
using UnityEngine;
using static UnityEditor.PlayerSettings;


//Queries Hitting triggers is off.

public class PlayerMovement : MonoBehaviour
{
    float initWallJumpDuration = 0.5f / 5f;

    public Boolean isGrounded;
    public float dirForParryKbVert = 0f;

    public float gScale;
    bool djavail = true;
    float djtimer = -1;


    public Boolean fastRunning = false;
    float pushbackamt;

    public float atkkbdur = 0f;

    public float parrybdur = 0f;

    public int dirParriedFrom = 0;

    //public int attackDir = 0;

    float initDashDur = 0.375f * 7/12f;


    public float dirY;
    bool justdashed = false;

    public float grappletimer = 0.3f;
    float updlastloccd = 180;
    public float lastgrndlocx = 0;
    public float lastgrndlocy = 0;
    int grapplemovespeedmult = 12;


    //grapplemovespeedmult initially 5, i changed to 6 because i wanted him to keep moving
    public Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private float hopspeed = -150f;
    private dcoll damagecoll;
    public bool beinghit = false;
    public int dirhitfrom = 0;
    [SerializeField] private LayerMask jumpableground;
    [SerializeField] private LayerMask projJumpableGround;

    
    float jforcemult = 6f;
    private enum MovementState { idle, running, jumping, falling, dash , hit, honored, attacking, wallsliding }

    public BoxCollider2D coll;

     public float movespeed = 60f;
     public float jumpforce = 100f;


    private float attackkbdur = 0 / 80;


    public float MoveBufferTimer = 0.1f;
    public float hittimer = 300 / 80 / 7.5f;
    private float PlayerX;
    private float dashamountup = 0f; //magic number to cancel out the gravity shifting him into the ground for some reason
    private float PlayerY;
    private Boolean ljumping = false;
    private Boolean rjumping = false;
    public Boolean dashing = false;
    public Boolean dashable = true;
    private attack attack;
    private float jumpdur = -100f / 80;
    private float dashspeed;
    public float acd = 0;
    [SerializeField] public float dashDur = 0f;
    grapplehook ghook;
    PlayerEnergyControl pec;

    BoxCollider2D dcollCollider;


    float timertobeginrun = 0.1f;

    // Start is called before the first frame update
    [SerializeField] GameObject TPP;
    //public float fcd = 0;
    [SerializeField] public float dirX = 0f;
    public float mostrecdirX = 1;
    [SerializeField]private float walljumpdurationleft = 0 / 80;
    float origy;
    float origx;
    public bool grappling;
    float prevdirx = 0.0f;



    public int dirpushedvertonhit = 0;
    public int dirpushedhorizonhit = 0;




    float runspeed = 45f;

    public float justgrappledtimer = 0.23f;

    public Boolean justgrappled = false;


    public float timerPostDashForGenericDashAttack = 0.15f;

    public Boolean parryhopresetted = false;
    void Start()
    {
        attack = FindObjectOfType<attack>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        coll=  GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        damagecoll = FindObjectOfType<dcoll>();
        ghook = FindObjectOfType<grapplehook>();
        dashspeed = 14 * movespeed;
        pec = FindObjectOfType<PlayerEnergyControl>();

        sprite.flipY = false;


        dcollCollider = damagecoll.GetComponent<BoxCollider2D>();
        sprite.flipX = false;

        //coll.isTrigger = false;

        filter.useTriggers = false;


        dashing = false;
        dashDur = 0;

        pushbackamt = 60f;

        rb.velocity = new Vector2(0,0);
        Physics2D.queriesHitTriggers = false;

        Debug.Log("Queries Hit Triggers : " + Physics2D.queriesHitTriggers);
    }

    // Update is called once per frame

    String prev = "";


    bool firstFrameStartDash = false;
    void Update()
    {
        callAllgrndIrrel();
        isGrounded = IsGrounded();
        if ((OnLWall() || OnRWall() ) && !IsGrounded())
        {
            justdashed = false;
        }

        /*
        if(!prev.Equals(OnRWallGroundIrrelevant() + " " + OnLWallGroundIrrelevant() + " " + OnRWallProjGroundIrrelevant() + " " + OnLWallProjGroundIrrelevant())) Debug.Log(OnRWallGroundIrrelevant() + " " + OnLWallGroundIrrelevant() + " " + OnRWallProjGroundIrrelevant() + " " + OnLWallProjGroundIrrelevant());
        prev = OnRWallGroundIrrelevant() + " " + OnLWallGroundIrrelevant() + " " + OnRWallProjGroundIrrelevant() + " " + OnLWallProjGroundIrrelevant();

        */
        SqueezeDamageCheck();


        prevdirx = dirX;   
        
        
        if(!justdashed)
        {
            runspeed = 45f;
        }


        if (justdashed)
        {
            timertobeginrun -= Time.deltaTime;
        }

        updlastloccd--;
        IsGrounded();

        Debug.Log(damagecoll.cd);


        if (firstFrameStartDash)
        {
            firstFrameStartDash = false;
            if(damagecoll.cd < dashDur)
            {
                damagecoll.cd = dashDur;
            }
        }

        if (dashing)
        {
            rb.velocity = new Vector2(dashspeed, dashamountup * 25/18);
            if (IsGrounded())  justdashed = true;
            timerPostDashForGenericDashAttack = 0.15f;
        }
        else
        {
            timerPostDashForGenericDashAttack -= Time.deltaTime;
        }

        if (!Input.GetKey(KeyCode.LeftShift) || mostrecdirX == 0 || attack.attackbegan)
        {
            //Debug.Log("hello");
            justdashed = false;
            timertobeginrun = 0.1f;
        }

        if (justdashed == true && timertobeginrun < 0)
        {
            fastRunning = true;
            runspeed = 80f;
        }
        else
        {
            fastRunning = false;
        }




        if (attack.CameInContact)
        {
            attackkbdur = 60 / 80;
            attack.CameInContact = false;
        }


        //ATTACK RIGHT HERE


        //RIGHT HERE



        /*
        if (attackkbdur >= 0)
        {
            rb.velocity = new Vector2(-mostrecdirX * movespeed, rb.velocity.y);
            attackkbdur-=1*Time.deltaTime;
            return;
        }
        */



        if (dirX != 0f && !dashing)
        {
            mostrecdirX = dirX;

            if (OnLWall())
            {
                mostrecdirX = 1;
            }

            if (OnRWall())
            {
                mostrecdirX = -1;
            }

        }



        acd += Time.deltaTime;
        //fcd += Time.deltaTime * 5f;
        dashDur -= Time.deltaTime;
        walljumpdurationleft -=  Time.deltaTime;

        hittimer -= Time.deltaTime;


        if((OnLWall() || OnRWall()) && walljumpdurationleft < 0.1f / 5f )
        {
            walljumpdurationleft = -1f / 5f;
        }





        //movespeed = 30f;
        if (!dashing) dirX = Input.GetAxisRaw("Horizontal");

        dirY = Input.GetAxisRaw("Vertical");


        if ((dirX == 1 && OnRWall()) || (dirX == -1 && OnLWall()))
        {

        }
        else
        {

            //Where all the movement is somehow
            if (atkkbdur <  0 && !pec.healing && parrybdur <= 0 && pec.abilityMoving == false && !beinghit && !(attack.attackbegan && attack.attackType == 2) && walljumpdurationleft <= 0) rb.velocity = new Vector2(dirX * runspeed * 1.5f, rb.velocity.y);


        }

        /*
        if (!(dirpushedhorizonhit == 0 && !pec.healing && parrybdur <= 0 && pec.abilityMoving == false))
        {
            Debug.Log("Dir pushed horiz on hit :" + dirpushedhorizonhit + " PEC Healing : " + pec.healing);
            Debug.Log("parrybdur" + parrybdur + " PEC abilmoving : " + pec.abilityMoving);

        }
        */






        if (ljumping)
        {
            rb.velocity = new Vector2(3.5f * movespeed, 6 * jumpforce * 25 / 18);
        }
        if (rjumping)
        {
            rb.velocity = new Vector2(3.5f*-movespeed, 6 * jumpforce * 25/18);
        }


        if (OnLWall() || OnRWall())
        {
            if (ljumping == false && rjumping == false && dashing == false)
            {
                rb.velocity = new Vector2(rb.velocity.x, -11);
            }

            if(MoveBufferTimer < 0)pec.abilityMoving = false;
        }

        MoveBufferTimer -= Time.deltaTime;



        if (Input.GetButtonDown("Jump") && !pec.healing){
            if (IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jforcemult * jumpforce * 1.5f);
                jumpdur = 5f/8;
            }else if (OnLWall())
            {
                walljumpdurationleft = initWallJumpDuration;
                rb.velocity = new Vector2(jumpforce * 7f, 1.3f * jumpforce * 1.5f);
                dirX = 1;
                ljumping = true;
                rjumping = false;

            }else if (OnRWall())
            {
                walljumpdurationleft = initWallJumpDuration;
                dirX = -1;

                rb.velocity = new Vector2(-jumpforce * 7f , 1.3f * jumpforce * 1.5f);
                ljumping = false;
                rjumping = true;

            }else if (djavail)
            {
                rb.velocity = new Vector2(rb.velocity.x, 7 * jumpforce * 30/18);

                djavail = false;
               
                jumpdur = -100 / 80;

            }
        }




        
        if (Input.GetButtonUp("Jump"))
        {



            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y /2f);
            }

            jumpdur = -100;
        }



        if (walljumpdurationleft < 0)
        {
            rjumping = false;
            ljumping = false;
        }







        if (Input.GetKeyDown(KeyCode.LeftShift) && !pec.healing)
        {

            pec.abilityMoving = false;

            if (OnLWall()){
                dashable = false;
                dashing = true;
                mostrecdirX = 1;
                dirX = 1;
                rb.velocity = new Vector2(mostrecdirX * dashspeed, dashamountup);
                dashDur = initDashDur;
                
            }
            else if(OnRWall()) {
                dashable = false;
                dashing = true;
                mostrecdirX = -1;
                dirX = -1;
                rb.velocity = new Vector2(mostrecdirX * dashspeed, dashamountup);
                dashDur = initDashDur;

            }
            else if (dashable == true || IsGrounded())
            {
                if (dashDur < -145f / 80 / 10)
                {
                    dashDur = initDashDur;
                    dashing = true;
                    rb.velocity = new Vector2(mostrecdirX * dashspeed, dashamountup * 25/18);
                    dashable = false;

                }
            }

            firstFrameStartDash = true;



        }

        if (dashing)
        {
            rb.velocity = new Vector2(mostrecdirX * dashspeed, dashamountup * 25/18);
            jumpdur = -100 / 80;

        }else if(jumpdur > -100 / 80)
        {

            rb.velocity = new Vector2(rb.velocity.x, jforcemult * jumpforce * 25/18);
            jumpdur -= Time.deltaTime * 35f;


        }




        if (Input.GetKey(KeyCode.E) && !IsGrounded() && !Input.GetKey(KeyCode.Space) && !pec.healing)
        {
            rb.velocity = new Vector2(rb.velocity.x, hopspeed);
            hopspeed -= 200f * Time.deltaTime;
        }
        else
        {
            hopspeed = -150f;
        }

        

        if (Input.GetKeyUp(KeyCode.E) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }        

        if (Input.GetKeyDown(KeyCode.N))
        {
            rb.gravityScale *= -1;
            if (sprite.flipY == true) sprite.flipY = false;

            if (sprite.flipY == false) sprite.flipY = true;
        }





        if (justgrappledtimer > 0)
        {
            justgrappledtimer -= Time.deltaTime;
            justgrappled = true;
        }
        else
        {
            justgrappled = false;
        }



        
        if (grappling)
        {
            justgrappledtimer = 0.23f;
            djavail = true;
            //if(grappletimer == 0.3f)
            //{
                origy = rb.position.y;
                origx = rb.position.x;
            //}

            if(grappletimer <= 0)
            {
                
                grappling = false;
                rb.velocity = new Vector2(0, 0);
              
            }
            else
            {

                
                rb.velocity = new Vector2( (grapplemovespeedmult*(ghook.gx - origx)), (grapplemovespeedmult*(ghook.gy - origy)) * 25/18);

            }
        }

        grappletimer -=Time.deltaTime;









        UpdateAnim();


        
        if(damagecoll.hp == 0)
        {
            sprite.material.color = new Color(1, 1, 1);
        }
        else
        {

            if (damagecoll.cd > 0)
            {
                sprite.material.color = new Color(0.5f, 0.5f, 0.5f);
            }
            else if (damagecoll.parrying)
            {
                sprite.material.color = new Color(1, 1.25f, 1);

            }
            else if (damagecoll.parryFlourishing)
            {
                sprite.material.color = new Color(1, 1.5f, 1);

            }
            else if (dashing)
            {
                sprite.material.color = new Color(1, 1, 1);

                //sprite.material.color = new Color(0.5f, 1, 0.5f);
            }
            
            else
            {
                sprite.material.color = new Color(1, 1, 1);

            }
        }





        if (beinghit)
        {
            grappling = false;
            dcollCollider.isTrigger = true;
            rb.velocity = new Vector2(movespeed * 2 * dirhitfrom, movespeed * 25/18);
            justdashed = false;
            parrybdur = -1;
            dashing = false;
            fastRunning = false;
            attack.attackbegan = false;
            walljumpdurationleft = -1f / 5f;


           
        }



        




        if (hittimer <=0 && beinghit)
        {
           // if(!dashing && ! playerlife.parrying && !playerlife.parryFlourishing)damagecoll.isTrigger = false;

            beinghit = false;
        }

        /*
        if(damagecoll.cd > 0)
        {
            dcollCollider.isTrigger = true;
        }
        */




        //ATTACKBDUR RIGHT HERE??

        
        if (atkkbdur > 0)
        {

            if (dirpushedhorizonhit !=0 )rb.velocity = new Vector2( pushbackamt* dirpushedhorizonhit * 1f, rb.velocity.y);

            if (dirpushedvertonhit != 0) rb.velocity = new Vector2(rb.velocity.x, pushbackamt * dirpushedvertonhit * 25/18) ;

            if(dirpushedvertonhit > 0)
            {
                djavail = true;
            }

            atkkbdur -= Time.deltaTime;

        }
        else
        {
            dirpushedhorizonhit = 0;
            dirpushedvertonhit = 0;
        }

        if (parrybdur > 0)
        {
            parryhopresetted = false;

            parrybdur -= Time.deltaTime;
            if(dirForParryKbVert == 0)rb.velocity = new Vector2(movespeed * 2.75f * dirParriedFrom, rb.velocity.y);
            if (dirForParryKbVert != 0) rb.velocity = new Vector2(movespeed * 2.75f * dirParriedFrom, 6f * dirForParryKbVert * movespeed);

        }



        if(parrybdur < 0 && !parryhopresetted)
        {
            if(dirForParryKbVert != 0) dashable = true; djavail = true;
            dirForParryKbVert = 0;
            dirParriedFrom = 0;
            parryhopresetted = true;

        }   



        if (pec.healing && !beinghit && parrybdur < 0 )
        {
            rb.velocity = new Vector2(0,rb.velocity.y);

        }

        if (dashDur <= 1 / 10 && dashing)
        {
            dashing = false;
            //if (!damageScript.parryFlourishing && !beinghit) damagecoll.isTrigger = false;
        }


        //CHANGED HERE
        //if(!damagecoll.parryFlourishing && !beinghit && !dashing && damagecoll.cd < 0) dcollCollider.isTrigger = false;





    }





    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.velocity = new Vector2(0, 0);
            dashing = false;
            dcd = 0;
            coll.isTrigger = false;
        }
        */
    }

    void UpdateAnim(){
        MovementState state;


        if (beinghit)
        {
            state = MovementState.hit;
        }
        else
        {
            if (dirX > 0)
            {
                state = MovementState.running;
                if (dashing == true)
                {
                    state = MovementState.dash;
                }


                sprite.flipX = false;
            }
            else if (dirX < 0)
            {

                state = MovementState.running;



                if (dashing == true)
                {
                    state = MovementState.dash;
                }

                sprite.flipX = true;

            }
            else
            {

                state = MovementState.idle;

            }
            if (rb.velocity.y > 0.001f)
            {

                state = MovementState.jumping;

            }

            else if (rb.velocity.y < -.001f)

            {
                state = MovementState.falling;
            }

            if(OnRWall() || OnLWall())
            {
                state = MovementState.wallsliding;

                if (OnRWall())
                {

                    sprite.flipX = true;
                }
                else
                {
                    sprite.flipX = false;
                }
            }

            if (dashing)
            {
                state = MovementState.dash;
            }


            if (Input.GetKeyDown(KeyCode.G))
            {
                state = MovementState.honored;  
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                state = MovementState.attacking;
            }

            if (pec.healing)
            {
                state = MovementState.idle;
            }
        }


        anim.SetInteger("state", (int)state);
    }



    private Boolean IsGrounded()
    {
        if (Physics2D.BoxCast(coll.bounds.center, coll.bounds.size * 0.99f, 0f, Vector2.down, 0.1f, jumpableground))
        {
            if(updlastloccd <= 0)
            {
                lastgrndlocx = rb.position.x;
                lastgrndlocy = rb.position.y;
                updlastloccd = 180;
            }

            //Debug.DrawRay(coll.bounds.center, Vector2.down * 0.1f, Color.red);


            djavail = true;
            dashable = true;
            return true;
        }

        if (Physics2D.BoxCast(coll.bounds.center, coll.bounds.size * 0.99f, 0f, Vector2.down, 0.1f, projJumpableGround))
        {
            if (updlastloccd <= 0)
            {
                lastgrndlocx = rb.position.x;
                lastgrndlocy = rb.position.y;
                updlastloccd = 180;
            }

            Debug.DrawRay(coll.bounds.center, Vector2.down * 0.1f, Color.red);


            djavail = true;
            dashable = true;
            return true;
        }

        return false;

    }




    //THE TWO WALL FUNCTIONS ONLY RUN IF YOU'RE ALSO NOT ON THE GROUND



    private Boolean OnRWall()
    {
        if ((Physics2D.BoxCast(coll.bounds.center, coll.bounds.size * 0.99f, 0f, Vector2.right, 0.1f, jumpableground)) && !IsGrounded())
        {
            dashable = true;
            djavail = true;

            return true;
        }

        if ((Physics2D.BoxCast(coll.bounds.center, coll.bounds.size * 0.99f, 0f, Vector2.right, 0.1f, projJumpableGround)) && !IsGrounded())
        {
            
            dashable = true;
            djavail = true;
            return true;
        }

        return false;
    }


    private Boolean OnLWall()
    {
        if ((Physics2D.BoxCast(coll.bounds.center, coll.bounds.size * 0.99f, 0f, Vector2.left, 0.1f, jumpableground)) && !IsGrounded())
        {
            dashable = true;
            djavail = true;

            return true;
        }


        if ((Physics2D.BoxCast(coll.bounds.center, coll.bounds.size * 0.99f, 0f, Vector2.left, 0.1f, projJumpableGround)) && !IsGrounded())
        {
            dashable = true;
            djavail = true;
            return true;
        }


        return false;
    }










    GameObject objectRightCasted;
    GameObject objectLeftCasted;
    GameObject objectUpCasted;
    GameObject objectDownCasted;

    float rightShift = 1f;
    ContactFilter2D filter = new ContactFilter2D();



    float boxCastvalScale = 0.7f;


    private void callAllgrndIrrel()
    {
        OnRWallGroundIrrelevant();
        OnLWallGroundIrrelevant();
        OnRWallProjGroundIrrelevant();
        OnLWallProjGroundIrrelevant();
        OnCeilProjGroundIrrelevant();
        OnFloorProjGroundIrrelevant();
        OnCeilGroundIrrelevant();
        OnFloorGroundIrrelevant();
    }
    private Boolean OnRWallProjGroundIrrelevant()
    {  
        Vector2 castCheckStartPos = (Vector2)coll.bounds.center + Vector2.right * rightShift;

        if ((Physics2D.BoxCast(castCheckStartPos, coll.bounds.size*boxCastvalScale, 0f, Vector2.right, 0.1f, projJumpableGround)))
        {
            objectRightCasted = Physics2D.BoxCast(castCheckStartPos, coll.bounds.size * boxCastvalScale, 0f, Vector2.right, 0.1f, projJumpableGround).collider.gameObject;

            return true;
        }


        objectRightCasted = null;
        return false;
    }


    private Boolean OnLWallProjGroundIrrelevant()
    {

        Vector2 castCheckStartPos = (Vector2)coll.bounds.center + Vector2.left * rightShift;
        if ((Physics2D.BoxCast(castCheckStartPos, coll.bounds.size * boxCastvalScale, 0f, Vector2.left, 0.1f, projJumpableGround)))
        {
            
            objectLeftCasted = Physics2D.BoxCast(castCheckStartPos, coll.bounds.size * boxCastvalScale, 0f, Vector2.left, 0.1f, projJumpableGround).collider.gameObject;

            return true;
        }

        objectLeftCasted = null;

        return false;
    }


    private Boolean OnCeilProjGroundIrrelevant()
    {
        Vector2 castCheckStartPos = (Vector2)coll.bounds.center + Vector2.up * rightShift;

        if ((Physics2D.BoxCast(castCheckStartPos, coll.bounds.size * boxCastvalScale, 0f, Vector2.up, 0.1f, projJumpableGround)))
        {
            objectUpCasted = Physics2D.BoxCast(castCheckStartPos, coll.bounds.size * boxCastvalScale, 0f, Vector2.up, 0.1f, projJumpableGround).collider.gameObject;
            return true;
        }


        objectUpCasted = null;
        return false;
    }


    private Boolean OnFloorProjGroundIrrelevant()
    {

        Vector2 castCheckStartPos = (Vector2)coll.bounds.center + Vector2.down * rightShift;

        if ((Physics2D.BoxCast(castCheckStartPos, coll.bounds.size * boxCastvalScale, 0f, Vector2.down, 0.1f, projJumpableGround)))
        {

            objectDownCasted = Physics2D.BoxCast(castCheckStartPos, coll.bounds.size * boxCastvalScale, 0f, Vector2.down, 0.1f, projJumpableGround).collider.gameObject;
            return true;
        }

        objectDownCasted = null;

        return false;
    }

    private Boolean OnRWallGroundIrrelevant()
    {
        Vector2 castCheckStartPos = (Vector2)coll.bounds.center + Vector2.right * rightShift;

        if ((Physics2D.BoxCast(castCheckStartPos, coll.bounds.size * boxCastvalScale, 0f, Vector2.right, 0.1f, jumpableground)))
        {
            objectRightCasted = null;

            return true;

        }
        return false;
    }


    private Boolean OnLWallGroundIrrelevant()
    {
        Vector2 castCheckStartPos = (Vector2)coll.bounds.center + Vector2.left * rightShift;

        if ((Physics2D.BoxCast(castCheckStartPos, coll.bounds.size * boxCastvalScale, 0f, Vector2.left, 0.1f, jumpableground)))
        {
            objectLeftCasted = null;

            return true;
        }

        return false;
    }

    private Boolean OnFloorGroundIrrelevant()

    {
        Vector2 castCheckStartPos = (Vector2)coll.bounds.center + Vector2.down * rightShift;

        if ((Physics2D.BoxCast(castCheckStartPos, coll.bounds.size * boxCastvalScale, 0f, Vector2.down, 0.1f, jumpableground)))
        {
            objectDownCasted = null;

            return true;

        }


        return false;
    }


    private Boolean OnCeilGroundIrrelevant()
    {
        Vector2 castCheckStartPos = (Vector2)coll.bounds.center + Vector2.up * rightShift;

        if ((Physics2D.BoxCast(castCheckStartPos, coll.bounds.size * boxCastvalScale, 0f, Vector2.up, 0.1f, jumpableground)))
        {
            objectUpCasted = null;

            return true;
        }

        return false;
    }
    private void SqueezeDamageCheck()
    {


        if (OnRWallProjGroundIrrelevant() || OnLWallProjGroundIrrelevant())
        {
            if ((OnRWallProjGroundIrrelevant() && OnLWallProjGroundIrrelevant()) || (OnRWallProjGroundIrrelevant() && OnLWallGroundIrrelevant()) || (OnRWallGroundIrrelevant() && OnLWallProjGroundIrrelevant()))
            {

                if (transform.parent != null && transform.parent.gameObject.CompareTag("Wall"))
                {
                    transform.SetParent(null);
                }

                Debug.Log("ITS WORKING");
                if (objectLeftCasted != null) objectLeftCasted.SetActive(false);
                if (objectRightCasted != null) objectRightCasted.SetActive(false);

                Destroy(objectRightCasted);
                Destroy(objectLeftCasted);


                objectLeftCasted = null;
                objectRightCasted = null;
                damagecoll.manualTakeDamage(0, 1);

            }

        }







        if (OnCeilProjGroundIrrelevant() || OnFloorProjGroundIrrelevant())
        {
            if ((OnCeilProjGroundIrrelevant() && OnFloorProjGroundIrrelevant()) || (OnCeilProjGroundIrrelevant() && OnFloorGroundIrrelevant()) || (OnCeilGroundIrrelevant() && OnFloorProjGroundIrrelevant()))
            {

                if (transform.parent != null && transform.parent.gameObject.CompareTag("Wall"))
                {
                    transform.SetParent(null);
                }

                if (objectUpCasted != null) objectUpCasted.SetActive(false);
                if (objectDownCasted != null) objectDownCasted.SetActive(false);

                Destroy(objectUpCasted);
                Destroy(objectDownCasted);
                objectDownCasted = null;
                objectUpCasted = null;
                damagecoll.manualTakeDamage(0, 1);
            }

        }
    }


    private void OnDrawGizmos()
    {
        if (coll == null) return;

        Vector2 center = coll.bounds.center;
        Vector2 size = coll.bounds.size * boxCastvalScale;

        // "Proj" ground casts — origin offset by rightShift along the cast direction
        DrawBoxCastGizmo(center + Vector2.right * rightShift, size, Vector2.right, 0.1f, Color.cyan);   // RWallProj
        DrawBoxCastGizmo(center + Vector2.left * rightShift, size, Vector2.left, 0.1f, Color.cyan);   // LWallProj
        DrawBoxCastGizmo(center + Vector2.up * rightShift, size, Vector2.up, 0.1f, Color.cyan);   // CeilProj
        DrawBoxCastGizmo(center + Vector2.down * rightShift, size, Vector2.down, 0.1f, Color.cyan);   // FloorProj

        // Regular ground casts — origin is bounds.center, no shift
    }

    private void DrawBoxCastGizmo(Vector2 origin, Vector2 size, Vector2 direction, float distance, Color color)
    {
        Gizmos.color = color;

        // box at the start of the cast
        Gizmos.DrawWireCube(origin, size);

        // box at the end of the cast, after traveling `distance` along `direction`
        Vector2 end = origin + direction.normalized * distance;
        Gizmos.DrawWireCube(end, size);

        // connect corresponding corners so the swept volume is visible, not just two floating boxes
        Vector2 half = size * 0.5f;
        Vector2[] offsets =
        {
        new Vector2(-half.x, -half.y),
        new Vector2(-half.x,  half.y),
        new Vector2( half.x, -half.y),
        new Vector2( half.x,  half.y),
    };

        foreach (var offset in offsets)
        {
            Gizmos.DrawLine(origin + offset, end + offset);
        }
    }
}


