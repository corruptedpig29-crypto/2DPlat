using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tpToOtherScrollPlatTP : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject otherScrollRegionTP;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        float xShift = 0f;
        if(this.gameObject.name == "TPLeft")
        {
            xShift = -1f;
        }
        else
        {
            xShift = 1f;
        }
        Vector2 pos;
        if (collision.CompareTag("Projectile"))
        {
            collision.gameObject.transform.position = pos = new Vector2(otherScrollRegionTP.transform.position.x + xShift * 50f,collision.transform.position.y );
        }
        if (collision.CompareTag("Monster"))
        {
            collision.gameObject.transform.position = pos = new Vector2(otherScrollRegionTP.transform.position.x + xShift * 50f, collision.transform.position.y);
        }
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.transform.position = pos = new Vector2(otherScrollRegionTP.transform.position.x + xShift * 50f, collision.transform.position.y);
        }
    }
}
