using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parryScript : MonoBehaviour
{
    // Start is called before the first frame update

    float maxinitframer = 0.113f;

    float parryframetimer = 0.133f;

    PlayerMovement Findz;

    dcoll dmg;
    Rigidbody2D rb;
    BoxCollider2D coll;
    void Start()
    {
        dmg = FindObjectOfType<dcoll>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();   
        Findz = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) )
        {

        }
    }
}
