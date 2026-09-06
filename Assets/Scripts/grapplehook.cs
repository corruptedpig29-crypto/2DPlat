using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class grapplehook : MonoBehaviour
{
    PlayerMovement x;
    public Rigidbody2D rb;
    List<GameObject> arrlist = new List<GameObject>();

    public float gy = 0;
    public float gx = 0;


    public float throwableposx = 0;

    public float throwableposy = 0;
    public bool grapplingToSling = false;



    private Vector3 grapplepoint;

    private LineRenderer rope;

    float grappledur = 0.225f;
    private float grapplelength = 100f;


    [SerializeField] GameObject frog;


    public bool grappling = false;

    float extradist = 30f;

    private DistanceJoint2D joint;
    // Start is called before the first frame update
    void Start()
    {
        joint = frog.GetComponent<DistanceJoint2D>();
        x = FindObjectOfType<PlayerMovement>();
        rb = FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody2D>();
        rope = GetComponent<LineRenderer>();
        joint.enabled = false;
        rope.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        string[] layerNames = { "Ground", "Enemy", "ProjGround"};
        string[] grappleLayerNames = { "Ground", "GrapplePoints" };








        if (x.grappling)
        {
            grappling = true;
        }
        else
        {
            grappling = false;
        }

        if (arrlist == null)
        {
            return;
        }





        //rb.position = x.rb.position;


        if (Input.GetKeyDown(KeyCode.F))
        {


            grapplingToSling = false;

            if (arrlist.Count > 0)
            {


                if (arrlist.Count == 1)
                {

                    Vector2 direfaced = new Vector2((float)arrlist[0].transform.position.x - rb.position.x, (float)arrlist[0].transform.position.y - rb.position.y);

                    RaycastHit2D z = Physics2D.Raycast(x.gameObject.GetComponent<Rigidbody2D>().position, direfaced, grapplelength, LayerMask.GetMask(grappleLayerNames));

                    if (z.collider.gameObject.CompareTag("gpoint"))
                    {
                        //Debug.Log(z.collider.gameObject.tag);

                        grapplingToSling = false;

                        gx = arrlist[0].transform.position.x;
                        gy = arrlist[0].transform.position.y;





                        x.grappling = true;
                        x.dashable = true;
                        x.grappletimer = grappledur;
                        if (arrlist[0].GetComponent<throwableGrapple>() != null)
                        {

                            throwableposx = gx;

                            throwableposy = gy;

                            grapplingToSling = true;

                            Debug.Log("Grappling to throwable");


                            Vector2 directionToPoint = new Vector2( -(rb.position.x - arrlist[0].transform.position.x), -(rb.position.y - arrlist[0].transform.position.y)).normalized;

                            gx = arrlist[0].transform.position.x + directionToPoint.x * extradist;
                            gy = arrlist[0].transform.position.y + directionToPoint.y * extradist;

                            

                            arrlist[0].GetComponent<throwableGrapple>().beingGrappledTo = true;

                            arrlist.RemoveAt(0);

                        }


                    }
                    else
                    {
                        Vector2 dirfaced = new Vector2(x.mostrecdirX, 0);

                        RaycastHit2D ze = Physics2D.Raycast(x.gameObject.GetComponent<Rigidbody2D>().position, dirfaced, grapplelength, LayerMask.GetMask(layerNames));

                        if (ze.collider != null)
                        {
                           // Debug.Log(z.collider.gameObject.tag);

                            gx = ze.point.x;
                            gy = ze.point.y;
                            x.grappling = true;
                            x.dashable = true;
                            x.grappletimer = grappledur;



                        }
                    }


                    //gx = (float)arrlist[0];
                    //gy = (float)arrlist[1];
                    ////Debug.Log(gx + " " + gy);


                    //x.grappling = true;
                    //x.dashable = true;
                    //x.grappletimer = 0.3f;


                }
                else
                {
                    bool notfacingany = true;


                    for (int i = 0; i < arrlist.Count; i ++)
                    {
                        if (x.mostrecdirX == 1 && arrlist[i].transform.position.x >= rb.position.x)
                        {
                            Vector2 dirfaced = new Vector2(arrlist[i].transform.position.x - rb.position.x, arrlist[i].transform.position.y - rb.position.y).normalized;

                            RaycastHit2D z = Physics2D.Raycast(x.gameObject.GetComponent<Rigidbody2D>().position, dirfaced, grapplelength, LayerMask.GetMask(grappleLayerNames));

                            if (z.collider.gameObject.CompareTag("gpoint"))
                            {
                                notfacingany = false;
                            }
                        }


                        if (x.mostrecdirX == -1 && arrlist[i].transform.position.x <= rb.position.x)
                        {
                            Vector2 dirfaced = new Vector2(arrlist[i].transform.position.x - rb.position.x, arrlist[i].transform.position.y - rb.position.y).normalized;

                            RaycastHit2D z = Physics2D.Raycast(x.gameObject.GetComponent<Rigidbody2D>().position, dirfaced, grapplelength, LayerMask.GetMask(grappleLayerNames));

                            if (z.collider.gameObject.CompareTag("gpoint"))
                            {
                                notfacingany = false;
                            }
                        }
                    }


                    float curmindist = float.MaxValue;
                    float dir = x.mostrecdirX;
                    float px = x.rb.position.x;
                    float py = x.rb.position.y;



                    int dex = -1;


                    //Debug.Log(notfacingany);



                    for(int i =0; i < arrlist.Count; i++)
                    {
                        //Debug.Log(arrlist[i].transform.position.x + "  " + arrlist[i].transform.position.y);

                    }

                    if (notfacingany)
                    {
                        Debug.Log("not facing any");

                        for (int i = 0; i < arrlist.Count; i ++)
                        {
                                
                            if (Mathf.Sqrt((arrlist[i].transform.position.x - rb.position.x) * (arrlist[i].transform.position.x - rb.position.x) + (arrlist[i].transform.position.y - rb.position.y) * (arrlist[i].transform.position.y - rb.position.y)) < curmindist)
                            {

                                Vector2 dirfaced = new Vector2(arrlist[i].transform.position.x - rb.position.x, arrlist[i].transform.position.y - rb.position.y).normalized;

                                RaycastHit2D z = Physics2D.Raycast(x.gameObject.GetComponent<Rigidbody2D>().position, dirfaced, grapplelength, LayerMask.GetMask(grappleLayerNames));

                                if (z.collider.gameObject.CompareTag("gpoint"))
                                {
                                        
                                    curmindist = Mathf.Sqrt((arrlist[i].transform.position.x - rb.position.x) * (arrlist[i].transform.position.x - rb.position.x) + (arrlist[i].transform.position.y - rb.position.y) * (arrlist[i].transform.position.y - rb.position.y));
                                    dex = i;

                                }
                            }
                        }
                    }
                    else
                    {

                        for (int i = 0; i < arrlist.Count; i ++)
                        {

                            if (Mathf.Sqrt((arrlist[i].transform.position.x - rb.position.x) * (arrlist[i].transform.position.x - rb.position.x) + (arrlist[i].transform.position.y - rb.position.y) * (arrlist[i].transform.position.y - rb.position.y)) < curmindist)
                            {




                                if (x.mostrecdirX == 1 && arrlist[i].transform.position.x >= px)
                                {



                                    Vector2 dirfaced = new Vector2(arrlist[i].transform.position.x - rb.position.x, arrlist[i].transform.position.y - rb.position.y).normalized;




                                    RaycastHit2D z = Physics2D.Raycast(x.gameObject.GetComponent<Rigidbody2D>().position, dirfaced, grapplelength, LayerMask.GetMask(grappleLayerNames));

                                    if (z.collider.gameObject.CompareTag("gpoint"))
                                    {
                                        curmindist = Mathf.Sqrt((arrlist[i].transform.position.x - rb.position.x) * (arrlist[i].transform.position.x - rb.position.x) + (arrlist[i].transform.position.y - rb.position.y) * (arrlist[i].transform.position.y - rb.position.y));
                                        dex = i;
                                        Debug.Log("hello");
                                    }
                                    else
                                    {

                                    }

                                }



                                else if (x.mostrecdirX == -1 && (float)arrlist[i].transform.position.x <= px)
                                {


                                    Debug.Log("C2");


                                    Vector2 dirfaced = new Vector2((float)arrlist[i].transform.position.x - rb.position.x, (float)arrlist[i].transform.position.y - rb.position.y).normalized;


                                    RaycastHit2D z = Physics2D.Raycast(x.gameObject.GetComponent<Rigidbody2D>().position, dirfaced, grapplelength, LayerMask.GetMask(grappleLayerNames));

                                    if (z.collider.gameObject.CompareTag("gpoint"))
                                    {
                                        curmindist = Mathf.Sqrt(((float)arrlist[i].transform.position.x - rb.position.x) * ((float)arrlist[i].transform.position.x - rb.position.x) + ((float)arrlist[i].transform.position.y - rb.position.y) * ((float)arrlist[i].transform.position.y - rb.position.y));
                                        dex = i;
                                        Debug.Log("it worked?");


                                    }
                                }
                                else
                                {
                                    Debug.Log("Should never be happening");
                                }
                            }
                            else
                            {
                                Debug.Log("this shouldn't be happening");
                            }
                        }
                    }


                    if (arrlist.Count == 0)
                    {
                        return;
                    }

                    if (curmindist != float.MaxValue)
                    {
                        x.grappling = true;
                        x.dashable = true;
                    }

                    //HAD AN ISSUE WHERE DEX WOULD NEVER CHANGE TO ANY OTHER VALUE FROM -1, SEEMINGLY FIXED BUT IF PERSISTS THESE ARE LOGS LEFT.


                    if (dex != -1)
                    {


                        gx = (float)arrlist[dex].transform.position.x;
                        gy = (float)arrlist[dex].transform.position.y;
                        grapplingToSling = false;


                        if (arrlist[dex].GetComponent<throwableGrapple>() != null)
                        {

                           


                            throwableposx = gx;

                            throwableposy = gy;


                            arrlist[dex].GetComponent<throwableGrapple>().beingGrappledTo = true;

                            grapplingToSling = true;

                            Debug.Log("Grappling to throwable");


                            Vector2 directionToPoint = new Vector2(-(rb.position.x - arrlist[dex].transform.position.x), -(rb.position.y - arrlist[dex].transform.position.y)).normalized;

                            gx = arrlist[dex].transform.position.x + directionToPoint.x * extradist;
                            gy = arrlist[dex].transform.position.y + directionToPoint.y * extradist;    

                            arrlist.RemoveAt(dex);


                        }



                        x.grappletimer = grappledur;

                    }
                    else
                    {


                        Vector2 dirfaced = new Vector2(x.mostrecdirX, 0);

                        RaycastHit2D z = Physics2D.Raycast(x.gameObject.GetComponent<Rigidbody2D>().position, dirfaced, grapplelength, LayerMask.GetMask(layerNames));

                        if (z.collider != null)
                        {

                            if (z.collider.CompareTag("Monster") || z.collider.CompareTag("EnemyAttackNoEnergy"))
                            {
                                gx = z.point.x - 5f * x.mostrecdirX;

                            }
                            else
                            {
                                gx = z.point.x;
                            }
                            gy = z.point.y;
                            x.grappling = true;
                            x.dashable = true;
                            x.grappletimer = grappledur;

                            Debug.Log("case3");
                        }
                    }
                }


            }
            else
            {

                Vector2 dirfaced = new Vector2(x.mostrecdirX, 0);

                RaycastHit2D z = Physics2D.Raycast(x.gameObject.GetComponent<Rigidbody2D>().position, dirfaced, grapplelength, LayerMask.GetMask(layerNames));


                if (z.collider != null)
                {

                    if (z.collider.CompareTag("Monster") || z.collider.CompareTag("EnemyAttackNoEnergy"))
                    {
                        gx = z.point.x - 5f * x.mostrecdirX;

                    }
                    else
                    {
                        gx = z.point.x;
                    }
                    gy = z.point.y;
                    x.grappling = true;
                    x.dashable = true;
                    x.grappletimer = grappledur;
                }


            }



        }










    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("gpoint"))
        {
            arrlist.Add(collision.attachedRigidbody.gameObject);
            arrlist[arrlist.Count-1].GetComponent<SpriteRenderer>().material.color = new Color(0.75f, 0f, 0f);

        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (arrlist == null || collision.attachedRigidbody == null)
        {
            return;
        }
        for (int i = 0; i < arrlist.Count; i++)
        {
            if (i >= arrlist.Count)
            {
                break;
            }
            if (arrlist[i] == null || arrlist[i] == null)
            {


            }
            else
            {
                if (collision.attachedRigidbody.gameObject.Equals(arrlist[i]))
                {

                    arrlist[i].GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f);
                    arrlist.RemoveAt(i);
                    break;
                }
            }

        }
    }
}
