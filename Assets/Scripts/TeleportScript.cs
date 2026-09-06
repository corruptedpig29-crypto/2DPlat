using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{


    [SerializeField]GameObject other;

    float cooldown = 5f;

    BoxCollider2D coll;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Monster"))
        {
            if (cooldown < 0)
            {

                other.GetComponent<TeleportScript>().cooldown = 5f;
                collision.gameObject.transform.position = other.transform.position;
                cooldown = 5f;
            }
        }

    }
}
