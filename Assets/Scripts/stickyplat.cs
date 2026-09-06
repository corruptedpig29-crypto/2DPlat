using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stickyplat : MonoBehaviour
{
    // Start is called before the first frame update


    private void OnTriggerStay2D(Collider2D collision)
    {   
        if(collision.gameObject.name.Equals("Player"))
        {
            collision.gameObject.transform.SetParent(transform);
        }

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")   
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
