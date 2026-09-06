using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncyBall : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]GameObject bossproj;
    int bouncecount = 0;
    [SerializeField] private LayerMask jumpableground;
    public PlayerMovement findz;
    float flipcd = 0.1f;
    public float speed;
    public float xdir;
    public float ydir;
    Rigidbody2D rb;
    BoxCollider2D coll;
    Transform trans;

    float bouncecd = 0.1f;

    Boolean firstframe = true;

    float addforcetimer = 0.01f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();   
        trans = GetComponent<Transform>();
        findz = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.gravityScale = ydir/5;
        rb.velocity =  new Vector2(xdir * speed, rb.velocity.y) ;
        bouncecd -= Time.deltaTime;
        flipcd -= Time.deltaTime;


        if(firstframe == true)
        {
            rb.velocity = new Vector2(xdir * speed, ydir);
            firstframe = false;
        }

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

        if((IsGrounded() || OnLWall() || OnRWall() ) && bouncecd < 0) {
            bouncecd = 0.1f;


            bouncecount++;

            Vector2 pos = new Vector2(rb.position.x,rb.position.y-5f);

            if (IsGrounded() && !(OnLWall() || OnRWall()))
            {
                bossprojshooter p1 = Instantiate(bossproj.gameObject, pos, Quaternion.identity).GetComponent<bossprojshooter>();

                p1.speed = 50f;
                p1.ydir = 1f;
                p1.xdir = 0f;
                p1.accelrate = 100f;
            }
        }

        if(bouncecount > 2)
        {
            Destroy(this.gameObject);
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
