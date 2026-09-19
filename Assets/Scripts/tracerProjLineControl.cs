using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tracerProjLineControl : MonoBehaviour
{
    private LineRenderer lr;
    public float x1;
    public float y1;
    public float x2;
    public float y2;
    tracerLineProjMovement movement;
    BoxCollider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        movement = GetComponent<tracerLineProjMovement>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {


            if (movement.isWaiting)
            {

                //Debug.Log("Waiting");
                lr.positionCount = 2;

                lr.enabled = true;
                //Debug.Log(x1 + " " + y1);

                Vector3 position1 = new Vector3(x1, y1, 0);
                Vector3 position2 = new Vector3(x2, y2, 0);

                lr.SetPosition(0, position1);
                lr.SetPosition(1, position2);

            }

            else
            {
                if (lr.positionCount != 0) lr.positionCount = 0;

            }


    }
}
