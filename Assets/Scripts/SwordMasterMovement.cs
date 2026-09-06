using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.UI.Image;

public class SwordMasterMovement : MonoBehaviour
{


    [SerializeField] private LayerMask jumpableground;


    

    

    private enum AnimState { idle, running, jumping, falling,attacking1,attacking2, dashing }


    List<GameObject> ghostSwords = new List<GameObject>();


    [SerializeField] GameObject SMGhostImage;

    float waittimer = 0.4f;
    [SerializeField] GameObject ghostSword;



    float runspeedincrease = 0f;
    bool firstFrame = true;


    float dirmoving;
    [SerializeField] GameObject slideSword;
    [SerializeField] GameObject bossproj;
    [SerializeField] GameObject tracerProj;


    float dirChangeTimer = 0.1f;



    float dbtimer = 4f;

    float downplungetimer = 0.75f;



    List<GameObject> tracerProjs = new List<GameObject>();

    float bossprojshootcd = 0.015f;
    float downdashdur = 0.5f;


    float initplayerx = 0f;


    float dashspeed = 50f;


    bool pxset = false;

    float osumcd = 0.05f;

    float hopbackdur = 0.4f;
    float sidedashdur = 0.7f;
    BoxCollider2D coll;
    Rigidbody2D rb;


    float jumpspeed = 75f;

    float plungespeed = 0f;

    float orbsummnum = 3f;
    SpriteRenderer sprite;
    float waitfill = 0.6f;
    float bjspeed = 100f;

    float ds1timer = 0.3f;
    float ds2timer = 0.5f;

    Boolean dashed = false;


    float waitpredswingtimer = 0.15f;

    Boolean tped = false;


    float movespeed = 90f;
    float gswordsummoncd = 0.3f;
    int gswordcount = 15;
    float aurafarmtime = 0.7f;
    float waitpredashtimer = 0.1f;
    float dashtimer = 0.4f;
    float bjtimer = 0.2f;
    dcoll damagecoll;



    
    int val;
    Animator anim;


    float jumpUpDur = 0.75f;

    float plungeDur = 0.5f;
    public float dirX;

    Transform tf;
    PlayerMovement player;

    Vector3 initscale;

    bool runchose = false;
    bool lastatkended = true;

    string[] layerNames = { "Ground" };

    bool beginnext = false;
    string[] layerNames2 = { "Player" };

    bool isGrounded;

    Vector2 directionToPlayer = new Vector2(0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        anim= GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        tf = GetComponent<Transform>(); 
        player = FindObjectOfType<PlayerMovement>();
        sprite  = GetComponent<SpriteRenderer>();   
        damagecoll= FindObjectOfType<dcoll>();
        initscale = transform.localScale;

        
    }

    float lastval = 0f;


    float disttoplayer = 0f;
    // Update is called once per frame
    void Update()
    {

        disttoplayer = Vector2.Distance(rb.position, player.rb.position);

        float hdiste = -(rb.position.x - player.gameObject.GetComponent<Rigidbody2D>().position.x);

        isGrounded =  IsGrounded();



        if (lastatkended )
        {
            anim.SetInteger("animState", (int)AnimState.idle);

            dirX = Mathf.Sign(-(rb.position.x - player.gameObject.GetComponent<Rigidbody2D>().position.x));
            //Debug.Log(dirX);

            float hdist = -(rb.position.x - player.gameObject.GetComponent<Rigidbody2D>().position.x);



            if (dirX > 0)
            {
                transform.localScale = initscale;
            }
            else
            {
                transform.localScale = new Vector3(-initscale.x, initscale.y, initscale.z);

            }




            if (Mathf.Abs(hdist) > 35f)
            {

                if (!runchose)
                {
                    if (UnityEngine.Random.Range(0, 3) == 1)
                    {
                        runchose = true;
                    }
                    else
                    {
                        lastatkended = false;
                        val = UnityEngine.Random.Range(-5, 0);

                        if(UnityEngine.Random.Range(0,3) == 0)
                        {
                            val = -4;
                        }
                        waittimer = 0.4f;

                    }
                }



                if (runchose)
                {

                    runspeedincrease += 30f * Time.deltaTime;
                    anim.SetInteger("animState", (int)AnimState.running);

                    rb.velocity = new Vector2(movespeed * dirX + runspeedincrease * dirX, rb.velocity.y);

                }


            }
            else
            {

                runchose = false;
                lastatkended = false;

                waittimer = 0.4f;
                val = UnityEngine.Random.Range(1, 4);

                if(lastval == 0 && val == 0)
                {
                    val = 1;
                }

                anim.SetFloat("speed", 1);

            }

        }
        else
        {
            if(waittimer > 0)
            {
                waittimer -= Time.deltaTime;

            }
            else
            {
                runspeedincrease = 0f;


                if(val == -5)
                {
                    teleportsBehindYou();
                }

                if(val == -4)
                {
                    tracerProjSumm();
                }

                if (val == -3)
                {
                    upThenPlunge();
                }
                if (val == -2)
                {
                    ProjSumm();
                }

                if (val == -1)
                {
                    swordSummon();
                }


                if (val == 1)
                {
                    BackJumpSwing();
                }
                if (val == 2)
                {
                    doubleSwing();
                }

                if (val == 3)
                {
                    upThenPlunge();
                }

                if(val == 4)
                {
                    tracerProjSumm();
                }

                lastval = val;
            }

        }

        

    }




