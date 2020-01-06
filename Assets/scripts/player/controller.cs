using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class controller : MonoBehaviour{

    public GameObject ship;
    public GameObject bullet;
    public GameObject cannon;
    public GameObject exposive;
    public GameObject preShip;
    public GameObject preBullet;
    public GameObject preExposve;
    public GameObject _shipEty;
    public GameObject[] touchControls;
    public GameObject _manager;
    public GameObject pauseMenu;
    public GameObject prePM;
    public GameObject gameOverScrn;
    public GameObject pauseScrn;
    public Text livesTXT;
    public float speed;
    public float hInput;
    public bool gamepad = false;
    public Material m_hurt;
    public string[] names;
    public string button;
    public checks check;
    public ghsUtility gu;
    public ghsObj ghs;
    public temp tp;
    public BoxCollider2D bc2D;
    public bool rewarded;

    private RaycastHit hitObj;
#if UNITY_EDITOR
    public float lives;
    #else
    private float lives;
#endif
    private Rigidbody2D rb;
    public float vTimer;
    private float maxVTimer = 3.0f;
    private bool firstSpawn;

    void Start(){

        lives = 3;
        rewarded = false;
        firstSpawn = true;

        /*----------------------------------------------------------
         * if ship doesnt have a Rigidbody
         * then it will be added and added to the rb
         * var and if it have one then just add it to the
         * rb var
         *--------------------------------------------------------*/
        if (_shipEty.GetComponent<Rigidbody2D>() == null){
            gameObject.AddComponent<Rigidbody2D>();
            rb = _shipEty.GetComponent<Rigidbody2D>();
        }else {
            rb = _shipEty.GetComponent<Rigidbody2D>();
        }

        if (gu == null) {
            gu = _manager.GetComponent<ghsUtility>();
        }

        if (bc2D == null) {
            bc2D = _shipEty.GetComponent<BoxCollider2D>();
        }

        rb.gravityScale = 0.0f;
    }

    void Update(){

        checkForGamePads();

        if (lives > 0 && preShip == null && preExposve == null){
            spawnPlayer();
        } else if (lives <= 0) {
            lives = 0;
            gameOverScreen();
        }

        livesTXT.text = "X" + Mathf.FloorToInt(lives).ToString();

        if (vTimer != 0){
            vTimer -= 1 * Time.deltaTime;
        }

        if (vTimer <= 0){
            vTimer = 0;
            bc2D.enabled = true;
        }

    }

    public void move(float m){

        if (preShip != null) {
            speed = Screen.width / 14 * m;
            rb.velocity = new Vector2(speed, 0f);
        }

    }

    public void fireBtn(){

        if (preShip != null) {
            preBullet = Instantiate(bullet, cannon.transform.position, Quaternion.identity, cannon.transform);
        }

    }

    public void liveCount(int l){

        lives += l;

    }

    string loadTempData() {
        string s;

        tp = gu.getTempData();
        s = tp.ship;

        return s;

    } 

    void spawnPlayer(){

        string sn = loadTempData();
        vulnerabilityState();

        if (firstSpawn != true) {
            lives -= 1;
        }

        switch (sn) {

            case "purpleShip":
            default:
                if (rb == null) {
                    this.gameObject.AddComponent<Rigidbody2D>();
                    rb = this.gameObject.GetComponent<Rigidbody2D>();
                    rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                }
                preShip = Instantiate(ship, _shipEty.transform.position, Quaternion.identity, _shipEty.transform) as GameObject;

                if (cannon == null) {
                    cannon = preShip.transform.GetChild(0).gameObject;
                }

                break;

        }

        firstSpawn = false;

    }

    public void pauseGame() {

        if (prePM == null){
            prePM = Instantiate(pauseMenu, _manager.transform.position, Quaternion.identity, _manager.transform);
            prePM.transform.localScale = new Vector3(1f, 1f, 1f);
            pauseScrn = prePM.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
            pauseScrn.SetActive(true);
            Time.timeScale = 0f;
        }

    }

    void vulnerabilityState() {
        vTimer = maxVTimer;
        bc2D.enabled = false;
    }

    void death() {

        if (vTimer == 0){
            Destroy(preShip);
//            Destroy(rb);
            preExposve = Instantiate(exposive, _shipEty.transform.position, Quaternion.identity, _shipEty.transform);
        }
    }

    void gameOverScreen(){
        
        if (prePM == null){
            prePM = Instantiate(pauseMenu, _manager.transform.position, Quaternion.identity, _manager.transform);
            prePM.transform.localScale = new Vector3(1f, 1f, 1f);
            gameOverScrn = prePM.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            gameOverScrn.SetActive(true);
            Time.timeScale = 0f;
        }
        
    }

    void checkForGamePads() {

#if UNITY_STANDALONE

        move(Input.GetAxis("Horizontal"));

#elif UNITY_ANDROID || UNITY_IOS

        names = Input.GetJoystickNames();
        //if (names.Length > 0) {
        // gamepad = true;
        // } else {
        //gamepad = false;
        //}

        gamepad = check.isTVMode();

        if (gamepad) {
            move(Input.GetAxis("Horizontal"));

            if (Input.GetButtonDown("Fire1")) {
                fireBtn();
            }

            for (int i = 0; i < touchControls.Length; i++) {
                touchControls[i].SetActive(false);
            }

        } else {

            for (int i = 0; i < touchControls.Length; i++) {
                touchControls[i].SetActive(true);
            }

        }


#endif

    }

    void OnTriggerEnter2D(Collider2D coll) {

    if (coll.gameObject.tag == "bullet") {
    death();
    }

    if (coll.gameObject.tag == "enemy") {
    death();
    }

    }
}
