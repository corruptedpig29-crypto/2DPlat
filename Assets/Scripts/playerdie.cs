using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerdie : MonoBehaviour
{
    // Start is called before the first frame update
    private dcoll damagecoll;
    private Animator anim;
    private Rigidbody2D rb;

    Boolean died = false;
    int timer = 0;
    void Start()
    {
        
        damagecoll = FindObjectOfType<dcoll>();   
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(damagecoll.hp <= 0 && !died)
        {
            damagecoll.gameObject.transform.SetParent(null);
            Die();
            died = true;
            
        }

    }
    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("Death");
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        died = false;
    }
}
