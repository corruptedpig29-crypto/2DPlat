using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class slopsaki : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D rb;
    public float speed = 70f;
    public float dirX = 1f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(speed * dirX, 0f);
    }
}
