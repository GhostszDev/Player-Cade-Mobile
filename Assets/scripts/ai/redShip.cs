using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redShip : MonoBehaviour {

    public GameObject _manager;
    public Material m_hurt;

    private Rigidbody2D rb;
    private int health;
    private float lifetime;
    private float speed;
    private scoreManager sc;

    // Use this for initialization
    void Start() {

        health = 3;
        lifetime = Screen.height / 10;
        speed = Screen.height / 4;
        rb = GetComponent<Rigidbody2D>();

        if (rb == null) {
            gameObject.AddComponent<Rigidbody2D>();
            rb = GetComponent<Rigidbody2D>();
        }

        if (_manager == null) {
            _manager = GameObject.FindGameObjectWithTag("manager");
        }

        rb.gravityScale = 0f;
        sc = _manager.GetComponent<scoreManager>();

    }

    void Update() {
        if (health == 0) {
            death();
            sc.addScore(200);
        }

        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void FixedUpdate() {

        rb.velocity = new Vector2(0f, -speed);

    }

    void death() {

        Destroy(this.gameObject);
    }

    void knockBack()  {

        rb.velocity = new Vector2(0f, rb.transform.position.y + 5f);

    }

    public void hurt(int hurt) {
        health -= hurt;
        knockBack();

    }

    void OnTriggerEnter2D(Collider2D coll) {

        if (coll.gameObject.tag == "bullet") {

            hurt(1);

        }

        if (coll.gameObject.tag == "deathBox" || coll.gameObject.tag == "player") {

            Debug.Log("death");
            death();

        }

    }
}
