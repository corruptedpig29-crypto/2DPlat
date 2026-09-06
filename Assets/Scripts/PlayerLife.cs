using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;


/*
public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    public float hopping = 0 / 80;
    private BoxCollider2D coll;
    PlayerMovement findz;
    BoxCollider2D Collider;

    [SerializeField]public int hp = 5;
    public float cd = 0 / 80;
    float cdtimer = 1000 / 80;




    public AudioSource source;
    public AudioClip clip;


    private void Start()
    {
        source = GetComponent<AudioSource>();

        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Collider = GetComponent<BoxCollider2D>();

        findz = FindObjectOfType<PlayerMovement>();
    }
    private void Update()
    {

        cd-=Time.deltaTime;
        if (hp == 0)
        {
            hp--;
            Die();
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        Rigidbody2D temp = collision.rigidbody;

        if (collision.gameObject.CompareTag("Monster"))
        {

            if (Input.GetKey(KeyCode.S) && temp.position.y <= rb.position.y)
            {
                hopping = 100 / 80;
            }
            else if (cd <= 0)
            {
                source.PlayOneShot(clip);


                if (hp == 1)
                {
                    Die();
                }else
                {
                    cd = cdtimer;
                    hp--;

                    if (collision.gameObject.GetComponent<Rigidbody2D>().position.x > rb.position.x)
                    {
                        findz.dirX = 1;
                        
                        findz.beinghit = true;
                        findz.dirhitfrom = -1;
                        findz.hittimer = 300 / 80;

                    }
                    else
                    {
                        findz.dirX = -1;
                        findz.beinghit = true;
                        findz.dirhitfrom = 1;
                        findz.hittimer = 300 / 80;
                    }
                }

            }




        }
        if (collision.gameObject.CompareTag("Trap"))
        {
            source.PlayOneShot(clip);

            hp--;
            rb.position = new Vector2(findz.lastgrndlocx, findz.lastgrndlocy);


            

            

        }


        if (collision.gameObject.CompareTag("Projectile"))
        {


            if(cd <= 0)
            {
                source.PlayOneShot(clip);

                cd = cdtimer;
                hp--;
                if (collision.gameObject.GetComponent<Rigidbody2D>().position.x > rb.position.x)
                {
                    findz.beinghit = true;
                    findz.dirhitfrom = -1;
                    findz.hittimer = 300 / 80;

                }
                else
                {
                    findz.beinghit = true;
                    findz.dirhitfrom = 1;
                    findz.hittimer = 300 / 80;
                }
            }



        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            if (cd <= 0)
            {
                cd = cdtimer;
                hp--;
            }
        }
    }


    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("Death");
    }

   private void RestartLevel()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame

}
*/