    void upThenPlunge()
    {


        //Debug.Log(pxset);


        //Debug.Log("Speed : " + (initplayerx - rb.position.x) / 10f);


        if (!pxset)
        {
            initplayerx = player.rb.position.x;

            pxset = true;
        }


        if (jumpUpDur > 0)
        {

            jumpspeed-=75f * Time.deltaTime;

            rb.velocity = new Vector2(( initplayerx - rb.position.x)/ 10f * jumpspeed, Mathf.Abs((initplayerx - rb.position.x) / 10f * jumpspeed));

            jumpUpDur -= Time.deltaTime;

        }
        else if(plungeDur  > 0)
        {

            rb.velocity = new Vector2(0f, -plungespeed);

            plungespeed += 750f * Time.deltaTime;

            plungeDur -= Time.deltaTime;

            Vector2 p1 = new Vector2(rb.position.x + 10, rb.position.y-5);
            Vector2 p2 = new Vector2(rb.position.x - 10, rb.position.y-5);


            if (bossprojshootcd <= 0f)
            {


                

                b1Atkscr bull1 = Instantiate(bossproj, p1, Quaternion.identity).GetComponent<b1Atkscr>();
                b1Atkscr bull2 = Instantiate(bossproj, p2, Quaternion.identity).GetComponent<b1Atkscr>();
                bull1.xdir = 1;
                bull2.xdir = -1;

                bull1.GetComponent<BoxCollider2D>().enabled = true;
                bull2.GetComponent<BoxCollider2D>().enabled = true;

                

                bull1.speed = 75 + 25f * UnityEngine.Random.Range(1f, 2f);
                bull2.speed = 75 + 25f * UnityEngine.Random.Range(1f, 2f);
                bull2.accelrate = 75 + 100f * UnityEngine.Random.Range(1f, 2f);
                bull1.accelrate = 75 + 100f * UnityEngine.Random.Range(1f, 2f); ;

                bossprojshootcd = 0.015f;
            }

            bossprojshootcd -= Time.deltaTime;  

        }
        else
        {
            lastatkended = true;
            jumpspeed = 75f;
            plungespeed = 0f;
            jumpUpDur = 0.75f;
            plungeDur = 0.5f;

            pxset = false;
        }

        if(jumpUpDur <= 0)
        {
            anim.SetInteger("animState", (int)AnimState.falling);

            if (isGrounded)
            {
                lastatkended = true;
                jumpspeed = 75f;
                plungespeed = 0f;
                jumpUpDur = 0.75f;
                plungeDur = 0.5f;

                pxset = false;
            }
        }
        else
        {
            anim.SetInteger("animState", (int)AnimState.jumping);
        }

    }


    float tpcd = 0.005f;

    float tpcount = 70f;
    float movetimer = 0.3f;

    float teleportmspeed = 30f;

