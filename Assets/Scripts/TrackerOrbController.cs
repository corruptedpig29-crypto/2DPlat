using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class TrackerOrbController : MonoBehaviour
{
    // Start is called before the first frame update
    BoxCollider2D coll;
    Rigidbody2D rb;
    PlayerMovement player;
    Transform trans;
    SpriteRenderer spriteRenderer;
/*
    public float xmove;
    public float ymove;
*/
    public float speed = 5;
    public float accelrate = 10;
    private bool setinitdir = false;

    public float rotateShiftMagnitude = 10f;


    float timealive = 0;
    public Vector2 initdir;   
    void Start()
    {
        
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        trans = GetComponent<Transform>();
        speed = 20;
        accelrate = 50;
        spriteRenderer = GetComponent<SpriteRenderer>();
        timealive = 0;

    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.flipY = true;


        float px = player.rb.position.x;



        float py = player.rb.position.y;





        Vector2 directionToPlayer = new Vector2((px - rb.position.x), (py - rb.position.y)).normalized;

        if (!setinitdir)

        {

            if(initdir == null)
            {
                initdir = directionToPlayer;
            }
            transform.up = initdir;
            setinitdir = true;
        }



        float angleDifference = Vector2.SignedAngle(transform.up, directionToPlayer);



        float rotationAmount = Mathf.Clamp(angleDifference, -rotateShiftMagnitude * Time.deltaTime, rotateShiftMagnitude * Time.deltaTime);




        transform.Rotate(0, 0, rotationAmount);







        speed += accelrate*Time.deltaTime;




        Vector2 waytomove = new Vector2(transform.up.x * speed, transform.up.y * speed);
        rb.velocity = waytomove;
        timealive += Time.deltaTime;
        if(timealive >= 3f)
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
    }
}
