using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthui : MonoBehaviour
{

    public int health;
    public int maxHealth;
    public Image[] hearts;
    public Sprite emptyheart;
    public Sprite fullheart;
    dcoll damagecollider;


    // Start is called before the first frame update
    void Start()
    {
        maxHealth = hearts.Length;

        damagecollider = FindObjectOfType<dcoll>();
        
    }

    // Update is called once per frame
    void Update()
    {
        health = damagecollider.hp;
        //Debug.Log(health);

        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].sprite = fullheart;
            }
            else
            {
                hearts[i].sprite = emptyheart;

            }

            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;

            }
        }
    }
}
