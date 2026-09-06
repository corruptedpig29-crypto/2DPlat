using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dcoll : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    public Rigidbody2D rb;
    public float hopping = 0 / 80;
    private BoxCollider2D coll;
    PlayerMovement findz;

    [SerializeField] public int hp = 1;
    public float cd = 0 / 80;
    float cdtimer =  1.5f;


    public bool parrying = false;

    float parrytimer = 0.25f;

   

    public bool parryFlourishing = false;

    public float parryFlourishTimer = 0.35f;



    public float initparryflourishtimer;

    public int numTimesParriedInLast = 0;
    public AudioSource source;
    public AudioClip clip;

    public AudioClip parrySound;


    private void Start()
    {

        coll = GetComponent<BoxCollider2D>();

        findz = FindObjectOfType<PlayerMovement>();

        coll.isTrigger = true;
        initparryflourishtimer = 0.35f;
    }
    private void Update()
    {
        parryFlourishCD -= Time.deltaTime;

        /*
        Debug.Log("Parry Timer : "  + parrytimer);
        Debug.Log("Flourish Timer : " + parryFlourishTimer);
        Debug.Log("Parrying : " + parrying);
        Debug.Log("Flourishing : " + parryFlourishing);

        */
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            parrying = true;
            parrytimer = 0.25f;

            for (int i = 0; i < numTimesParriedInLast; i++) {
                parrytimer *= 1f/3f;
            }

            numTimesParriedInLast++;

        }


        if(parrytimer < 0)
        {
            parrying = false;
            numTimesParriedInLast = 0;
        }

        if (hopping > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 60f);
            hopping -= 1 * Time.deltaTime;
        }


        if (findz.beinghit)
        {
            numTimesParriedInLast = 0;
        }

        cd -= Time.deltaTime;

        if (hp == 0)
        {
            hp--;
        }

        if (parryFlourishing)
        {
            numTimesParriedInLast = 0;
            //coll.isTrigger = true;
        }

        /*
        Debug.Log(parrytimer);
        Debug.Log("NUM TIMES :" + numTimesParriedInLast);
        */

        //Debug.Log(parryFlourishTimer);



        if (parryFlourishTimer < 0 && parryFlourishing)
        {
            parryFlourishing = false;
            //coll.isTrigger = false;
        }


        //Debug.Log(coll.isTrigger);


        parrytimer -= Time.deltaTime;
        parryFlourishTimer -= Time.deltaTime;

    }

    public void manualTakeDamage(float xposDamage, int dmgNum)
    {
        if (parrying)
        {
            parrying = false;
            parryFlourishing = true;
            parryFlourishTimer = initparryflourishtimer;
            
            source.PlayOneShot(parrySound);

            findz.parrybdur = initparryflourishtimer / 2;
            findz.dirParriedFrom = -1 * (int)Mathf.Sign(xposDamage - rb.position.x);
            if (findz.isGrounded) findz.dirForParryKbVert = 0;
            if (!findz.isGrounded) findz.dirForParryKbVert = 1;

            if (findz.dirParriedFrom == 0)
            {
                findz.dirParriedFrom = UnityEngine.Random.Range(0, 2) * 2 - 1;
            }

            coll.isTrigger = true;
            return;
        }

        if (cd <= 0 && findz.dashing == false && !parryFlourishing)
        {
            source.PlayOneShot(clip);

            cd = cdtimer;
            hp-=dmgNum;
            if (xposDamage > rb.position.x)
            {
                findz.beinghit = true;
                findz.dirhitfrom = -1;
                findz.hittimer = 300 / 80 / 7.5f;

            }
            else
            {
                findz.beinghit = true;
                findz.dirhitfrom = 1;
                findz.hittimer = 300 / 80 / 7.5f;
            }
        }
    }

    static float initParryFlourishCD = 0.5f;

    float parryFlourishCD = initParryFlourishCD;

    private void OnTriggerStay2D(Collider2D collision)
    {


        Rigidbody2D temp = collision.GetComponent<Rigidbody2D>();
        if ((collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("Monster")) || collision.GetComponent<BoxCollider2D>().tag.Equals("DoubleDamage") || collision.gameObject.CompareTag("EnemyAttackNoEnergy"))
        {

            //Debug.Log("Detected ");
            if (parryFlourishing)
            {
                if(parryFlourishCD >= 0)
                {
                    return;
                }
                else
                {
                    parryFlourishCD = initParryFlourishCD;
                    source.PlayOneShot(parrySound);

                    findz.parrybdur = initparryflourishtimer / 2;
                    findz.dirParriedFrom = -1 * (int)Mathf.Sign(collision.gameObject.GetComponent<Transform>().position.x - rb.position.x);

                    if (findz.isGrounded) findz.dirForParryKbVert = 0;
                    if (!findz.isGrounded) findz.dirForParryKbVert = 1;

                    if (findz.dirParriedFrom == 0)
                    {
                        findz.dirParriedFrom = UnityEngine.Random.Range(0, 2) * 2 - 1;
                    }

                    parrying = false;
                    parryFlourishing = true;
                    parryFlourishTimer = initparryflourishtimer;

                    coll.isTrigger = true;
                }
                return;
            }


            if (parrying)

            {
                source.PlayOneShot(parrySound);

                findz.parrybdur = initparryflourishtimer / 2;
                findz.dirParriedFrom = -1 * (int)Mathf.Sign(collision.gameObject.GetComponent<Transform>().position.x - rb.position.x);

                if (findz.isGrounded) findz.dirForParryKbVert = 0;
                if (!findz.isGrounded) findz.dirForParryKbVert = 1;

                if (findz.dirParriedFrom == 0)
                {
                    findz.dirParriedFrom = UnityEngine.Random.Range(0, 2) * 2 - 1;
                }

                parryFlourishCD = initParryFlourishCD;


                parrying = false;
                parryFlourishing = true;
                parryFlourishTimer = initparryflourishtimer;

                coll.isTrigger = true;

                return;
            }




            if (cd <= 0)
            {
                source.PlayOneShot(clip);

                cd = cdtimer;
                if (collision.gameObject.tag.Equals("DoubleDamage"))
                {
                    hp -= 2;
                }
                else
                {
                    hp--;

                }



                if (collision.gameObject.GetComponent<Transform>().position.x > rb.position.x)
                {
                    findz.beinghit = true;
                    findz.dirhitfrom = -1;
                    findz.hittimer = 300 / 80 / 7.5f;

                }
                else
                {
                    findz.beinghit = true;
                    findz.dirhitfrom = 1;
                    findz.hittimer = 300 / 80 / 7.5f;
                }
            }



        }

        if(collision.GetComponent<BoxCollider2D>().tag.Equals("DoubleDamage"))
        {
            /*
            if (parrying)

            {
                source.PlayOneShot(parrySound);

                parryFlourishCD = 0.5f;

                findz.parrybdur = initparryflourishtimer / 2;
                findz.dirParriedFrom = -1 * (int)Mathf.Sign(collision.gameObject.GetComponent<Transform>().position.x - rb.position.x);
                if (findz.isGrounded) findz.dirForParryKbVert = 0;
                if (!findz.isGrounded) findz.dirForParryKbVert = 1;

                if (findz.dirParriedFrom == 0)
                {
                    findz.dirParriedFrom = UnityEngine.Random.Range(0,2) * 2 -1;
                }
                Debug.Log(findz.dirParriedFrom);

                parrying = false;
                parryFlourishing = true;
                parryFlourishTimer = initparryflourishtimer;

                coll.isTrigger = true;

                return;
            }
            




            if (cd <= 0)
            {
                source.PlayOneShot(clip);

                cd = cdtimer;
                hp-=2;
                if (collision.gameObject.transform.position.x > rb.position.x)
                {
                    findz.beinghit = true;
                    findz.dirhitfrom = -1;
                    findz.hittimer = 300 / 80 / 7.5f;

                }
                else
                {
                    findz.beinghit = true;
                    findz.dirhitfrom = 1;
                    findz.hittimer = 300 / 80 / 7.5f;
                }
            }
            */
        }





        if (collision.gameObject.CompareTag("Trap"))
        {
            source.PlayOneShot(clip);

            hp--;
            rb.position = new Vector2(findz.lastgrndlocx, findz.lastgrndlocy);

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


}
