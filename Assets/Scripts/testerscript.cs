using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testerscript : MonoBehaviour
{
    // Start is called before the first frame update

    BoxCollider2D coll;
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        Debug.Log(collision.gameObject.name);   
    }
}
