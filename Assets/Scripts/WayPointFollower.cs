using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject[] points;
    private int waydex = 0;

    private float speed = 20f;

    private void Update()
    {
        if (Vector2.Distance(points[waydex].transform.position, transform.position) < .1f)
        {
            waydex++;
            if(waydex >= points.Length)
            {
                waydex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, points[waydex].transform.position, Time.deltaTime*speed);
    }
}
