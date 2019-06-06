using UnityEngine;
using System.Collections;

public class fushiaShip : MonoBehaviour {

    public GameObject _manager;
    public GameObject player;
    public GameObject enemyBullet;
    public GameObject cannon;
    public Material m_hurt;
    public Transform rayStart;
    public Transform rayEnd;
    public float playerX;
    public float height;
    public bool fire;

    private Rigidbody2D rb;
    private int health;
    private float lifetime;
    public float shootTimer;
    private float speed;
    private scoreManager sc;
    private GameObject bullet;

    // Use this for initialization
    void Start() {

        health = 4;
        shootTimer = 0.0f;
        lifetime = Screen.height / 10;
        speed = Screen.height / 4;
        rb = GetComponent<Rigidbody2D>();
        rayStart = cannon.transform;
        height = -(Screen.height - 50);

        if (rb == null) {
            gameObject.AddComponent<Rigidbody2D>();
            rb = GetComponent<Rigidbody2D>();
        }

        if (_manager == null) {
            _manager = GameObject.FindGameObjectWithTag("manager");
        }

        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        rb.gravityScale = 0f;
        sc = _manager.GetComponent<scoreManager>();

    }

    void Update() {
        if (health == 0) {
            death();
            sc.addScore(400);
        }

        if (shootTimer > 0) {
            shootTimer -= 1 * Time.deltaTime;
        }
        trackPlayer();

        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void FixedUpdate() {

        rb.velocity = new Vector2(0f, -speed);

    }

    void trackPlayer() {

        rayCastToCheckForPlayer();

        if (shootTimer <= 0 && fire) {

            shoot();

        }

    }

    void rayCastToCheckForPlayer() {

        rayEnd.position = new Vector3(rayStart.position.x, height, 0);

        Debug.DrawLine(rayStart.position, rayEnd.position, Color.red);

        fire = Physics2D.Linecast(rayStart.position, rayEnd.position, 1 << LayerMask.NameToLayer("Player"));

    }

    void shoot() {

        Debug.Log("Enemy Shot!");
        bullet = Instantiate(enemyBullet, Vector3.zero, Quaternion.identity, cannon.transform) as GameObject;
        shootTimer = 1.2f;

    }

    void death() {

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

        if (coll.gameObject.tag == "deathBox" || coll.gameObject.tag == "player") {

            Debug.Log("death");
            death();

        }

    }
}
