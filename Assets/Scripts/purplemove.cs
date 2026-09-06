using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class purplemove : MonoBehaviour
{
    // Start is called before the first frame update
    int timer = 0;
    public Rigidbody2D rb;
    public PlayerMovement findz;
    int scale = 0;
    Vector3 scaleChange = new Vector3(-0.02f, -0.02f, -0.02f);
    Vector3 initscale = new Vector3(-5, -5, -5);
    float dir = 0;
    int count = 0;

    DestructibleTiles tilecode;
    void Start()
    {
        tilecode = FindObjectOfType<DestructibleTiles>();
        rb = GetComponent<Rigidbody2D>();
        findz = FindObjectOfType<PlayerMovement>();



    }

    // Update is called once per frame
    void Update()
    {

        /*
        
        timer--;
        if (timer <=-2000)
        {


            if(Input.GetKeyDown(KeyCode.G)) {


                rb.position = new Vector2(findz.gameObject.transform.position.x + 10f * findz.mostrecdirX, findz.gameObject.transform.position.y);
                rb.velocity = new Vector2(0,0);
                timer = 5000;
                dir = findz.mostrecdirX;
                count++;
            }
            else
            {
                rb.position = new Vector2(99, -999);
            }
        }


        if(timer > -2000 && timer < 4400)
        {
            rb.rotation += 3;
            rb.velocity = new Vector2(99*dir, 0);
        }
        */



    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("Monster") || collision.gameObject.CompareTag("Trap"))
        {
            Destroy(collision.gameObject);

        }
    }
}
