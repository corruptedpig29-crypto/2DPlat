using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class attack : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private PlayerMovement Findz;

    public Boolean attackbegan = false;



    public Boolean CameInContact = false;
    BoxCollider2D attackCollider;

    float playerstartdirY = 0;
    bool hittingvert = false;

    public int attackType;

    Animator anim;
    PlayerEnergyControl energyctrl;
    // Start is called before the first frame update
    void Start()
    {
        Findz = FindObjectOfType<PlayerMovement>();
        attackCollider = GetComponent<BoxCollider2D>();
        energyctrl = FindObjectOfType<PlayerEnergyControl>();
        anim = GetComponent<Animator>();

        transform.rotation = Quaternion.Euler(0, 0, 90);

        attackType = 0;
        attackCollider.isTrigger = true;

    }

    float dashInitialDir;

    // Update is called once per frame

    float dashAttackSpeed = 250f;

    void Update()
    {
        //Debug.Log(Findz.dirY);


        //Debug.Log(transform.rotation.z);
       

        if (Findz.acd >= 0.125f && Input.GetMouseButtonDown(0) && !attackbegan)
        {
            attackbegan = true;
            attackCollider.enabled = true;
            Findz.acd =0.125f;
            playerstartdirY = Findz.dirY;
            dashAttackSpeed = 250f;


            if((Findz.fastRunning == true || Findz.justgrappled || Findz.timerPostDashForGenericDashAttack > 0) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) {
                Findz.timerPostDashForGenericDashAttack = 0;
                attackType = 2;
                dashInitialDir = Findz.mostrecdirX;

                Findz.grappling = false;
                Findz.grappletimer = 0;

                transform.localPosition = new Vector2( 3 * dashInitialDir,2);
            }
            else
            {
                transform.localPosition = new Vector2(0, 10 * playerstartdirY);
                attackType = 1;
            }

        }

        if(!attackbegan)
        {
            attackCollider.enabled = false;
        }




        if (attackbegan && attackType == 1)
        
        {
            anim.SetInteger("attackNum", 1);

            if (playerstartdirY != 0)
            {
               
                transform.localPosition = new Vector2(0, 3 *playerstartdirY);
                transform.rotation = Quaternion.Euler(0, 0, 90);
                hittingvert = true;



                if(playerstartdirY > 0)
                {
                    transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(Math.Abs(transform.localScale.x) * - 1, transform.localScale.y, transform.localScale.z);
                }

            }
            else
            {
                transform.localPosition = new Vector2(3 * Findz.mostrecdirX, 0.2f);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                hittingvert = false;

                if (Findz.mostrecdirX > 0)
                {
                    transform.localScale = new Vector3( Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(Math.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
                }
            }



        }


        if(attackbegan && attackType == 2)
        {

            transform.rotation = Quaternion.Euler(0, 0, 90);




            transform.localPosition = new Vector2(0, 0);




            anim.SetInteger("attackNum", 2);

            transform.rotation = Quaternion.Euler(0, 0, 0);
            hittingvert = false;


            if (dashInitialDir > 0)
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
            }



            Findz.dashing = false;
            Findz.rb.velocity = new Vector2(dashInitialDir * dashAttackSpeed, Findz.rb.velocity.y);

            dashAttackSpeed+= 850f * Time.deltaTime;

        }


        if (attackbegan && Findz.acd >= 0.125f * 2 &&attackType == 1)
            {
                Findz.acd = 0;

                attackCollider.enabled = false;

                attackbegan = false;

                Findz.dirpushedhorizonhit = 0;
                Findz.dirpushedvertonhit = 0;

                anim.SetInteger("attackNum", 0);

        }


        if (attackbegan && Findz.acd >= 0.125f * 3 && attackType == 2)
        {
            Findz.acd = 0;

            attackCollider.enabled = false;

            attackbegan = false;

            Findz.dirpushedhorizonhit = 0;
            Findz.dirpushedvertonhit = 0;

            anim.SetInteger("attackNum", 0);

        }



        if (!attackbegan)
        {
            Findz.dirpushedhorizonhit = 0;
            anim.SetInteger("attackNum", 0);
            attackCollider.enabled = false;

            Findz.dirpushedvertonhit = 0;
        }




    }


    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Monster") || collision.gameObject.CompareTag("EnemyAttackNoEnergy"))
        {
            if(collision.gameObject.CompareTag("Monster"))energyctrl.energy += 7f;
            CameInContact = true;

            UniHpManager hpManager = null;
            if (collision.gameObject.CompareTag("Monster")) {
                hpManager = collision.gameObject.GetComponent<UniHpManager>();

                if (hpManager.timeSinceLastDamage > 0.2f)
                {
                    collision.gameObject.GetComponent<UniHpManager>().hp--;
                    collision.gameObject.GetComponent<UniHpManager>().tookdamage = true;
                    collision.gameObject.GetComponent<UniHpManager>().timeSinceLastDamage = 0f;
                }
            }
            Findz.atkkbdur = 0.2f;

            if (!hittingvert)
            {
                Findz.dirpushedhorizonhit = (int)Mathf.Sign(Findz.gameObject.transform.position.x - collision.gameObject.transform.position.x);
            }
            else
            {
                Findz.dirpushedvertonhit = (int)Mathf.Sign(Findz.gameObject.transform.position.y - collision.gameObject.transform.position.y);

                if (Findz.dirpushedvertonhit > 0) Findz.dashable = true;
            }



            if(attackbegan && attackType == 2)
            {
                Findz.rb.velocity = new Vector2(-30, dashAttackSpeed/2);
                Findz.acd = 0;

                attackCollider.enabled = false;

                attackbegan = false;

                Findz.dirpushedhorizonhit = 0;
                Findz.dirpushedvertonhit = 0;

                anim.SetInteger("attackNum", 0);
            }

        }



    }


}