    float slidetimer = 0.5f;
    void teleportsBehindYou()
    {

        if (movetimer > 0)
        {
            rb.velocity = new Vector2(teleportmspeed * dirX, 0f);
            movetimer -= Time.deltaTime;

            teleportmspeed += 450f * Time.deltaTime;    
        }
        else
        {




            tpcd -= Time.deltaTime;
            if (tpcd < 0 && tpcount > 0)
            {
                teleportmspeed = 0f;
                tpcd = 0.005f;
                tpcount--;


                if (tpcount % 2 == 0)
                {
                    transform.position = new Vector2(player.rb.position.x + 50f * UnityEngine.Random.Range(0.4f, 2f), player.rb.position.y + 5f);
                    transform.localScale = new Vector3(-1 * initscale.x, initscale.y, initscale.z);
                    dirX = -1;

                }
                else
                {
                    transform.position = new Vector2(player.rb.position.x - 50f * UnityEngine.Random.Range(0.4f, 2f), player.rb.position.y + 5f);
                    transform.localScale = new Vector3(initscale.x, initscale.y, initscale.z);

                    dirX = 1;

                }

                Instantiate(SMGhostImage, rb.position, Quaternion.identity);

                rb.velocity = new Vector2(dirX * 100f, 0f);


            }



            if (tpcount == 0) {
                transform.position = new Vector2(player.rb.position.x + 40f * player.mostrecdirX * -1, player.rb.position.y + 5f);

                if(player.mostrecdirX == -1)
                {
                    transform.localScale = new Vector3(-1 * initscale.x, initscale.y, initscale.z);
                    dirX = -1;
                }
                else
                {
                    transform.localScale = new Vector3(initscale.x, initscale.y, initscale.z);
                    dirX = 1;
                }


                tpcount--;
            }


            if (tpcount < 0)
            {
                Debug.Log(dirX);
                anim.SetInteger(name: "animState", (int)AnimState.attacking1);

                slidetimer -= Time.deltaTime;

                rb.velocity = new Vector2(teleportmspeed * dirX, 0f);
                 teleportmspeed += 450f * Time.deltaTime;

                if (slidetimer < 0)
                {
                    lastatkended = true;

                    teleportmspeed = 30f;
                    movetimer = 0.3f;
                    tpcount = 70f;
                    slidetimer = 0.5f;

                    dirX = Mathf.Sign(player.rb.position.x - rb.position.x);


                    if (dirX > 0)
                    {
                        transform.localScale = initscale;
                    }
                    else
                    {
                        transform.localScale = new Vector3(-initscale.x, initscale.y, initscale.z);

                    }
                }

            }
        }
    }

    void doubleSwing()

