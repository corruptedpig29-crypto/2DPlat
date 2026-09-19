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
    float tiemrToDestroyAfterDetection = 0.03f;
    bool beginDestroyTimer = false; 
    void Start()
    {
        beginDestroyTimer = false;

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


        string[] layerNames2 = { "Player" };
        Vector2 boxsize = new Vector2(coll.bounds.size.x, coll.bounds.size.y);
        float dashdist = 50f;

        Vector2 direction = new Vector2(0f, 0f);

        ContactFilter2D filter2 = new ContactFilter2D();
        filter2.SetLayerMask(LayerMask.GetMask(layerNames2));
        filter2.useTriggers = true;

        RaycastHit2D[] results2 = new RaycastHit2D[1];
        int count2 = Physics2D.BoxCast(transform.position, boxsize, transform.rotation.z, direction, filter2, results2, dashdist);
        RaycastHit2D pz = count2 > 0 ? results2[0] : default;

        if (pz.collider != null && pz.collider.gameObject != null)
        {
            if (pz.collider.gameObject.CompareTag("Player") && pz.collider.gameObject.GetComponentInChildren<dcoll>() != null)
            {
                pz.collider.gameObject.GetComponentInChildren<dcoll>().manualTakeDamage(transform.position.x, 1);
                Destroy(this.gameObject);
            }
        }
    }

    //FIX THIS, ADAPT SO THAT ALL PROJECTILES WORK EVEN WHEN THE COLLIDER IS A TRIGGER AT ALL TIMES.
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);

        }
    }
}
