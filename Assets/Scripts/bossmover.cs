using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;


public class bossmover : MonoBehaviour
{
    [SerializeField] private LayerMask jumpableground;

    // Start is called before the first frame update
    Boolean slidestarted = false;

    float zambonispeed = 40f;

    int resetnum = 0;

    [SerializeField]GameObject x;


    int zdirtraveling = 0;
    float zwaittimer = 1f;
    float groundaccel = 1f;
    public Rigidbody2D rb;
    private PlayerMovement Findz;
    private Boolean falling = false;
    private BoxCollider2D coll;
    private float timer = 0;
    private int hp = 7;
    public Boolean invert = true;
    Boolean start = false;
    private float cd = 0;
    bool fallen = false;
    float val = 0;
    float waitstar = 0;
    Boolean shift = false;
    float initpongtimer = 7;
    float pongtimer = 7;
    float xbdir = 20f;
    float ybdir = -20f;

    float firecd = 1.2f;
    float dirchangecd = 0.1f;
    bool lastatkended = true;

    float waitcd = 0;
    //b1Atkscr x;
    [SerializeField] Boolean isGrounded;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Findz = FindObjectOfType<PlayerMovement>();
        coll = GetComponent<BoxCollider2D>();
        //x = FindObjectOfType<b1Atkscr>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(val);

       // Debug.Log(rb.velocity.x + ", " + rb.velocity.y);

        cd -= Time.deltaTime;
        dirchangecd-=Time.deltaTime;

        if (isGrounded)
        {
            groundaccel = 0;
        }

        firecd-=Time.deltaTime;
        if (!start)
        {
            if (Math.Abs(Findz.rb.position.x - rb.position.x) < 90)
            {
                start = true;
            }
            else
            {
                return;

            }
        }

        cd++;
        isGrounded = IsGrounded();


        if (lastatkended)
        {
            Boolean justponged = false;
            Boolean justzambonied = false;


            if (val != 2)
            {
               waitstar = 0;
            }

            if (val == 3)
            {
                justponged = true;
            }



            if(val == 4)
            {

                justzambonied = true;
            }


            val = UnityEngine.Random.Range(1, 5);


            while (justponged && val == 3) val = 4;


            if(val == 4)
            {
                if(UnityEngine.Random.Range(1,3) == 1)
                {
                    zdirtraveling = -1;
                }
                else
                {
                    zdirtraveling=1;
                }
            }



            if (justzambonied) val = 1;

            lastatkended = false;
        }


        if(lastatkended == false)
        {
            //Debug.Log(val);


            if (val == 1) {
                if (OnCeiling())
                {
                    lastatkended = true;
                    return;
                }
                slam();
            }
            if(val == 2)
            {
                stars();
            }
            if (val == 3)
            {
                Pong();
            }
            if (val == 4)
            {
                Zamboni();
            }
        }





        if (IsGrounded() && firecd < 0)
        {
            Vector2 p1 = new Vector2(rb.position.x + 20, rb.position.y - 10);
            Vector2 p2 = new Vector2(rb.position.x - 20, rb.position.y - 10);
            b1Atkscr bull1 = Instantiate(x.gameObject, p1, Quaternion.identity).GetComponent<b1Atkscr>();
            b1Atkscr bull2 = Instantiate(x.gameObject, p2, Quaternion.identity).GetComponent<b1Atkscr>();
            bull1.xdir = 1;
            bull2.xdir = -1;

            bull1.GetComponent<BoxCollider2D>().enabled = true;
            bull2.GetComponent<BoxCollider2D>().enabled = true;

            bull1.speed = 50; bull2.speed = 50;
            bull2.accelrate = 200;
            bull1.accelrate = 200;


            firecd = 0.5f;
        }


