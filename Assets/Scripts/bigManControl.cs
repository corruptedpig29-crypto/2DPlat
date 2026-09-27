using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class bigManControl : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D rb;
    BoxCollider2D coll;


    PlayerMovement player;

    int animState;

    Animator anim;

    float xvelocity = 0;
    float yvelocity = 0;

    float scale = 5;

    int dirX;
    int dirY;

    [SerializeField] GameObject wall;

    [SerializeField] GameObject lineTracerProj;

    [SerializeField] GameObject warningProj;
    [SerializeField] GameObject bossProjNoWarning;


    Transform trans;
    bool lastatkended = false;
    int atknum = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<PlayerMovement>();
        anim = GetComponent<Animator>();
        trans = GetComponent<Transform>();

        trans .localScale = new Vector3(scale, scale, 1);
    }

    // Update is called once per frame


    int mostrecdirX = 0;
    int mostrecdirY = 0;

    void Update()
    {

        animUpdate();

        mostrecdirX = dirX;
        mostrecdirY = dirY;


        dirX = Math.Sign((int)(player.transform.position.x - transform.position.x));

        dirY = Math.Sign((int)(player.transform.position.y - transform.position.y));




        if (!lastatkended)
        {
            //atknum = 4;
            if (atknum == 0)
            {
                move();
            }

            if (atknum == 1)
            {
                handRaise();
            }

            if (atknum == 2)
            {
                burst();
            }
            if (atknum == 3)
            {
                hammer();
            }
            if(atknum == 4)
            {
                gunTransform();
            }

        }

        if (lastatkended)
        {
            
            rb.velocity = new Vector2(0, 0);
            anim.SetInteger("state", 0);


            int mostRecAtknum = atknum;

            atknum = UnityEngine.Random.Range(1, 5);

            while(atknum == mostRecAtknum)
            {
                atknum = UnityEngine.Random.Range(1, 5);
            }


            if(atknum != 4)
            {
                if (dirX > 0)
                {
                    transform.localScale = new Vector3(scale, scale, 1);
                }
                else if (dirX < 0)
                {
                    transform.localScale = new Vector3(-scale, scale, 1);
                }
            }
            else
            {
                transform.localScale = new Vector3(scale, scale, 1);
            }


            if ((player.transform.position - transform.position).magnitude >= 75f)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    atknum = 0;
                }
                else
                {
                    atknum = 1;
                }
            }

            lastatkended = false;
        }


    }


    void move()

    {

        if (OnCeil())
        {
            if (yvelocity > 0)
            {
                yvelocity = 0f;
            }
        }

        if (OnGround())
        {
            if (yvelocity < 0)
            {
                yvelocity = 0f;
            }
        }


        if (OnRight())
        {
            if (xvelocity > 0)
            {
                xvelocity = 0f;
            }
        }

        if (OnLeft())
        {
            if (xvelocity < 0)
            {
                xvelocity = 0f;
            }
        }

        anim.SetInteger("state", 0);

        if (dirX > 0)
        {
            transform.localScale = new Vector3(scale, scale, 1);
        }
        else if (dirX < 0)
        {
            transform.localScale = new Vector3(-scale, scale, 1);
        }


        if (Mathf.Abs(player.transform.position.x - transform.position.x) < scale * 8f && Mathf.Abs(player.transform.position.y - transform.position.y) < scale * 3f)
        {
            xvelocity = 0f;
            yvelocity = 0f;

            lastatkended = true;
        }
        else
        {
            rb.velocity = new Vector2(xvelocity, yvelocity);
            if(dirX > 0)xvelocity += 50f * Time.deltaTime;
            if(dirY > 0)yvelocity += 50f * Time.deltaTime;
            if(dirX < 0)xvelocity -= 50f * Time.deltaTime;
            if (dirY < 0) yvelocity -= 50f * Time.deltaTime;

        }
    }



    float hraisetimer = 5f;
    float wallSummonCD = 0.6f;
    void handRaise()  
    {
        if (hraisetimer == 5f) anim.SetInteger("state", 1);
        hraisetimer -= Time.deltaTime;

       
        if (wallSummonCD < 0)
        {
            int horizDirOfWall;
            int vertDirOfWall;
            if (UnityEngine.Random.Range(0f, 1f) >= 0.5f)
            {
                vertDirOfWall = 0;
                if (UnityEngine.Random.Range(0f, 1f) >= 0.5f)
                {
                    horizDirOfWall = 1;
                }
                else
                {
                    horizDirOfWall = -1;
                }
                
            }
            else
            {
                horizDirOfWall = 0;
                if (UnityEngine.Random.Range(0f, 1f) >= 0.5f)
                {
                    vertDirOfWall = 1;
                }
                else
                {
                    vertDirOfWall = -1;
                }
            }
            Vector2 pos = new Vector2(0, 0);
            if(horizDirOfWall != 0)
            {
                pos = new Vector2(150f * -horizDirOfWall + player.transform.position.x, UnityEngine.Random.Range(-1, 1) * 25f + player.transform.position.x);
            }

            if (vertDirOfWall != 0)
            {
                pos = new Vector2(UnityEngine.Random.Range(-1, 1) * 25f + player.transform.position.x, 75f * -vertDirOfWall + player.transform.position.y);
            }


            wallSummonCD = 0.6f;

            wallMover wMove = Instantiate(wall, pos, Quaternion.identity).GetComponent<wallMover>() ;

            wMove.horizDirMove = horizDirOfWall;
            wMove.vertDirMove = vertDirOfWall;
            wMove.speed = UnityEngine.Random.Range(1f, 2f) * 100f;

            wMove.widthScale = 1;
            wMove.heightScale = 1;

            if (vertDirOfWall != 0) wMove.widthScale = UnityEngine.Random.Range(0.5f, 0.75f);
            if (horizDirOfWall != 0) wMove.heightScale = UnityEngine.Random.Range(0.5f, 0.75f);

            //Debug.Log(wMove.heightScale);
        }
        wallSummonCD -= Time.deltaTime;

        if (hraisetimer <= 0f)
        {
            hraisetimer = 5f;
            lastatkended = true;
        }
    }

    float bursttimer = 2f;


    List<tracerLineProjMovement> projList = new List<tracerLineProjMovement>();

    float startDegree = 0f;
    void burst()
    {

        if (bursttimer == 2f)
        {
            startDegree = UnityEngine.Random.Range(0f, 360f);

            for (int i = 0; i < 200f; i++)
            {
                float projDegree = UnityEngine.Random.Range(startDegree + 45f, 360f + startDegree);

                GameObject proj = Instantiate(lineTracerProj, new Vector2(rb.position.x, rb.position.y + 5f), Quaternion.identity);
                proj.GetComponent<tracerLineProjMovement>().degreeOfRotation = projDegree;

                

                proj.GetComponent<SpriteRenderer>().enabled = false;
                proj.GetComponent<BoxCollider2D>().enabled = false;
                proj.GetComponent<tracerLineProjMovement>().begin = true;
                proj.GetComponent<tracerLineProjMovement>().waitTimer = 0.4f;
                projList.Add(proj.GetComponent<tracerLineProjMovement>());
            }
            anim.SetInteger("state", 2);
        }

        bursttimer -= Time.deltaTime;

        if(bursttimer < 1.25f)
        {
            for(int i =0; i < projList.Count; i++)
            {
                projList[i].begin = true;
                projList.RemoveAt(i);
                i--;
            }
        }

        if(bursttimer <= 0f)
        {
            
            bursttimer = 2f;
            lastatkended = true;
        }
    }

    float hammertimer = 1.3f;


    float hammerXspeed = -60f;
    float hammerYspeed = 60f;

    float initDirX = 0f;
    bool HammerProjSummoned = false;
    void hammer()
    {
        if (hammertimer == 1.3f)
        {
            initDirX = dirX;
            anim.SetInteger("state", 3);
        }
        hammertimer -= Time.deltaTime;
        
        rb.velocity = new Vector2(hammerXspeed * initDirX, hammerYspeed);

        hammerXspeed += 150f * Time.deltaTime;
        hammerYspeed += -150f * Time.deltaTime;


        if(!HammerProjSummoned)
        {
            HammerProjSummoned = true;

            if (UnityEngine.Random.Range(0f, 1f) >= 0.5f)
            {


                for (int i = 0; i < 50; i++)
                {
                    bossprojshooter bproj = Instantiate(warningProj, new Vector2(player.rb.position.x - 250 + i * 10, player.rb.position.y - 50f), Quaternion.identity).GetComponent<bossprojshooter>();
                    bproj.xdir = 0f;
                    bproj.ydir = 1f;
                    bproj.timer = 1f;
                }
                for (int i = 0; i < 50; i++)
                {
                    bossprojshooter bproj = Instantiate(warningProj, new Vector2(player.rb.position.x + 60f, player.rb.position.y - 250 + i * 10), Quaternion.identity).GetComponent<bossprojshooter>();
                    bproj.xdir = -1f;
                    bproj.ydir = 0f;
                    bproj.timer = 1f;
                }
            }
            else
            {
                for (int i = 0; i < 50; i++)
                {
                    bossprojshooter bproj = Instantiate(warningProj, new Vector2(player.rb.position.x - 250 + i * 10, player.rb.position.y + 50f), Quaternion.identity).GetComponent<bossprojshooter>();
                    bproj.xdir = 0f;
                    bproj.ydir = -1f;
                    bproj.timer = 1f;
                }

                for (int i = 0; i < 50; i++)
                {
                    bossprojshooter bproj = Instantiate(warningProj, new Vector2(player.rb.position.x - 60f, player.rb.position.y - 250 + i * 10), Quaternion.identity).GetComponent<bossprojshooter>();
                    bproj.xdir = 1f;
                    bproj.ydir = 0f;
                    bproj.timer = 1f;
                }
            }


        }
        if (hammertimer <= 0f)
        {
            hammertimer = 1.3f;
            lastatkended = true;
            hammerXspeed = -60f;
            hammerYspeed = 60f;
            HammerProjSummoned = false;
        }
    }

    float transformWaitTimer = 0.7f;

    float gunTransformTimer = 8f;
    float rotSpeed = 80f;

    float gunShootCD = 0.03f;
    float quadraCD = 0.1f;
    bool gunTransformed = false;
    float fourGapCD = 1.5f;
    void gunTransform()
    {
        angle +=  Time.deltaTime;
        if (!gunTransformed)
        {
            anim.SetInteger("state", 4);
            gunTransformed = true;
        }
        gunShootCD-= Time.deltaTime;    

        Vector2 actualDirectionToPlayer = new Vector2((player.rb.position.x - rb.position.x), (player.rb.position.y - rb.position.y)).normalized;

        Vector2 directionToPlayer = new Vector2(-(player.rb.position.y - rb.position.y), (player.rb.position.x - rb.position.x)).normalized;


        rotSpeed += 10f * Time.deltaTime;

        rotSpeed = Mathf.Clamp(rotSpeed, 40f, 360f);    

        gunTransformTimer -= Time.deltaTime;

        transformWaitTimer -= Time.deltaTime;

        if(transformWaitTimer < 0f && gunShootCD < 0f)
        {
            float projDegree = UnityEngine.Random.Range(transform.eulerAngles.z - 5f, transform.eulerAngles.z + 5f);
            Debug.Log("Rotation : " + transform.eulerAngles.z);
            GameObject proj = Instantiate(lineTracerProj, rb.position + Vector2.up*(float)Math.Sin(projDegree * Mathf.Deg2Rad +  Mathf.Deg2Rad * 13f) * 20f + Vector2.right*(float)Math.Cos(projDegree * Mathf.Deg2Rad +  Mathf.Deg2Rad * 13f) * 20f, Quaternion.identity);
            proj.GetComponent<tracerLineProjMovement>().degreeOfRotation = projDegree;



            proj.GetComponent<SpriteRenderer>().enabled = false;
            proj.GetComponent<BoxCollider2D>().enabled = false;
            proj.GetComponent<tracerLineProjMovement>().begin = true;
            proj.GetComponent<tracerLineProjMovement>().waitTimer = 0.01f;
            projList.Add(proj.GetComponent<tracerLineProjMovement>());

            gunShootCD = 0.03f;



        }
        fourGapCD-= Time.deltaTime;
        quadraCD -= Time.deltaTime;
        if(quadraCD < 0f && transformWaitTimer < 0f && fourGapCD > 0)
        {
            summonTheFour();
            quadraCD = 0.1f;
        }

        if(fourGapCD < -1.5f)
        {
            fourGapCD = 1.5f;
        }


        float angleDifference = Vector2.SignedAngle(transform.up, directionToPlayer);

        float rotateShiftMagnitude = rotSpeed;  

        float rotationAmount = Mathf.Clamp(angleDifference, -rotateShiftMagnitude * Time.deltaTime, rotateShiftMagnitude * Time.deltaTime);




        transform.Rotate(0, 0, rotationAmount);

        


        if (gunTransformTimer < 0f)
        {
            transformWaitTimer = 0.7f;
            gunTransformTimer = 8f;
            lastatkended = true;
            rotSpeed = 80f;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            gunTransformed = false;
        }
    }

    float angle = 0f;

    void summonTheFour()
    {

        float projspeed = 75f;
        float accelrate = 100f;
        b1Atkscr p1 = Instantiate(bossProjNoWarning.gameObject, rb.position, Quaternion.identity).GetComponent<b1Atkscr>();
        p1.xdir = Mathf.Sin(angle);
        p1.ydir = Mathf.Cos(angle);
        p1.speed = projspeed;
        p1.accelrate = accelrate;

        b1Atkscr p2 = Instantiate(bossProjNoWarning.gameObject, rb.position, Quaternion.identity).GetComponent<b1Atkscr>();
        p2.xdir = -Mathf.Sin(angle);
        p2.ydir = -Mathf.Cos(angle);
        p2.speed = projspeed;
        p2.accelrate = accelrate;

        
        b1Atkscr p3 = Instantiate(bossProjNoWarning.gameObject, rb.position, Quaternion.identity).GetComponent<b1Atkscr>();
        p3.xdir = Mathf.Sin(angle * 2f + 3.14159265f / 2);
        p3.ydir = Mathf.Cos(angle * 2f + 3.14159265f / 2);
        p3.speed = projspeed;
        p3.accelrate = accelrate;


        /*
        b1Atkscr p4 = Instantiate(bossProjNoWarning.gameObject, rb.position, Quaternion.identity).GetComponent<b1Atkscr>();
        p4.xdir = -Mathf.Sin(angle + 3.14159265f / 2);
        p4.ydir = -Mathf.Cos(angle + 3.14159265f / 2);
        p4.speed = projspeed;
        p4.accelrate = accelrate;
        */
    }
    

    [SerializeField] private LayerMask ground;

    private Boolean OnGround()
    {
        if (Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 3f, ground))
        {
            return true;
        }


        return false;

    }

    private Boolean OnLeft()
    {
        if (Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.left, 3f, ground))
        {
            return true;
        }


        return false;

    }

    private Boolean OnRight()
    {
        if (Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.right, 3f, ground))
        {
            return true;
        }


        return false;

    }

    private Boolean OnCeil()
    {
        if (Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.up, 5f, ground) )
        {
            return true;
        }


        return false;

    }
    void animUpdate()
    {
        //anim.SetInteger("state", animState);
    }
}
