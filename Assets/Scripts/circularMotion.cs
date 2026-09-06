using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circularMotion : MonoBehaviour
{
    // Start is called before the first frame update

    float xMotion;
    float yMotion;
    Rigidbody2D rb;

    [SerializeField]float circleRadius = 10f;
    [SerializeField] float rotationScale = 1f;
    float initTime = 0f;

    GameObject child;
    float initX = 0f;
    float initY = 0f;

    float startingAngle = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initX = transform.position.x;
        initY = transform.position.y;
        child = transform.parent.Find("CenterPoint").gameObject;


        startingAngle = Mathf.Atan2(transform.position.y - child.transform.position.y, transform.position.x - child.transform.position.x);

        circleRadius = (child.transform.position - transform.position).magnitude;

        rb.position = new Vector2(initX,initY);
        initTime = Time.time;

        
    }

    // Update is called once per frame
    void Update()
    {
        xMotion = child.transform.position.x + Mathf.Cos((Time.time-initTime) * rotationScale + startingAngle) * circleRadius;
        yMotion = child.transform.position.y + Mathf.Sin((Time.time-initTime) * rotationScale + startingAngle) * circleRadius;
        Vector2 targetPosition = new Vector2(xMotion, yMotion);
        transform.position = Vector2.MoveTowards(transform.position,targetPosition , Time.deltaTime * rotationScale * circleRadius);
       
            
    }
}
