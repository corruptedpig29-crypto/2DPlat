using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;
using System;
public class b1Atkscr : MonoBehaviour
{
    [SerializeField] private LayerMask jumpableground;
    private bossmover bossmover;
    private Rigidbody2D rb;
    Boolean changeddifacing = false;
    Transform transforme;
    private BoxCollider2D coll;
    // Start is called before the first frame update

    float deletetimer = 8f;
    public float accelrate = 0f;
    float radtodeg = 360 / (2 * (float)PI);

    [SerializeField] public float xdir;
    [SerializeField] public float ydir;


    float timerSinceInitialization = 0f;
    public float speed;
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        bossmover = FindObjectOfType<bossmover>();
        transforme = GetComponent<Transform>();
        timerSinceInitialization = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timerSinceInitialization += Time.deltaTime;
        if (deletetimer < 0)
        {
            Destroy(this.gameObject);
        }
        deletetimer -= Time.deltaTime;


        float initxdir = xdir;
        float initydir = ydir;


        if(xdir!=0 && ydir != 0)
        {
            xdir = xdir / (Mathf.Abs(initxdir) + Mathf.Abs(initydir));
            ydir = ydir / (Mathf.Abs(initxdir) + Mathf.Abs(initydir));
        }
        else
        {
            if(xdir == 0 && ydir!=0)
            {
                ydir/=Mathf.Abs(ydir);
            }
            if(ydir == 0 && xdir!=0)
            {
                xdir/=Mathf.Abs(xdir);
            }
        }

        if (!changeddifacing && (xdir !=0 && ydir != 0 ))   
        {
            transforme.Rotate(new Vector3(0, 0, (float)System.Math.Atan(ydir / xdir) * radtodeg));

            changeddifacing = true;
        }
        if(!changeddifacing && (xdir == 0) && (ydir != 0))
        {
            transforme.Rotate(new Vector3(0, 0, 90));

            changeddifacing = true;
        }
        

        rb.velocity = new Vector2(speed * xdir, speed * ydir);
        speed += accelrate * Time.deltaTime;
        if(timerSinceInitialization > 5f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);

        }
        /*
        if (collision.gameObject.CompareTag("Attack"))
        {
            xdir = FindZ.mostrecdirX;
            ydir*= -1;
        }
        */
    }
}
