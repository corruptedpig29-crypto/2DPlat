using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostSMImage : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D rb;

    public float px = 0f;
    public float py = 0f;

    bool moved = false;

    float dirX;

    public float movetimer = 0f;
    void Start()
    {
        moved = false;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {


        if (dirX > 0)
        {

            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        if (!moved)
        {
            rb.position = new Vector2(px, py);
            moved = true;
        }

        if (movetimer > 0f){
            movetimer -= Time.deltaTime;

            rb.velocity = new Vector2(dirX * 100f, 0f);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}
