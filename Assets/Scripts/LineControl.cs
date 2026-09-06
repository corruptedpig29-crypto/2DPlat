using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineControl : MonoBehaviour
{
    private LineRenderer lr;
    float x1;
    float y1;
    float x2;
    float y2;
    PlayerMovement player;
    grapplehook hookscript;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        hookscript = FindObjectOfType<grapplehook>();
        player = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        
        if (hookscript.grappling)
        {
            if (lr.positionCount == 0)
            {
                lr.positionCount = 2;

                x2 = hookscript.gx;
                y2 = hookscript.gy;

                if(hookscript.grapplingToSling)
                {
                    x2 = hookscript.throwableposx;
                    y2 = hookscript.throwableposy;
                }
            }



            x1 = player.rb.position.x;
            y1 = player.rb.position.y;


            //Debug.Log(x1 + " " + y1);

            Vector3 position1 = new Vector3(x1, y1, 0);
            Vector3 position2 = new Vector3(x2, y2, 0);

            lr.SetPosition(0, position1);
            lr.SetPosition(1, position2);

        }
        else
        {
            if(lr.positionCount!=0) lr.positionCount = 0;

        }

    }
}
