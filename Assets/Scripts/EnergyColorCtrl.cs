using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

using UnityEngine.UI;
public class EnergyColorCtrl : MonoBehaviour
{
    // Start is called before the first frame update

    Image img;
    PlayerEnergyControl energybarctrl;
    void Start()
    {
        img = GetComponent<Image>();
        energybarctrl = FindObjectOfType<PlayerEnergyControl>();

    }

    // Update is called once per frame
    void Update()
    {
        if (energybarctrl.energy > 33f)
        {

            img.color = new Color(0.72f,1f,0.73f);
        }
        else
        {
            img.color = new Color(184f / 255f, 225f / 255f, 189f / 255f);
        }


    }
}
