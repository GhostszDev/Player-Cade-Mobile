using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private float bulletSpeed = Screen.height * 15 ;
    
    // Start is called before the first frame update
    void Start()
    {

        if (!rb2d)
        {
            this.gameObject.AddComponent<Rigidbody2D>();
        }

        rb2d = this.gameObject.GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2d.velocity = Vector2.up * bulletSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "deathBoxTop")
        {
            Destroy(this.gameObject);
        }
    }
}
