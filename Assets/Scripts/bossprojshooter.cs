using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Jobs;
using static System.Math;

public class bossprojshooter : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject bossproj;
    
    public float timer = 0.5f;
    float radtodeg = 360 / (2 * (float)PI);
    bool changeddifacing = false;
    public float xdir = 1f;
    public float ydir = 0f;
    public float speed = 20f;
    public float accelrate = 200f;
    Rigidbody2D rb;

    Transform transforme;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transforme = GetComponent<Transform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {

            Vector3 pos = new Vector3(rb.position.x + 10*xdir, rb.position.y + 10 * ydir);

            GameObject bossproje = Instantiate(bossproj,pos,Quaternion.identity);

            bossproje.GetComponent<b1Atkscr>().xdir = xdir;
            bossproje.GetComponent<b1Atkscr>().ydir = ydir;
            bossproje.GetComponent<b1Atkscr>().speed = speed;
            bossproje.GetComponent<b1Atkscr>().accelrate = accelrate;
            


            if (!changeddifacing && (xdir != 0 && ydir != 0))
            {
                transforme.Rotate(new Vector3(0, 0, (float)System.Math.Atan(ydir / xdir) * radtodeg));

                changeddifacing = true;
            }
            if (!changeddifacing && (xdir == 0) && (ydir != 0))
            {
                transforme.Rotate(new Vector3(0, 0, 90));

                changeddifacing = true;
            }


            Destroy(this.gameObject);
        }
    }
}
