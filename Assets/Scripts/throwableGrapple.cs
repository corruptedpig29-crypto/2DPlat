using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwableGrapple : MonoBehaviour
{
    // Start is called before the first frame update

    int bouncecount = 0;
    [SerializeField] private LayerMask jumpableground;


    float flipcd = 0.1f;
    public float speed;
    public float xdir;
    public float ydir;
    Rigidbody2D rb;
    BoxCollider2D coll;
    Transform trans;

    public bool beingGrappledTo = false;


    PlayerMovement player;

    float bouncecd = 0.1f;

    bool dying = false;

    float timer = 0f;
    Boolean firstframe = true;
    void Start()
    {
        timer = 0f;
        player = FindObjectOfType<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        trans = GetComponent<Transform>();

        beingGrappledTo = false;
        xdir = 1;


        dying = false;
        ydir = 0;

        speed = 275f;
    }

    float rateOfDecrAdd = 1f;

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;
        rateOfDecrAdd += Time.deltaTime * 2f;

        speed = Mathf.Clamp(speed, 0, 225f);

        speed-= 100f * Time.deltaTime * rateOfDecrAdd;
        //ydir -= 100f * Time.deltaTime * rateOfDecrAdd;
        /*
        if (ydir <= 0)
        {
            ydir = 0f;
        }
        */

        if(timer > 10f)
        {
            Destroy(this.gameObject);
        }


        speed -= 100f * Time.deltaTime;

        if (speed <= 0)
        {
            speed = 0;
        }
        if (firstframe == true)
        {
            xdir = player.mostrecdirX;

            speed += Math.Abs(player.rb.velocity.x);
            //ydir = player.rb.velocity.y;
            //ydir += (player.rb.velocity.y);

            //rb.velocity = new Vector2(xdir * speed, ydir);
            firstframe = false;
        }

        rb.velocity = xdir * speed * Vector2.right +  speed / 10f * Vector2.up ;

        /*
        rb.velocity = new Vector2(xdir * speed, rb.velocity.y);
        bouncecd -= Time.deltaTime;
        flipcd -= Time.deltaTime;
        


        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, ydir);
        }
        if (OnRWall())
        {
            if (flipcd < 0)
            {
                xdir = -1;
            }
        }



        if (OnLWall())
        {
            if (flipcd < 0)
            {
                xdir = 1;
            }
        }

        if ((IsGrounded() || OnLWall() || OnRWall()) && bouncecd < 0)
        {
            bouncecd = 0.1f;


            bouncecount++;
        }

        if (bouncecount > 5f)
        {
            Destroy(this.gameObject);
        }
        */

        if (beingGrappledTo == true)
        {
            if (!dying) Destroy(this.gameObject, 0.3f);

            rb.velocity = new Vector2(0, 0);
        }

    }

    private Boolean IsGrounded()
    {
        if (Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableground))
        {
            return true;
        }

        return false;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Attack"))
        {
            //xdir = findz.mostrecdirX;
        }
    }
}
