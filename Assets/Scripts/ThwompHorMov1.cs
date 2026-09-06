using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThwompHorMov1 : MonoBehaviour
{
    // Start is called before the first frame update
    Boolean goleft = false;

    Rigidbody2D rigidbody2;
    int count = 0;
    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(count == 3000)
        {
            goleft = true;
        }
        if(count == 6000)
        {
            goleft = false;
            count = 0;
        }
        if (!goleft)
        {
            Vector2 x = new Vector2(15, 0);
            rigidbody2.velocity = x;
        }
        else
        {
            Vector2 x = new Vector2(-15, 0);
            rigidbody2.velocity = x;
        }
        count++;
    }
}
