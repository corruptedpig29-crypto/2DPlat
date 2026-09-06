using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class EnergyBarController : MonoBehaviour
{
    // Start is called before the first frame update

    Slider slider;

    float energymaxval;
    public float curenergyval;
    PlayerEnergyControl energyctrl;
    dcoll playerscript;
    void Start()
    {
        
        slider =  GetComponent<Slider>(); 
        playerscript = FindObjectOfType<dcoll>();
        energyctrl = FindObjectOfType<PlayerEnergyControl>();  
    }

    // Update is called once per frame
    void Update()
    {



        curenergyval = energyctrl.energy;
        energymaxval = energyctrl.maxenergy;


        slider.maxValue = energymaxval;
        slider.value = curenergyval;

        //Debug.Log(slider.value);
    }
}
