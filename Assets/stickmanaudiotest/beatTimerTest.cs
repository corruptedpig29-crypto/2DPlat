using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class beatTimerTest : MonoBehaviour
{

    AudioSource source;
    double[] notes = 
{
    44576,
    44926,
    45351,
    45726,
    46151,
    46601,
    47000,
    47425,
    47850,
    48175,
    48600,
    49000,
    49474,
    49874,
    50274,
    50649,
    51124,
    51549,
    51948,
    52348,
    52773,
    53123,
    53573,
    54073,
    54472,
    54847,
    55272,
    55697,
    56147,
    56547,
    56971,
    57371,
    57771,
    58146,
    58571,
    58946,
    59345,
    59820,
    60295,
    60670,
    61120,
    61520,
    61944,
    62319,
    62744,
    63169,
    63594,
    63969,
    64418,
    64768,
    65243,
    65643,
    66068,
    66468,
    66917,
    67267,
    67692,
    68092,
    68492,
    68917,
    69092,
    69267,
    69441,
    70166,
    71166,
    71341,
    71466,
    71641,
    71815,
    71965,
    72265,
    72515,
    72715,
    73090,
    73490,
    74664,
    74814,
    74964,
    75264,
    75539,
    75764,
    75989,
    76339,
    76813,
    77388,
    78063,
    78288,
    78488,
    79188,
    79737,
    80162,
    81312,
    81562,
    81811,
    82411,
    82636,
    83511,
    84410,
    84585,
    84735,
    84885,
    85035,
    85210,
    85510,
    85735,
    85960,
    86360,
    86809,
    87184,
    87409,
    87734,
    88059,
    88384,
    88809,
    89084,
    89383,
    89633,
    89783,
    89933
};

    // Start is called before the first frame update

    float timer = 0f;
    int dex = 0;
    void Start()
    {

        source = GetComponent<AudioSource>();
        timer = 0f;
        //source.Play();


    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (dex < notes.Length - 2)
        {


            if (timer > notes[dex] / 1000 && timer < notes[dex + 1] / 1000)
            {
                Debug.Log("beat" + dex);
                dex++;
            }
        }

    }
}
