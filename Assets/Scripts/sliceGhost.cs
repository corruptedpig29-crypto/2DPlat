using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class sliceGhost : MonoBehaviour
{
    // Start is called before the first frame update
    private enum AnimState { appear, fill, disappear}
    BoxCollider2D coll;
    SpriteRenderer sprite;

    public bool begin = false;
    Animator anim;

    float fadetimer = 0.5f;


    public bool began = false;
    float fadeoutfinishtimer = 1/3f;



    void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        
        anim = GetComponent<Animator>();

        began = false;
        
        sprite.enabled = false;
        coll.enabled = true;
        coll.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (!coll.isTrigger)
        {
            coll.isTrigger = true;
            Debug.Log("hello");
        }


        coll.isTrigger = true;


        if (begin)
        {
            sprite.enabled = true;
            begin = false;
            anim.SetInteger("state", (int)AnimState.appear);

        }



        if (began)
        {
            fadetimer -= Time.deltaTime;

        }
        if(fadetimer <= 0)
        {
            fadeOut();
        }


    }

    public void fill()
    {
        began = true;
        anim.SetInteger("state", (int)AnimState.fill);
    }

    public void fadeOut()
    {
        coll.enabled = false;

        fadeoutfinishtimer -= Time.deltaTime;
        anim.SetInteger("state", (int)AnimState.disappear);


        if(fadeoutfinishtimer <= 0)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if(collision.gameObject.CompareTag("Player") && began)
        {

            collision.gameObject.GetComponentInChildren<dcoll>().manualTakeDamage(transform.position.x,1);
        }
    }


}
