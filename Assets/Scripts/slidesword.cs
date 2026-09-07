using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slidesword : MonoBehaviour
{
    // Start is called before the first frame update


    Rigidbody2D rb;
    BoxCollider2D coll;

    SpriteRenderer sprite;
    public int dirX = 1;

    public float timeTillDeath = 5f;


    public float speed = 50f;

    dcoll playerHP;
    public float accelrate = 150f;

    public float scale = 33f;

    public float scalegrowthrate = 100f;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        Destroy(gameObject, timeTillDeath);

        playerHP = FindObjectOfType<dcoll>();

        sprite = GetComponent<SpriteRenderer>();


        if(dirX == -1)
        {
            sprite.flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(dirX * speed, 0);

        speed += accelrate * Time.deltaTime;

        //Debug.Log(speed);


        scale += Time.deltaTime * scalegrowthrate;
        transform.localScale = new Vector2(scale, scale);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