        if (OnCeiling() && firecd < 0)
        {
            Vector2 p1 = new Vector2(rb.position.x + 20, rb.position.y + 10);
            Vector2 p2 = new Vector2(rb.position.x - 20, rb.position.y + 10);
            b1Atkscr bull1 = Instantiate(x.gameObject, p1, Quaternion.identity).GetComponent<b1Atkscr>();
            b1Atkscr bull2 = Instantiate(x.gameObject, p2, Quaternion.identity).GetComponent<b1Atkscr>();
            bull1.xdir = 1;
            bull2.xdir = -1;

            bull1.GetComponent<BoxCollider2D>().enabled = true;
            bull2.GetComponent<BoxCollider2D>().enabled = true;

            bull1.speed = 50; 
            bull2.speed = 50;
            bull2.accelrate = 200;
            bull1.accelrate = 200;


            firecd = 0.5f;
        }
    }


    void stars()
    {
        
        rb.velocity = new Vector2(0, 0);
        

        waitstar-=Time.deltaTime;

        if (shift)
        {

            if (waitstar <= 0)
            {
                makenewbull(rb.position.x - 10, rb.position.y, -1, 0, 20, 180);
                makenewbull(rb.position.x + 10, rb.position.y, 1, 0, 20, 180);
                for (float i = 4; i <= 7; i+=2)
                {


                    Vector2 p1 = new Vector2(rb.position.x - 50 + 10 * i, rb.position.y - 20);
                    b1Atkscr bull1 = Instantiate(x.gameObject, p1, Quaternion.identity).GetComponent<b1Atkscr>();
                    bull1.ydir = p1.y - rb.position.y;
                    bull1.xdir = (p1.x - rb.position.x) * 3;

                    bull1.GetComponent<BoxCollider2D>().enabled = true;
                    bull1.speed = 20;
                    bull1.accelrate = 180;

                }
                for (float i = 4; i <= 7; i +=2)
                {


                    Vector2 p1 = new Vector2(rb.position.x - 50 + 10 * i, rb.position.y + 20);
                    b1Atkscr bull1 = Instantiate(x.gameObject, p1, Quaternion.identity).GetComponent<b1Atkscr>();
                    bull1.ydir = p1.y - rb.position.y;
                    bull1.xdir = (p1.x - rb.position.x) * 3;

                    bull1.GetComponent<BoxCollider2D>().enabled = true;
                    bull1.speed = 20;
                    bull1.accelrate = 180;

                }

                lastatkended = true;
                waitstar = 0.6f;
                shift = !shift;

            }



        }


        if (!shift)
        {
            if (waitstar <= 0)
            {

                for (float i = 0; i <= 4; i+=2)
                {
                    

                    Vector2 p1 = new Vector2(rb.position.x - 20 + 10 * i, rb.position.y - 20);
                    b1Atkscr bull1 = Instantiate(x.gameObject, p1, Quaternion.identity).GetComponent<b1Atkscr>();
                    bull1.ydir = p1.y - rb.position.y;
                    bull1.xdir = (p1.x - rb.position.x) * 3;

                    bull1.GetComponent<BoxCollider2D>().enabled = true;

                    bull1.speed = 20;
                    bull1.accelrate = 180;
                }

                for (float i = 0; i <= 4; i+=2)
                {


                    Vector2 p1 = new Vector2(rb.position.x - 20 + 10 * i, rb.position.y + 20);
                    b1Atkscr bull1 = Instantiate(x.gameObject, p1, Quaternion.identity).GetComponent<b1Atkscr>();


                    bull1.ydir = p1.y - rb.position.y;
                    bull1.xdir = (p1.x - rb.position.x) * 3;

                    bull1.GetComponent<BoxCollider2D>().enabled = true;

                    bull1.speed = 20;
                    bull1.accelrate = 180;
                }


                lastatkended = true;
                waitstar = 0.6f;
                shift = !shift;

            }
        }


    }

    void makenewbull(float xpspawn, float ypspawn, float dirX, float dirY, float speed, float accelrate)
    {
        Vector2 p1 = new Vector2(xpspawn, ypspawn);
        b1Atkscr bull1 = Instantiate(x.gameObject, p1, Quaternion.identity).GetComponent<b1Atkscr>();
        bull1.ydir = dirY;
        bull1.xdir = dirX;

        bull1.GetComponent<BoxCollider2D>().enabled = true;
        bull1.speed = speed;
        bull1.accelrate = accelrate;
    }

    void slam()
    {
        if (rb.position.y < Findz.rb.position.y + 50 && !falling)
        {
            if (OnCeiling())
            {
                fallen = false;
                lastatkended = true;

            }

            rb.velocity = new Vector2(0, 20f);

        }else if (fallen)
        {
            fallen = false;
            lastatkended = true;
        }





        //These lines make the thwomp position itself over the player
        if (!fallen)
        {
            if (falling == false)
            {
                if (Math.Abs(rb.position.x - Findz.rb.position.x) <= 5)
                {
                    falling = true;
                }


                else if (rb.position.x > Findz.rb.position.x)
                {
                    rb.velocity = new Vector2(-20, 0);

                }
                else if (rb.position.x < Findz.rb.position.x)
                {
                    rb.velocity = new Vector2(20, 0);

                }

            }
        }


        //These lines make the thwomp fall downwards
        if (falling)
        {
            rb.velocity = new Vector2(0, -groundaccel);
            groundaccel += 175f*Time.deltaTime;
            if (IsGrounded() && waitcd <= 0)
            {
                waitcd = 0.66f;
            }
        }


        //these lines make the thwomp wait before going back up

        waitcd-=Time.deltaTime;
        if (waitcd <= 0 && isGrounded == true)
        {

            fallen = true;
            falling = false;
        }

    }


    void Pong()
    {


        if(pongtimer <= 0)
        {
            lastatkended = true;
            xbdir = 20f;
            ybdir = 20f;
            pongtimer = initpongtimer;
            return;
        }

        pongtimer -= Time.deltaTime;
        rb.velocity = new Vector2(xbdir, ybdir);




        if(pongtimer > initpongtimer/2 && Math.Abs(xbdir) < 50)
        {
            xbdir += (0.75f * xbdir + 5f) * Time.deltaTime;
            ybdir += (0.75f * ybdir + 5f) * Time.deltaTime;
        }

        if (pongtimer < initpongtimer / 2 && Math.Abs(xbdir) > 20)
        {

            xbdir -= (0.75f * xbdir + 5f) * Time.deltaTime;
            ybdir -= (0.75f * ybdir + 5f) * Time.deltaTime;
        }

        //FIX THIS RAPID SHIFTING

        

        if (OnRWall())
        {
            if (dirchangecd < 0)
            {
                xbdir *= -1;
                dirchangecd = 0.1f;

            }
        }

        if (OnLWall())
        {
            if (dirchangecd < 0)
            {
                xbdir *= -1;
                dirchangecd = 0.1f;
            }
        }


        if (IsGrounded())
        {
            if (dirchangecd < 0)
            {
                ybdir *= -1;
                dirchangecd = 0.1f;
            }
        }
        
        if (OnCeiling())
        {
            if (dirchangecd < 0)
            {
                ybdir *= -1;
                dirchangecd = 0.1f;

            }
        }
    }

    void Zamboni()
    {
        //Debug.Log(zambonispeed);


        zambonispeed += (100f) * Time.deltaTime;



        if (OnRWall() || OnLWall())
        {
            slidestarted = true;
            if (resetnum == 0)
            {
                zambonispeed = 60f;
                resetnum = 1;
            }
        }

        if (!slidestarted)
        {
            rb.velocity = new Vector2(zdirtraveling * zambonispeed, 0f);
            
        }


        else
        {
            if (!isGrounded)
            {
                
                rb.velocity = new Vector2(0f, -zambonispeed);
            }
            else
            {
                if(resetnum == 1)
                {
                    zambonispeed = 40f;
                    resetnum++;
                }


                rb.velocity = new Vector2(zdirtraveling * zambonispeed * -1, 0f);
                zwaittimer -= Time.deltaTime;

                if(zwaittimer < 0 && (OnLWall() || OnRWall()))
                {
                    lastatkended = true;
                    slidestarted=false;
                    zwaittimer = 1f;
                    zambonispeed = 40f;
                }
            }
        }



    }


    private Boolean IsGrounded()
    {
        Vector2 newvec = new Vector2(coll.bounds.center.x, coll.bounds.center.y -0.5f);
        Vector2 sizevec = new Vector2(coll.bounds.size.x-5f, coll.bounds.size.y - 0.5f);
        if (Physics2D.BoxCast(newvec,sizevec, 0f, Vector2.down, 0.1f, jumpableground))
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
        Vector2 newvec = new Vector2(coll.bounds.center.x-0.5f, coll.bounds.center.y);
        Vector2 sizevec = new Vector2(coll.bounds.size.x-0.5f, coll.bounds.size.y - 5f);
        if (Physics2D.BoxCast(newvec, sizevec, 0f, Vector2.left, 0.1f, jumpableground))
        {
            return true;
        }

        return false;
    }


    private Boolean OnCeiling()
    {
        Vector2 newvec = new Vector2(coll.bounds.center.x, coll.bounds.center.y+0.5f);
        Vector2 sizevec = new Vector2(coll.bounds.size.x - 5f, coll.bounds.size.y-0.5f);

        if (Physics2D.BoxCast(newvec,sizevec, 0f, Vector2.up, 0.1f, jumpableground))
        {
            return true;
        }

        return false;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.CompareTag("Attack"))
        {
            if (cd < 0)
            {
                cd = 0.1f;
                hp--;
            }
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
        */
        
    }

}
