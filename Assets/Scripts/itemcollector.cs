using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class itemcollector : MonoBehaviour
{
    // Start is called before the first frame update
    private int cherrycount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cherry"))
        {
            cherrycount++;
            Destroy(collision.gameObject);
            Debug.Log("Cherries : " + cherrycount);
        }
    }
}
