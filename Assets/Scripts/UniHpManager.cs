using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniHpManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int hp = 5;
    public Boolean tookdamage = false;
    public float timeSinceLastDamage = 0f;
    void Start()
    {
        if(GetComponent<bossmover>() != null)
        {
            hp = 30;
        }
        if (GetComponent<wizkingmovement>() != null)
        {
            hp = 20;
        }
        if (GetComponent<monkeyman>() != null)
        {
            hp = 5;
        }

        if(GetComponent<SwordMasterMovement>()!= null)
        {
            hp = 50;
        }


        if (GetComponent<bigManControl>() != null)
        {
            hp = 30;
        }
        if(gameObject.name == "Circle")
        {
            hp = 50000;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0)
        {
            Destroy(this.gameObject);
        }
        timeSinceLastDamage += Time.deltaTime;
    }
}
