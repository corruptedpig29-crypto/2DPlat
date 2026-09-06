using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UIElements;

public class wizkingmovement : MonoBehaviour
{
    Animator animator;
    [SerializeField] GameObject x;
    [SerializeField] GameObject summonorb;
    [SerializeField] GameObject trackerorb;
    [SerializeField] GameObject haltSword;
    [SerializeField] LayerMask jumpableground;
    BoxCollider2D coll;
    [SerializeField] GameObject bossproj;
    bool running = false;
    float trackerbeamtimer = 1.5f;
    float orbsummontimer = 4.5f;
    [SerializeField] GameObject warningshooter;
    float haltswordtimer = 1f;
    SpriteRenderer sprite;
    int val = 0;
    Boolean lastatkended = true;
    int animval = 0;
    int hp = 10;
    float dirhitfrom = 0;
    int warningcount = 30;
    float hswordcount = 0f;
    float warningraystimer = 0.05f;


    float hswordcd = 0.1f;
    float damagetimer = 0;
    float fastballshootcd = 0.5f;
    int fastballshootcount = 3;
    float timer = 0.7f;
    float dashtimer = 2f;
    float dashspeed = 5f;
    float orbshootcd = 0.1f;
    int orbshootcount = 3;

    float trackerbeamcd = 0.05f;
    int trackerbeamprojnum = 7;

    bool extrashotfiredinorb = false;
    private enum MovementState { idle, attack1, attack2, walking};

    float orbcd = 1f;

    float summtrackorbfoot = 0.2f;

    Rigidbody2D rb;
    PlayerMovement findz;
    int dirX = 1;

    int lastfaceddirx = 1;
    // Start is called before the first frame update
    void Start()
    {
        //hp = 10;
        coll = GetComponent<BoxCollider2D>(); 
        animator = GetComponent<Animator>();   
        findz = FindObjectOfType<PlayerMovement>(); 
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        
        dirX = 1;
    }

    // Update is called once per frame



    void Update()

    {
        

        summtrackorbfoot -= Time.deltaTime;

        if (GetComponent<UniHpManager>().tookdamage == true && damagetimer <= 0)

        {

            dirhitfrom = (rb.position.x - findz.rb.position.x) / Math.Abs((rb.position.x - findz.rb.position.x));
            damagetimer = 0.2f;

            GetComponent<UniHpManager>().tookdamage = false;

            /*
            hp--;
            if (hp <= 0)
            {
                Destroy(this.gameObject);
            }
            */
        }


        if (damagetimer > 0) {
            damagetimer -= Time.deltaTime;
            //rb.velocity = new Vector2(dirhitfrom * 30f, 40f);
            return;
        }

        float disttoplayer = Mathf.Sqrt((rb.position.x - findz.rb.position.x) *(rb.position.x - findz.rb.position.x) + (rb.position.y - findz.rb.position.y) * (rb.position.y - findz.rb.position.y));


        UpdateAnim();

        if (lastatkended == true && !OnRWall() && !OnLWall())
        {
            if (disttoplayer > 100f)
            {


                running = true;
                animval = 3;

                if (findz.rb.position.x > rb.position.x)
                {
                    dirX = 1;
                }
                else if (findz.rb.position.x < rb.position.x)
                {
                    dirX = -1;

                }
                rb.velocity = new Vector2(40 * dirX, rb.velocity.y);


                UpdateAnim();
                if(summtrackorbfoot < 0)
                {
                    Vector2 p1 = new Vector2(rb.position.x, rb.position.y - 20f);
                    bossprojshooter warningshoot = Instantiate(warningshooter, p1, Quaternion.identity).GetComponent<bossprojshooter>();
                    warningshoot.ydir = 1;
                    warningshoot.xdir = 0;
                    warningshoot.speed = 60;
                    warningshoot.accelrate = 100;
                    summtrackorbfoot = 0.2f;


                }




                if (lastfaceddirx != dirX)
                {
                    transform.Rotate(new Vector3(0, 180, 0));
                }

                lastfaceddirx = dirX;


                return;


            }
            else if (disttoplayer < 50f)
            {

                running = true;
                animval = 3;    

                if (findz.rb.position.x > rb.position.x)
                {
                    dirX = -1;
                }
                else if (findz.rb.position.x < rb.position.x)
                {
                    dirX = 1;

                }

                rb.velocity = new Vector2(40 * dirX, rb.velocity.y);

                if (summtrackorbfoot < 0)
                {
                    Vector2 p1 = new Vector2(rb.position.x, rb.position.y - 20f);
                    bossprojshooter warningshoot = Instantiate(warningshooter, p1, Quaternion.identity).GetComponent<bossprojshooter>();
                    warningshoot.ydir = 1;
                    warningshoot.xdir = 0;
                    warningshoot.speed = 60;
                    warningshoot.accelrate = 100;

                    summtrackorbfoot = 0.2f;


                }
                UpdateAnim();





                if (lastfaceddirx != dirX)
                {
                    transform.Rotate(new Vector3(0, 180, 0));
                }

                lastfaceddirx = dirX;


                return;
            }
            else
            {
                running = false;
                animval = -1;
            }
        }
        else
        {
            running = false;

            animval = -1;
        }







         //if(lastatkended)dirX = (int)((findz.rb.position.x - rb.position.x)/(Mathf.Abs(findz.rb.position.x - rb.position.x)));
        if(dirX == 0)
        {
            dirX = lastfaceddirx;
        }


        if(lastfaceddirx != dirX)
        {
            transform.Rotate(new Vector3(0, 180, 0));
        }

        lastfaceddirx = dirX;

        

        if (lastatkended == false)
        {

            //Debug.Log(val);

            if (val == 1)
            {
                orbThrow();
            }
            if (val == 2)
            {
                fastBall();
            }
            if (val == 3)
            {
                warningrays();
            }
            if (val == 4)
            {
                orbSummon();
            }
            if(val == 5)
            {
                dash();
            }
            if (val == 6)
            {
                trackerBeam();
            }

            if(val == 7)
            {
                HaltSword();
            }



        }
        else
        {


            int mostrecval = val;

            val = UnityEngine.Random.Range(1,8);

            if(mostrecval == val && val == 6)
            {
                val = 1;
            }
            //Debug.Log(findz.rb.position.x - rb.position.x);
            if (disttoplayer < 50f)
            {
                if (UnityEngine.Random.Range(1, 5) == 1)
                {
                    val = 5;
                }
            }
            
            dirX = Math.Sign(findz.rb.position.x - rb.position.x);

            if(dirX == 0)
            {

                Debug.Log("hello?/");
            }
            lastatkended = false;


        }

    }


