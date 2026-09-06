using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallMover : MonoBehaviour
{
    // Start is called before the first frame update
    BoxCollider2D coll;
    public int vertDirMove;
    public int horizDirMove;


    public float heightScale = 1f;
    public float widthScale = 1f;
    
    public float speed;

    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {


            if(vertDirMove != 0)transform.localScale = new Vector3(300 * widthScale, 60 * heightScale, 1);
            if(horizDirMove !=0)transform.localScale = new Vector3(60 * widthScale, 300 * heightScale, 1);
    }

    private void FixedUpdate()
    {
        Vector2 target = new Vector2(horizDirMove, vertDirMove) * 10000;
        transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed / 2);
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);

        }
    }
    /*

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.transform.parent == null )
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);

        }
    }
    */
}