/*

public class attack : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Rigidbody2D rb;
    private PlayerMovement Findz;

    public Boolean attackbegan = false;



    public Boolean CameInContact = false;
    BoxCollider2D attackCollider;

    float playerstartdirY = 0;
    bool hittingvert = false;

    public int attackType;

    Animator anim;
    PlayerEnergyControl energyctrl;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Findz = FindObjectOfType<PlayerMovement>();
        attackCollider = GetComponent<BoxCollider2D>();
        energyctrl = FindObjectOfType<PlayerEnergyControl>();
        anim = GetComponent<Animator>();

        transform.rotation = Quaternion.Euler(0, 0, 90);

        attackType = 0;
        attackCollider.isTrigger = true;

    }

    float dashInitialDir;

    // Update is called once per frame

    float dashAttackSpeed = 250f;

    void Update()
    {
        //Debug.Log(Findz.dirY);
        rb.freezeRotation = true;


        //Debug.Log(transform.rotation.z);


        if (Findz.acd >= 0.125f && Input.GetMouseButtonDown(0) && !attackbegan)
        {
            attackbegan = true;
            attackCollider.enabled = true;
            Findz.acd = 0.125f;
            playerstartdirY = Findz.dirY;
            dashAttackSpeed = 250f;


            if (Findz.fastRunning == true)
            {
                attackType = 2;
                dashInitialDir = Findz.mostrecdirX;
                Vector2 vector2 = new Vector2(Findz.rb.position.x + 3 * dashInitialDir, Findz.rb.position.y + 2);



                rb.position = vector2;
            }
            else
            {
                Vector2 vector2 = new Vector2(Findz.rb.position.x, Findz.rb.position.y + 10 * playerstartdirY);
                rb.position = vector2;
                attackType = 1;
            }

        }

        if (!attackbegan)
        {
            attackCollider.enabled = false;
        }




        if (attackbegan && attackType == 1)

        {
            anim.SetInteger("attackNum", 1);

            if (playerstartdirY != 0)
            {
                rb.velocity = new Vector2(Findz.dirX * Findz.movespeed, Findz.rb.velocity.y);
                Vector2 vector2 = new Vector2(Findz.rb.position.x, Findz.rb.position.y + 10 * playerstartdirY);
                rb.position = vector2;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                hittingvert = true;



                if (playerstartdirY > 0)
                {
                    transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(Math.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
                }

            }
            else
            {
                rb.velocity = new Vector2(Findz.dirX * Findz.movespeed, Findz.rb.velocity.y);
                Vector2 vector2 = new Vector2(Findz.rb.position.x + 10 * Findz.mostrecdirX, Findz.rb.position.y + 2);
                rb.position = vector2;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                hittingvert = false;

                if (Findz.mostrecdirX > 0)
                {
                    transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(Math.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
                }
            }



        }


        if (attackbegan && attackType == 2)
        {

            transform.rotation = Quaternion.Euler(0, 0, 90);




            Vector2 vector2 = new Vector2(Findz.rb.position.x - 2 * dashInitialDir, Findz.rb.position.y);



            rb.position = vector2;

            anim.SetInteger("attackNum", 2);

            transform.rotation = Quaternion.Euler(0, 0, 0);
            hittingvert = false;


            if (dashInitialDir > 0)
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
            }



            Findz.dashing = false;
            Findz.rb.velocity = new Vector2(dashInitialDir * dashAttackSpeed, Findz.rb.velocity.y);

            dashAttackSpeed += 1000f * Time.deltaTime;

        }


        if (attackbegan && Findz.acd >= 0.125f * 2)
        {
            Findz.acd = 0;

            attackCollider.enabled = false;

            attackbegan = false;

            Findz.dirpushedhorizonhit = 0;
            Findz.dirpushedvertonhit = 0;

            anim.SetInteger("attackNum", 0);

        }

        if (!attackbegan)
        {
            Findz.dirpushedhorizonhit = 0;
            anim.SetInteger("attackNum", 0);
            attackCollider.enabled = false;

            Findz.dirpushedvertonhit = 0;
        }




    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            energyctrl.energy += 7f;
            CameInContact = true;
            collision.gameObject.GetComponent<UniHpManager>().hp--;
            collision.gameObject.GetComponent<UniHpManager>().tookdamage = true;
            Findz.atkkbdur = 0.2f;

            if (!hittingvert)
            {
                Findz.dirpushedhorizonhit = (int)Mathf.Sign(Findz.gameObject.transform.position.x - collision.gameObject.transform.position.x);
            }
            else
            {
                Findz.dirpushedvertonhit = (int)Mathf.Sign(Findz.gameObject.transform.position.y - collision.gameObject.transform.position.y);

                if (Findz.dirpushedvertonhit > 0) Findz.dashable = true;
            }

        }

    }


}
*/

