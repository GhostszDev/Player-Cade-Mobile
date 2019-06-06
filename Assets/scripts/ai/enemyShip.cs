using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyShip : MonoBehaviour {

    public GameObject _manager;
    public GameObject canvas;
    public GameObject player;
    public GameObject enemyBullet;
    public GameObject cannon;
    public GameObject explosion;
    public Material m_hurt;
    public Transform rayStart;
    public Transform rayEnd;
    public float playerX;
    public float height;
    public bool fire;
    public string shipName;

    private GameObject bullet;
    private Rigidbody2D rb;
    public scoreManager sc;    
    private int health;
    private float score;
    private int ranBool;
    private float lifetime;
    private float shootTimer;
    private float speed;
    private bool leftTurn;
    private bool rightTurn;

    // Use this for initialization
    void Start () {

        shipCheckAndStart();
		
	}

    // Update is called once per frame
    void Update() {

        if (health == 0) {
            sc.addScore(score);
            death();
        }

        if (shootTimer > 0) {
            shootTimer -= 1 * Time.deltaTime;
        }

        switch (shipName) {
            case "fushiaShip(Clone)":
            case "fushiaShip":
                trackPlayer();
                break;
        }

        Destroy(gameObject, lifetime);

    }

    void FixedUpdate() {
        switch (shipName) {

            case "fushiaShip(Clone)":
            case "fushiaShip":
                fushiaShip();
                break;

            case "goldShip(Clone)":
            case "goldShip":
                goldShip();
                break;

            case "redShip(Clone)":
            case "redShip":
                redShip();
                break;

        }
    }

    void fushiaShip() {
        rb.velocity = new Vector2(0f, -speed);
    }

    void goldShip() {
        rb.velocity = new Vector2(0f, -speed);
    }

    void redShip() {

        if(leftTurn && !rightTurn) { 
            rb.velocity = new Vector2(speed, -speed);
        }

        if (!leftTurn && rightTurn) {
            rb.velocity = new Vector2(-speed, -speed);
        }
    }

    void shipCheckAndStart() {

        rb = gameObject.GetComponent<Rigidbody2D>();

        if (rb == null) {
            gameObject.AddComponent<Rigidbody2D>();
            rb = GetComponent<Rigidbody2D>();
        }

        if (_manager == null) {
            _manager = GameObject.FindGameObjectWithTag("manager");
        }

        if (canvas == null) {
            canvas = GameObject.FindGameObjectWithTag("canvas");
        }

        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        rb.gravityScale = 0f;
        sc = _manager.GetComponent<scoreManager>();

        shootTimer = 0.0f;
        lifetime = Screen.height / 10;
        height = -(500);

        shipName = gameObject.name;

        switch (shipName) {

            case "goldShip":
            case "goldShip(Clone)":
                health = 3;
                speed = Screen.height / 13.7f;
                score = 200.0f;
                break;

            case "fushiaShip(Clone)":
            case "fushiaShip":
                health = 4;
                speed = Screen.height / 14.0f;
                score = 400.0f;

                if (cannon == null) {
                    cannon = gameObject.transform.GetChild(0).gameObject;
                }

                if (rayEnd == null) {
                    rayEnd = gameObject.transform.GetChild(1).gameObject.transform;
                }

                rayStart = cannon.transform;

                break;

            case "redShip":
            case "redShip(Clone)":
                health = 3;
                speed = Screen.height / 14.3f;
                score = 600.0f;
                ranBool = Random.Range(0,2);

                if (ranBool == 1) {
                    leftTurn = true;
                    rightTurn = false;
                } else {
                    leftTurn = false;
                    rightTurn = true;
                }

                break;
        }

    }

    void trackPlayer() {

        rayCastToCheckForPlayer();

    }

    void rayCastToCheckForPlayer() {

        rayEnd.position = new Vector3(rayStart.position.x, height, 0);

        Debug.DrawLine(rayStart.position, rayEnd.position, Color.red);

        fire = Physics2D.Linecast(rayStart.position, rayEnd.position, 1 << LayerMask.NameToLayer("Player"));

    }

    void shoot() {

        bullet = Instantiate(enemyBullet, cannon.transform.position, Quaternion.identity, cannon.transform) as GameObject;
        shootTimer = 1.2f;

    }

    void death() {

        GameObject ex = Instantiate(explosion, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform.parent.gameObject.transform) as GameObject;
        Destroy(this.gameObject);
    }

    void knockBack() {

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

        if (coll.gameObject.tag == "deathBox") {

            //Debug.Log("death");
            death();

        }

        if (coll.gameObject.tag == "leftTurn") {
            leftTurn = true;
            rightTurn = false;
        }

        if (coll.gameObject.tag == "rightTurn") {
            leftTurn = false;
            rightTurn = true;
        }

    }

}