    void HaltSword()
    {
        if(haltswordtimer < 0)
        {
            lastatkended = true;
            haltswordtimer = 1f;
            hswordcd = 0.1f;
            Debug.Log(hswordcount);
            hswordcount = 0;

        }
        haltswordtimer-=Time.deltaTime;


        hswordcd -= Time.deltaTime;

        if(hswordcd < 0)
        {

            
            Vector2 hswordpos = new Vector2(rb.position.x - 10f*dirX +hswordcount * 3f * dirX*-1, rb.position.y + 28.75f + hswordcount * -1.5f);


            Vector2 hswordpos2 = new Vector2(rb.position.x - 10f*dirX + hswordcount * 3f * dirX*-1, rb.position.y + hswordcount * 1.5f);

            hswordcd = 0.1f;
            hswordcount++;
            haltSwordMover x = Instantiate(haltSword, hswordpos, Quaternion.identity).GetComponent<haltSwordMover>();
            haltSwordMover x1 = Instantiate(haltSword, hswordpos2, Quaternion.identity).GetComponent<haltSwordMover>();

        }





    }
    void orbThrow()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }


        if(orbcd < 0)
        {
            lastatkended = true;

            orbcd = 1f;
            timer = 0.7f;
            orbshootcd = 0.1f;
            orbshootcount = 3;
            extrashotfiredinorb = false;
        }

        if(timer <= 0.65f && extrashotfiredinorb == false)
        {
            Vector2 fastballe = new Vector2(rb.position.x + 10 * dirX, rb.position.y-10f);


            bossprojshooter warningshoot = Instantiate(warningshooter, fastballe, Quaternion.identity).GetComponent<bossprojshooter>();
            warningshoot.timer = 0.75f;
            warningshoot.ydir = 0;
            warningshoot.xdir = dirX;
            warningshoot.speed = 150f;
            warningshoot.accelrate = 300f;

            extrashotfiredinorb = true;
        }

        if(orbshootcd < 0 && orbshootcount >0)
        {

            Vector2 p1 = new Vector2(rb.position.x + 5 * dirX + 10 * (3-orbshootcount) * dirX, rb.position.y - 7.5f * (3-orbshootcount) + 20f);
            bouncyBall z = Instantiate(x, p1, Quaternion.identity).GetComponent<bouncyBall>();
            z.xdir = dirX;
            z.ydir = 120f + 10*orbshootcount + (findz.rb.position.y- rb.position.y )*2f;

            z.speed = 70f + 10 * orbshootcount - 60f + Math.Abs(rb.position.x - findz.rb.position.x) / 8;
           
            orbshootcd = 0.1f;
            orbshootcount--;





        }




        orbshootcd -= Time.deltaTime;
        orbcd -= Time.deltaTime;
    }

    void fastBall()
    {
        
        float posyofthing = 0f;


        Vector2 fastballe;

        if (fastballshootcount % 2 == 0)
        {
            fastballe = new Vector2(rb.position.x + 10 * dirX, rb.position.y);
        }
        else
        {
            fastballe = new Vector2(rb.position.x + 10 * dirX, rb.position.y -10);
        }



        if(fastballshootcd < 0)
        {

            b1Atkscr z = Instantiate(bossproj, fastballe, Quaternion.identity).GetComponent<b1Atkscr>();
            z.xdir = dirX;

            z.speed = 20f;
            z.accelrate = 500f;
            fastballshootcount++;
            fastballshootcd = 0.75f;
        }

        fastballshootcd-= Time.deltaTime;



        if(fastballshootcount > 2)
        {
            fastballshootcount = 0;
            fastballshootcd = 0f;
            lastatkended = true;
        }
    }
    
    void warningrays()
    {
        warningraystimer -= Time.deltaTime;
        if(warningraystimer < 0)
        {
            warningraystimer = 0.05f;
            warningcount--;
            Vector2 p1 = new Vector2(rb.position.x + 10f * dirX + 3 * (30-warningcount+1) * dirX, rb.position.y - 20f);
            bossprojshooter warningshoot = Instantiate(warningshooter, p1, Quaternion.identity).GetComponent<bossprojshooter>();
            warningshoot.ydir = 1;
            warningshoot.xdir = 0;
            warningshoot.speed = 60;
            warningshoot.accelrate = 100;
        }

        if(warningcount <= 0)
        {
            warningcount = 30;
            lastatkended = true;
        }


        
    }

    void orbSummon()
    {
        //Debug.Log("hello john");
        Vector2 p1 = new Vector2(rb.position.x + 5 * dirX, rb.position.y + 5f);

        if (orbsummontimer == 4.5f)
        {
            dangerOrb script = Instantiate(summonorb.gameObject, p1, Quaternion.identity).GetComponent<dangerOrb>();

            script.xleft = -80;
            script.xright = 90;
            script.ybot = 0;
            script.ytop = 80;
            script.projspeed = 75f;
            script.accelrate = 150f;
        }
        orbsummontimer -= Time.deltaTime;
        if(orbsummontimer < 0)
        {
            lastatkended= true;
            orbsummontimer = 4.5f;
        }


    }

    void dash()
    {
        dashtimer -= Time.deltaTime;
        rb.velocity = new Vector2(dashspeed * dirX, rb.velocity.y);
        dashspeed += Time.deltaTime*100;

        if(dashtimer < 0)
        {
            lastatkended = true;
            rb.velocity = new Vector2(0,rb.velocity.y);
            dashtimer = 2f;
            dashspeed = 5f;
        }

    }

    void trackerBeam()
    {
        if(trackerbeamprojnum == 0 && trackerbeamtimer < 0) {

            trackerbeamprojnum = 7;
            trackerbeamtimer = 1.5f;
            lastatkended = true;
            return;
        }

        trackerbeamtimer -= Time.deltaTime;

        trackerbeamcd-=Time.deltaTime;
        
        if(trackerbeamcd < 0 && trackerbeamprojnum > 0)
        {
            trackerbeamprojnum--;
            trackerbeamcd = 0.1f;

            Vector2 pos = new Vector2(rb.position.x + dirX*5, rb.position.y-3f + trackerbeamprojnum * 2); 
            TrackerOrbController z = Instantiate(trackerorb,rb.position,Quaternion.identity).GetComponent<TrackerOrbController>();
            z.speed = 10f + UnityEngine.Random.Range(0, 10) * 10f;
            z.accelrate = 1000f + UnityEngine.Random.Range(-10,10) * 20f;
            Vector2 directionToPlayer = new Vector2((findz.rb.position.x - rb.position.x), (findz.rb.position.y - rb.position.y) + UnityEngine.Random.Range(-5,5) * 30 ).normalized;
            z.initdir = directionToPlayer;
            z.rotateShiftMagnitude = 20f + UnityEngine.Random.Range(2,5) * 10f;

        }
    }


    void UpdateAnim()
    {

        if(val == 4)
        {
            animval = 3;
        }

        if(animval!= 0 && !(animval == 3 || animval == 5 || animval == 6) )
        {
            animval = val;
        }


        if(!running && (animval == 3 || animval ==5 || animval == 6 ))
        {
            animval = 1;
        }

        

        animator.SetInteger("animstate", animval);
    }



    
    private void OnCollisionStay2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.CompareTag("Attack") && damagetimer <= 0)

        {
            
            dirhitfrom = (rb.position.x - findz.rb.position.x)/Math.Abs((rb.position.x - findz.rb.position.x));
            damagetimer = 0.2f;
            
            hp--;
            if(hp <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        */
        
    }
    private Boolean OnRWall()
    {
        if (Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.right, 0.1f, jumpableground))
        {
            return true;
        }
        return false;
    }

    private Boolean OnLWall()
    {
        if (Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.left, 0.1f, jumpableground))
        {
            return true;
        }

        return false;
    }
}

