using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class monkeyman : MonoBehaviour
{

    private Rigidbody2D rb;
    Rigidbody2D Findz;
    private float movespeed = 20;
    private int dir = 1;
    private int dir2 = 1;
    private SpriteRenderer sprite;
    [SerializeField]Boolean beingshot = false;
    int c1 = 0;
    int hp = 100;
    bool dashing = false;
    float dashdur = 0;
    float dcd = 0.2f;
    

    float waitcd = 0;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();    
        rb = GetComponent<Rigidbody2D>();
        Findz = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()


    {

        
        if (c1 >= 2) {
            beingshot = false;
              c1 = 0;
        }




        if (Mathf.Abs(Findz.position.x - rb.position.x) < 50 && Mathf.Abs(Findz.position.y - rb.position.y) < 50 && dashing == false && waitcd <=0)
        {
            dashdur = 2;
            if (Findz.position.x < rb.position.x)
            {
                dir = -1;
                sprite.flipX = true;

            }
            else
            {
                dir = 1;
                sprite.flipX = false;
            }
            dashing = true;

        }

        if (dashing)
        {

            rb.velocity = new Vector2(movespeed * dir, rb.velocity.y);
            movespeed += Time.deltaTime * 20;
            dashdur-=Time.deltaTime;
        }

        

        if(dashdur < 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            movespeed = 0;
            dashing = false;
        }

        dcd -= Time.deltaTime;
        
        if (beingshot)
        {
            rb.velocity = new Vector2(30 * dir2, 0);
            c1++;
            movespeed = 20;
            return;
        }

        if(GetComponent<UniHpManager>().tookdamage == true){
            GetComponent<UniHpManager>().tookdamage = false;
            if (Findz.position.x < rb.position.x)
            {
                dir2 = 1;
            }
            else
            {
                dir2 = -1;
            }

            rb.velocity = new Vector2(10 * dir2, 0);
            beingshot = true;


            dcd = 0.2f;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.CompareTag("Attack") && dcd < 0)
        {
            
            hp -= 34;
            if (Findz.position.x < rb.position.x)
            {
                dir2 = 1;
            }
            else
            {
                dir2 = -1;
            }
            
            rb.velocity = new Vector2(10 * dir2, 0);
            beingshot = true;
            if (hp <= 0)
            {
                Destroy(gameObject);
            }

            dcd = 0.2f;
        }
        */
    }
}
