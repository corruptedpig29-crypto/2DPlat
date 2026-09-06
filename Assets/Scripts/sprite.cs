using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sprite : MonoBehaviour
{

    public Rigidbody2D myRigidbody;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Update is called once per frame


     public bool apressed = false;
     public bool spressed = false;
     public bool dpressed = false;
     public int movespeed = 5;
    void Update()
    {
        if(movespeed>50){
            movespeed = 50;
        }

        if(Input.GetKeyDown(KeyCode.W) == true){
            myRigidbody.velocity = Vector2.up*20;
        }

        if(Input.GetKeyDown(KeyCode.A) == true){
            apressed = true;
            movespeed+=1;
        }
        if(Input.GetKeyUp(KeyCode.A) == true){
            apressed = false;
        }
        if(Input.GetKeyDown(KeyCode.D) == true){
           dpressed = true;
           
        }
        if(Input.GetKeyUp(KeyCode.D) == true){
            dpressed = false;
        }

        if(apressed){
            myRigidbody.velocity = Vector2.left*movespeed;
            movespeed+=1;
        }
        if(dpressed){
            myRigidbody.velocity = Vector2.right*movespeed;
            movespeed+=1;
        }
        
        if(movespeed<0){
            movespeed =0;
        }
         if(dpressed ==false&& apressed ==false&& movespeed>0){
             movespeed -=20;
        }
    }
}