    {


        
        waitpredswingtimer -= Time.deltaTime;
        if (waitpredswingtimer < 0)
        {



            if (ds1timer > 0)
            {
                ds1timer -= Time.deltaTime;
                anim.SetInteger("animState", (int)AnimState.attacking1);

                if (ds1timer <= 0.1f)
                {
                    rb.velocity = new Vector2(100f * dirX, rb.velocity.y);
                }
                if (ds1timer <= 0.03f)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }

            }
            else if (ds2timer > 0)
            {

                if (ds2timer <= 0.2)
                {
                    rb.velocity = new Vector2(200f * dirX, rb.velocity.y);
                }
                if (ds2timer <= 0.1f)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
                ds2timer -= Time.deltaTime;
                anim.SetInteger("animState", (int)AnimState.attacking2);
            }
            else
            {
                ds1timer = 0.3f;
                ds2timer = 0.5f;
                lastatkended = true;

                waitpredswingtimer = 0.15f;
                anim.SetInteger("animState", (int)AnimState.idle);
            }
        }
        else
        {
            anim.SetInteger("animState", (int)AnimState.idle);
        }
    }


    void BackJumpSwing()
    {
        if (bjtimer > 0)
        {
            anim.SetInteger("animState", (int)AnimState.idle);

            bjspeed -= Time.deltaTime * 50f;

            rb.velocity = new Vector2(-bjspeed * dirX, rb.velocity.y);

            bjtimer -= Time.deltaTime;
        }


        else if (waitpredashtimer > 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            anim.SetInteger("animState", (int)AnimState.idle);

            /*
            if(waitpredashtimer < 0.07f)
            {
                anim.SetInteger("animState", (int)AnimState.attacking1);
            }
            */


            waitpredashtimer -= Time.deltaTime;
            bjspeed = 30f;

        }
        else if (dashtimer > 0)
        {
            //anim.SetInteger("animState", (int)AnimState.attacking1);

            bjspeed += Time.deltaTime * 50f;
            rb.velocity = new Vector2(bjspeed * dirX, rb.velocity.y);


            dashtimer -= Time.deltaTime;
        }
        else
        {


            if (!dashed)
            {
                dashed = true;

                rb.velocity = new Vector2(0, rb.velocity.y);


                Vector2 boxsize = new Vector2(5f, 5f);
                float dashdist = 50f;

                Vector2 direction = new Vector2(dirX, 0f);
                ContactFilter2D filter = new ContactFilter2D();
                filter.useLayerMask = true;
                filter.layerMask = LayerMask.GetMask("Enemy");




                RaycastHit2D z = Physics2D.BoxCast(rb.position, boxsize, 0f, direction, dashdist, LayerMask.GetMask(layerNames));

                RaycastHit2D pz = Physics2D.BoxCast(rb.position, boxsize, 0f, direction, dashdist, LayerMask.GetMask(layerNames2));


                Debug.DrawRay(rb.position, direction);

                if (z.collider == null)
                {


                    //Debug.Log("No hit");

                    if (pz.collider != null)
                    {

                        Debug.Log(pz.collider.name);
                        damagecoll.manualTakeDamage(rb.position.x, 2);
                    }

                    rb.position = new Vector2(rb.position.x + (dashdist * dirX), rb.position.y);


                }
                else
                {

                    Debug.Log(z.point.x + " " + z.point.y);

                    rb.position = new Vector2(z.point.x + Mathf.Sign(rb.position.x - z.point.x) * 20f, rb.position.y);




                    if (pz.collider != null)
                    {
                        damagecoll.manualTakeDamage(rb.position.x, 2);
                    }
                }
            }
            else
            {
                if (aurafarmtime > 0)
                {




                    aurafarmtime -= Time.deltaTime;


                    bjspeed -= 50 * Time.deltaTime;
                    rb.velocity = new Vector2(bjspeed * dirX, rb.velocity.y);


                }
                else
                {

                    anim.SetFloat("speed", 1);
                    anim.SetInteger("animState", (int)AnimState.idle);


                    lastatkended = true;
                    waitpredashtimer = 0.1f;
                    dashtimer = 0.4f;
                    bjtimer = 0.2f;
                    bjspeed = 100f;
                    aurafarmtime = 0.7f;
                    dashed = false;
                }
            }





        }



    }
    

    void swordSummon()
    {
        rb.velocity = new Vector2(0,rb.velocity.y);
        if (gswordsummoncd <= 0 && gswordcount > 0)
        {
            anim.SetInteger("animState", (int)AnimState.attacking1);

            if(UnityEngine.Random.Range(0,2) == 0)
            {
                //SoundManager.PlaySound("swordwhoosh1");

                GameObject gsword = Instantiate(ghostSword, new Vector2(player.rb.position.x + 40f * UnityEngine.Random.Range(-1f, 1f), rb.position.y - 10f), Quaternion.identity);
                gsword.GetComponent<sliceGhost>().begin = true;
                gsword.GetComponent<BoxCollider2D>().isTrigger = true;

                gsword.transform.Rotate(0, 0, 0f);
                ghostSwords.Add(gsword);
                gswordsummoncd = 0.1f;
                gswordcount -= 1;
            }
            else
            {

                GameObject gsword = Instantiate(ghostSword, new Vector2(player.rb.position.x + 400f, player.rb.position.y + 40f * UnityEngine.Random.Range(0f, 1f)), Quaternion.identity);
                gsword.GetComponent<sliceGhost>().begin = true;
                gsword.GetComponent<BoxCollider2D>().isTrigger = true;

                gsword.transform.Rotate(0, 0, 90f);
                ghostSwords.Add(gsword);
                gswordsummoncd = 0.1f;
                gswordcount -= 1;
            }


        }
        else
        {
            anim.SetInteger("animState", (int)AnimState.attacking2);

            gswordsummoncd -= Time.deltaTime;
          
        }

        if(gswordcount == 0 && waitfill >= 0)
        {
            waitfill -= Time.deltaTime;
        }


        if (gswordcount == 0 && waitfill < 0)
        {
            while(ghostSwords.Count > 0)
            {
                ghostSwords[0].GetComponent<sliceGhost>().fill();
                ghostSwords.RemoveAt(0);
            }

            gswordcount = 15;
            lastatkended = true;

            waitfill = 0.6f;

        }
    }


    /*void downDash()
    {
        /*
        if (!tped)
        {
            rb.position = new Vector2(player.rb.position.x + 15f * dirX * -1, player.rb.position.y + 30f);

            anim.SetInteger("animState", (int)AnimState.idle);
            tped = true;
        }
        else
        {
            if (downdashdur > 0)
            {
                downdashdur -= Time.deltaTime;

                rb.velocity = new Vector2(-dirX * dashspeed, dashspeed * -1);


                dashspeed += 200f * Time.deltaTime;

                if (downdashdur < 0.1f)
                {
                    anim.SetInteger("animState", (int)AnimState.dashing);
                    dashspeed = 50f;
                }
            }
            if(sidedashdur > 0)
            {
               
                rb.velocity = new Vector2(dirX * dashspeed, -50f);

                    dashspeed += 300f*Time.deltaTime;

                if(sidedashdur < 0.2f)
                {
                    anim.SetInteger("animState", (int)AnimState.idle);

                    dashspeed -= 1000f * Time.deltaTime;    

                }

                sidedashdur -= Time.deltaTime;

            }
            else
            {

                dashspeed = 100f;
                lastatkended = true;
                anim.SetInteger("animState", (int)AnimState.idle);


                rb.velocity = new Vector2(0, rb.velocity.y); 
                downdashdur = 0.5f;
                tped = false;
                sidedashdur = 0.7f;
            }
        
    }
            */


    void hopBack()
    {
        Debug.Log("hopping");
        if(hopbackdur < 0)
        {
            lastatkended = true;
            hopbackdur = 0.4f;
        }
        hopbackdur -= Time.deltaTime;

        rb.velocity = new Vector2(-(dirX * 50f + dirX * 150f * hopbackdur), rb.velocity.y);
    }

    void ProjSumm()
    {

        if (orbsummnum == 3)
        {
            anim.SetInteger("animState", (int)AnimState.attacking1);
        }
        if (osumcd < 0  && orbsummnum > 0)
        {
            GameObject proj = Instantiate(slideSword, new Vector2(rb.position.x + 20f * dirX + dirX * (8 - orbsummnum) * - 5, rb.position.y), Quaternion.identity);
            proj.GetComponent<slidesword>().dirX = (int)dirX;

            proj.GetComponent<slidesword>().speed = 50f;
            proj.GetComponent<slidesword>().accelrate = 150f + (3- orbsummnum)*50f;
            proj.GetComponent<slidesword>().scalegrowthrate = 60f;


            orbsummnum -= 1;
            osumcd = 0.001f;

        }


        osumcd -= Time.deltaTime;

        if(osumcd < -1f)
        {
            anim.SetInteger("animState", (int)AnimState.idle);
        }   

        if (orbsummnum <= 0 && osumcd < -1f)
        {
            orbsummnum = 3f;
            lastatkended = true;
        }

        
    }

    int tracerProjNum = 14;


    float tracerjumpspeed = 100f;

    float tracerWaittimer = 0.6f;

    float jumptimer = 0.2f;
    

    void tracerProjSumm()
    {

        rb.gravityScale = 0f;

        rb.velocity = new Vector2(0, 0);



        if (jumptimer > 0)
        {
            jumptimer -= Time.deltaTime;
            rb.velocity = new Vector2(tracerjumpspeed * dirX * -1, tracerjumpspeed);

            tracerjumpspeed -= 350f * Time.deltaTime;

            if(tracerjumpspeed < 0f)
            {
                tracerjumpspeed = 0f;
            }
            return;
        }else

        if (tracerProjNum > 0)
        {


                GameObject proj = Instantiate(tracerProj, new Vector2(rb.position.x, rb.position.y + 5f), Quaternion.identity);
                proj.GetComponent<tracerLineProjMovement>().degreeOfRotation = 25-5*(14-tracerProjNum);

                proj.GetComponent<tracerLineProjMovement>().dirX = (int)dirX;
                proj.GetComponent<tracerLineProjMovement>().waitTimer = 0.2f;
                proj.GetComponent<SpriteRenderer>().enabled = false;
                proj.GetComponent<BoxCollider2D>().enabled = false;



                //proj.GetComponent<tracerLineProjMovement>().speed = 80f;
                tracerProjNum -= 1;

                tracerProjs.Add(proj);
                return;
            


        }
        else


        {
            if (tracerWaittimer > 0f)
            {
                tracerWaittimer -= Time.deltaTime;
            }
            else
            {

                rb.gravityScale = 55f;
                jumptimer = 0.2f;
                tracerProjNum = 14;
                tracerWaittimer = 0.6f;
                lastatkended = true;
                firstFrame = true;
                tracerjumpspeed = 100f;
            }
        }


        if(tracerProjNum  == 0)
        {

            Debug.Log(tracerProjs.Count);
            for (int i = 0; i < tracerProjs.Count; i++)
            {
                tracerProjs[i].GetComponent<tracerLineProjMovement>().begin = true;
            }



            tracerProjs.Clear();

            Debug.Log(tracerProjs.Count);
        }
    }






    private Boolean IsGrounded()
    {
        Vector2 newvec = new Vector2(coll.bounds.center.x, coll.bounds.center.y - 0.5f);
        Vector2 sizevec = new Vector2(coll.bounds.size.x - 5f, coll.bounds.size.y - 0.5f);
        if (Physics2D.BoxCast(newvec, sizevec, 0f, Vector2.down, 0.1f, jumpableground))
        {
            return true;
        }

        return false;

    }
    private Boolean OnRWall()
    {
        Vector2 newvec = new Vector2(coll.bounds.center.x + 0.5f, coll.bounds.center.y);
        Vector2 sizevec = new Vector2(coll.bounds.size.x - 0.5f, coll.bounds.size.y - 5f);
        if (Physics2D.BoxCast(newvec, sizevec, 0f, Vector2.right, 0.1f, jumpableground))
        {
            return true;
        }
        return false;

    }

    private Boolean OnLWall()
    {
        Vector2 newvec = new Vector2(coll.bounds.center.x - 0.5f, coll.bounds.center.y);
        Vector2 sizevec = new Vector2(coll.bounds.size.x - 0.5f, coll.bounds.size.y - 5f);
        if (Physics2D.BoxCast(newvec, sizevec, 0f, Vector2.left, 0.1f, jumpableground))
        {
            return true;
        }

        return false;
    }


    private Boolean OnCeiling()
    {
        Vector2 newvec = new Vector2(coll.bounds.center.x, coll.bounds.center.y + 0.5f);
        Vector2 sizevec = new Vector2(coll.bounds.size.x - 5f, coll.bounds.size.y - 0.5f);

        if (Physics2D.BoxCast(newvec, sizevec, 0f, Vector2.up, 0.1f, jumpableground))
        {
            return true;
        }

        return false;

    }























    /*
    void doubleBouncePlunge()
    {



        dirChangeTimer -= Time.deltaTime;


        if (firstFrame)
        {

            dirmoving = Mathf.Sign(UnityEngine.Random.Range(-1f, 1f));
            firstFrame = false;
        }


        if (dbtimer > 0)
        {
            sticktowallspeed -= 100f * Time.deltaTime;



            if ((OnLWall() || OnRWall()))
            {
                dbtimer = -1;
            }

            if (OnLWall())
            {
                dirX = 1;
            }

            if (OnRWall())
            {
                dirX = -1;
            }


            if (dirX > 0)
            {
                transform.localScale = initscale;
            }
            else
            {
                transform.localScale = new Vector3(-initscale.x, initscale.y, initscale.z);

            }


            rb.velocity = new Vector2(dirmoving * sticktowallspeed, sticktowallspeed / 2);



            dbtimer -= 5 * Time.deltaTime;

        }
        else if (downplungetimer > 0)
        {

            if (disttoplayer < 50f) anim.SetInteger("animState", (int)AnimState.attacking2);


            if (downplungetimer == 2f)
            {

                sticktowallspeed = 30f;
                dirX = Mathf.Sign(-(rb.position.x - player.gameObject.GetComponent<Rigidbody2D>().position.x));
                directionToPlayer = new Vector2(player.rb.position.x - rb.position.x, player.rb.position.y - rb.position.y + 10f).normalized;
            }



            rb.velocity = directionToPlayer * sticktowallspeed;

            sticktowallspeed += 300f * Time.deltaTime;


            downplungetimer -= Time.deltaTime;

            if (IsGrounded() || ((OnLWall() || OnRWall()) && downplungetimer < 1.9f))
            {
                downplungetimer = -1f;

                rb.velocity = new Vector2(0f, 0f);
            }
        }


        else if (downplungetimer < 0)
        {
            lastatkended = true;
            dbtimer = 4;

            downplungetimer = 2f;


            sticktowallspeed = 600f;
            firstFrame = true;
        }
    }
    */
}


