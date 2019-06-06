using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour {

    public GameObject canvas;

    private Rigidbody2D rb;
    private float lifeTime = Screen.height / 15;

    // Use this for initialization
    void Awake()  {

        rb = GetComponent<Rigidbody2D>();

        if (rb == null) {
            gameObject.AddComponent<Rigidbody2D>();
        }

        rb.gravityScale = 0f;

    }

    void Start() {
        Destroy(this.gameObject, lifeTime);
    }

    void FixedUpdate() {

        rb.velocity = new Vector2(0f, -Screen.height / 3);

    }

    void OnTriggerEnter2D(Collider2D coll) { 
     
        if (coll.tag == "deathBox")  {
            Destroy(this.gameObject);
        }

    }
}
