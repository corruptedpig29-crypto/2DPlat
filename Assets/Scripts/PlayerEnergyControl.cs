using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergyControl : MonoBehaviour
{
    // Start is called before the first frame update

    public float maxenergy;
    public float energy = 0f;

    float healenergamt = 27f;

    public bool healing = false;
    private float healrate = 27f;
    private bool firsthealdone = false;
    PlayerMovement findz;
    dcoll php;

    Rigidbody2D rb;


    float superJumpTimer = 1f;
    float dirFacingAfterPressed = 0f;
    public bool abilityMoving = false;


    [SerializeField] GameObject throwGrapple;
    [SerializeField] GameObject hollowPurple;


    int numTimesDashed = 0;
    void Start()
    {
        maxenergy = 100f;

        php = FindObjectOfType<dcoll>();    
        findz = FindObjectOfType<PlayerMovement>(); 

        energy = maxenergy;

        rb = findz.GetComponent<Rigidbody2D>();
        initscale = rb.gravityScale;

    }

    // Update is called once per frame

    float initscale = 0f;
    void Update()
    {

        if (healing)
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = initscale;
        }
        if (energy > maxenergy) energy = maxenergy;

        if (energy <= 0f) energy = 0f;

        healrate = 54f;



        if (Input.GetKeyDown(KeyCode.R) && !healing && energy > healenergamt)
        {
            healing = true;
        }


        /*
        if (Input.GetKeyDown(KeyCode.Q) && !healing && energy >= 33f)
        {
            Instantiate(throwGrapple, findz.transform.position, Quaternion.identity);

            //energy -= 33f;
        }
        */

        if (healing)
        {

            findz.rb.velocity = new Vector2(0, 0);
            healenergamt -= Time.deltaTime * healrate;
            energy -= Time.deltaTime * healrate;




            if (healenergamt <= 0f)
            {
                firsthealdone = true;
                php.hp++;

                if (firsthealdone)
                {
                    healrate = 80f;
                    healenergamt = 27f;
                }

                if (php.hp == 5f || energy < healenergamt)
                {
                    Debug.Log("healed");
                    healrate = 27f;
                    healenergamt = 27f;
                    healing = false;
                }


            }

            //rb.gravityScale = 0f;



        }



        if ((Input.GetKeyUp(KeyCode.R) || findz.beinghit) && healing)
        {
            healing = false;
            firsthealdone = false;
            healrate = 54f;
            healenergamt = 27f;
        }



        if (Input.GetKeyDown(KeyCode.T) && energy >= 1f)
        {
            energy -= 1f;


            Vector2 pos = new Vector2(findz.transform.position.x + 10 * findz.mostrecdirX, findz.transform.position.y); 
            slopsaki purpleScript = Instantiate(hollowPurple, pos, Quaternion.identity).GetComponent<slopsaki>();
            purpleScript.speed = 70f;
            purpleScript.dirX = findz.mostrecdirX;

        }


        if (Input.GetKeyDown(KeyCode.G))
        {
            findz.MoveBufferTimer = 0.1f;


            if (superJumpTimer > 0f)
            {
                numTimesDashed++;
            }
            else
            {
                dirFacingAfterPressed = findz.mostrecdirX;
            }

            abilityMoving = true;
            superJumpTimer = 1f;
        }

        if (superJumpTimer <= 0f)
        {
            abilityMoving = false;
            numTimesDashed = 0;
        }
        else
        {

            //Debug.Log("Speed this time  :  " + dirFacingAfterPressed * numTimesDashed * 50f * superJumpTimer * superJumpTimer + (float)(500f * superJumpTimer * superJumpTimer * superJumpTimer * superJumpTimer) * dirFacingAfterPressed);
            rb.velocity = new Vector2(dirFacingAfterPressed * numTimesDashed * 50f * superJumpTimer * superJumpTimer + (float)(500f * superJumpTimer * superJumpTimer) * dirFacingAfterPressed, 0.1f * 250f * superJumpTimer);
        }

        superJumpTimer -= Time.deltaTime;


        if(abilityMoving == false)
        {
            numTimesDashed = 0;
            superJumpTimer = 0f;
        }

    }
}
