using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
/*
public class Direction : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Rigidbody2D rb;
    private float dir;
    private PlayerMovement Findz;
    private int count = 0;
    float y = 1;
    private int discd = 0;
    Boolean disap = false;
    private Boolean cantp = false;
    [SerializeField] private LayerMask jumpableground;

    BoxCollider2D coll;
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(0, 3);
        rb = GetComponent<Rigidbody2D>();
        Findz = FindObjectOfType<PlayerMovement>();
        GameObject gameObject = rb.gameObject;
        coll = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {


        if (Findz.fcd >= 2000 && Input.GetKeyDown(KeyCode.F))
        {

            y = 1;
            Vector2 vector2 = new Vector2(Findz.rb.position.x + 5 * Findz.mostrecdirX, Findz.rb.position.y);
            rb.position = vector2;
            Findz.fcd = 0;
            y = Findz.mostrecdirX;
            cantp = true;
        }



        if (Input.GetKeyDown(KeyCode.E) && cantp == true)
        {

            Findz.rb.position = rb.position;

            rb.position = new Vector2(999,999);
            cantp = false;
        }

        rb.velocity = new Vector2(165 * y, 0);
        if(disap == true)
        {
            discd++;
        }
        if (discd > 100)
        {
            Findz.fcd = 2000;
            rb.position = new Vector2(999, 999);
            cantp = false;
            disap = false;
            discd = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {

            disap = true;
            y = 0;
            rb.velocity = new Vector2(0, 0);

        }

    }
}


*/
