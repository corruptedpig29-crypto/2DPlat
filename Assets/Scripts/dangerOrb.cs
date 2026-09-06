using System;
using UnityEngine;
public class dangerOrb : MonoBehaviour
{

    [SerializeField] GameObject warningproj;

    [SerializeField] GameObject hsword;

    public float xleft, xright, ybot, ytop;

    Rigidbody2D rb;
    BoxCollider2D coll;
    SpriteRenderer sprite;
    float xpos, ypos;
    float speed = 2f;

    float timer = 3f;
    float degrees = 0;

    bool shooting = false;

    float cd = 0f;
    public float projspeed;
    public float accelrate;
    Boolean start = false;

    float bigcd = 0.5f;

    float trackd = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        xpos = UnityEngine.Random.Range(xleft, xright);
        ypos = UnityEngine.Random.Range(ybot, ytop);
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        xleft = 0f;
        xright = 0f;
        ytop = 0f;
        ybot = 0f;



    }

    // Update is called once per frame
    void Update()
    {


        degrees %= 360;

        degrees += Time.deltaTime;

         
        if (!start)
        {
            if (Mathf.Abs(xpos - rb.position.x) < 10 && (Mathf.Abs(ypos - rb.position.y) < 10))
            {
                rb.velocity = new Vector2(0, 0);
                start = true;
            }
            else
            {
                rb.velocity = new Vector2((xpos - rb.position.x) * speed, (ypos - rb.position.y) * speed);
            }
            speed += Time.deltaTime;
        }

        if (start)
        {
            
            timer-= Time.deltaTime;
            cd -= Time.deltaTime;


            if (bigcd > 0.15f)
            {


                if (cd < 0)
                {
                    cd = 0.05f;

                    b1Atkscr p1 = Instantiate(warningproj.gameObject, rb.position, Quaternion.identity).GetComponent<b1Atkscr>();
                    p1.xdir = Mathf.Sin(degrees);
                    p1.ydir = Mathf.Cos(degrees);
                    p1.speed = projspeed;
                    p1.accelrate = accelrate;

                    b1Atkscr p2 = Instantiate(warningproj.gameObject, rb.position, Quaternion.identity).GetComponent<b1Atkscr>();
                    p2.xdir = -Mathf.Sin(degrees);
                    p2.ydir = -Mathf.Cos(degrees);
                    p2.speed = projspeed;
                    p2.accelrate = accelrate;

                    b1Atkscr p3 = Instantiate(warningproj.gameObject, rb.position, Quaternion.identity).GetComponent<b1Atkscr>();
                    p3.xdir = Mathf.Sin(degrees + 3.14159265f / 2);
                    p3.ydir = Mathf.Cos(degrees + 3.14159265f / 2);
                    p3.speed = projspeed;
                    p3.accelrate = accelrate;

                    b1Atkscr p4 = Instantiate(warningproj.gameObject, rb.position, Quaternion.identity).GetComponent<b1Atkscr>();
                    p4.xdir = -Mathf.Sin(degrees + 3.14159265f / 2);
                    p4.ydir = -Mathf.Cos(degrees + 3.14159265f / 2);
                    p4.speed = projspeed;
                    p4.accelrate = accelrate;


                }
            }



            if (bigcd > 0f)
            {
                bigcd -= Time.deltaTime;
            }
            else
            {
                bigcd = 0.5f;
            }

                trackd -= Time.deltaTime;

            //Debug.Log("Trackd : " + trackd);
            if (trackd < 0)
            {

                Instantiate(hsword.gameObject, rb.position, Quaternion.identity);
                trackd = 0.3f;
            }


        }

        if(timer < 0)
        {
            Destroy(this.gameObject);
        }


    }
}
