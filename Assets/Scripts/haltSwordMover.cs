using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Rendering;

public class haltSwordMover : MonoBehaviour
{
    // Start is called before the first frame update

    PlayerMovement player;
    Rigidbody2D rb;
    BoxCollider2D coll;

    [SerializeField] LayerMask ground;
    public float rotatetime;

    bool initset = false;
    public float movespeed;

    float amtdecr;
   
    void Start()
    {
        amtdecr = 1f;


        movespeed = -80f;
        player = FindObjectOfType<PlayerMovement>();
        rotatetime = 0.75f;
        rb = GetComponent<Rigidbody2D>();   
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!initset)
        {
            float px = player.rb.position.x;



            float py = player.rb.position.y;
            Vector2 directionToPlayer = new Vector2((px - rb.position.x), (py - rb.position.y)).normalized;
            

            transform.up = directionToPlayer;
            initset = true;
        }
        amtdecr += Time.deltaTime;
        rotatetime -= Time.deltaTime;

        if(rotatetime >= 0)
        {

            
            rb.freezeRotation = false;

            float px = player.rb.position.x;



            float py = player.rb.position.y;





            Vector2 directionToPlayer = new Vector2((px - rb.position.x), (py - rb.position.y)).normalized;



            float angleDifference = Vector2.SignedAngle(transform.up, directionToPlayer);



            //float rotationAmount = Mathf.Clamp(angleDifference, -300f * Time.deltaTime, 300f * Time.deltaTime)/amtdecr;

            float rotationAmount = angleDifference;



            transform.Rotate(0, 0, rotationAmount);

            
        }

        if (rotatetime < 0)
        {

            rb.freezeRotation = true;

            if(TouchingWall() && movespeed <= 0)
            {
                rb.velocity = new Vector2(0, 0);
            }
            else
            {
                rb.velocity = new Vector2(transform.up.x * movespeed, transform.up.y * movespeed);
            }


            movespeed += 175f * Time.deltaTime;

        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(movespeed > 10f &&( collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player")))
        {
            Destroy(this.gameObject);
        }


        if(movespeed < 0f && collision.gameObject.CompareTag("Wall"))
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
    private Boolean TouchingWall()
    {
        if (Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down,0.1f, ground) || Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.right, 0.1f, ground) || Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.left, 0.1f, ground) ||Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.up, 0.1f, ground))
        {
            return true;
        }


        return false;

    }

}
