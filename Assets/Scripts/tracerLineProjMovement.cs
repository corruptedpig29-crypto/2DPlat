using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tracerLineProjMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isWaiting = false;
    public float waitTimer = 0.3f;

    public float degreeOfRotation = 35f;

    Rigidbody2D rb;

    BoxCollider2D coll;

    public float speed = 80f;
    public float accel = 500f;
    public int dirX = 1;

    public bool begin = false;

    bool rotated = false;
    SpriteRenderer spriteRenderer;

    tracerProjLineControl tracerProjLine;

    dcoll playerhitbox;


    float timerSinceBegan = 0f;
    void Start()
    {


        timerSinceBegan = 0f;

        playerhitbox = FindObjectOfType<dcoll>();

        rb = GetComponent<Rigidbody2D>();

        coll = GetComponent<BoxCollider2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        tracerProjLine = GetComponent<tracerProjLineControl>();

        isWaiting = false;

        rb.freezeRotation = false;
        rotated = false;
    }

    // Update is called once per frame
    void Update()
    {

            if (!begin)
        {
            return;
        }
        else

        if (waitTimer > 0f)
        {
            isWaiting = true;
            waitTimer -= Time.deltaTime;
            coll.enabled = false;

            spriteRenderer.enabled = false;

            tracerProjLine.x1 = transform.position.x;
            tracerProjLine.y1 = transform.position.y;



            tracerProjLine.x2 = transform.position.x + 1000 * Mathf.Cos(degreeOfRotation * Mathf.PI / 180) * dirX;
            tracerProjLine.y2 = transform.position.y + 1000 * Mathf.Sin(degreeOfRotation * Mathf.PI / 180);


            //if (rotated == false)
            //{
            //    transform.rotation = Quaternion.Euler(0, 0, degreeOfRotation * dirX);

            //    rotated = true;
            //}


        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, degreeOfRotation * dirX);

            timerSinceBegan += Time.deltaTime;

            rb.freezeRotation = true;

            speed += accel * Time.deltaTime;
            spriteRenderer.enabled = true;
            coll.enabled = true;
            rb.velocity = new Vector2(Mathf.Cos(degreeOfRotation * Mathf.PI / 180) * dirX, Mathf.Sin(degreeOfRotation * Mathf.PI / 180)) * speed;


            isWaiting = false;
        }

        if(timerSinceBegan > 5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            playerhitbox.manualTakeDamage(rb.position.x,1);
            Destroy(gameObject);
        }
    }
}
