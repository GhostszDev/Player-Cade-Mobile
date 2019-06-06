using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class spawnManager : MonoBehaviour {

	public GameObject sp1;
	public GameObject sp2;
	public GameObject sp3;
	public GameObject sp4;
	public GameObject sp5;
	public GameObject goldShipPreb;
    public GameObject fushiaShipPreb;
    public GameObject redShipPreb;
    public GameObject canvas;

	private GameObject ship;
	private GameObject sp;
    private GameObject deathBox;
    private GameObject deathBoxTop;
    private Transform trans;
	private int diceRoll;
    private int shipDiceRoll;
    public float timer;
    public float screenWidth;
    public float screenHeight;

	void spawn(){

		diceRoll = Random.Range (1, 6);

		switch (diceRoll) {
			case 1:
				trans = sp1.transform;
			break;

			case 2:
				trans = sp2.transform;
			break;

			case 3:
				trans = sp3.transform;
			break;
			
			case 4:
				trans = sp4.transform;
			break;

			case 5:
				trans = sp5.transform;
			break;

		}

        shipDiceRoll = Random.Range(1, 4);

        switch (shipDiceRoll) {

            case 1:
                ship = Instantiate(goldShipPreb, trans.position, Quaternion.identity, trans) as GameObject;
                break;

            case 2:
                ship = Instantiate(fushiaShipPreb, trans.position, Quaternion.identity, trans) as GameObject;
                break;

            case 3:
                ship = Instantiate(redShipPreb, trans.position, Quaternion.identity, trans) as GameObject;
                break;
        }
        
		timer = 0.9f;

	}

    void deathBoxPos(){

        deathBox.transform.localPosition = -(sp.transform.localPosition);
        deathBoxTop.transform.localPosition = sp.transform.localPosition;

    }

    void findObjs(){

        if (sp == null){
            sp = GameObject.FindGameObjectWithTag("SP");
        }

        if (deathBox == null){
            deathBox = GameObject.FindGameObjectWithTag("deathBox");
        }

        if (deathBoxTop == null) {
            deathBoxTop = GameObject.FindGameObjectWithTag("deathBoxTop");
        }

        if (sp1 == null){
            sp1 = GameObject.FindGameObjectWithTag("sp1");
        }

        if (sp2 == null){
            sp2 = GameObject.FindGameObjectWithTag("sp2");
        }

        if (sp3 == null) {
            sp3 = GameObject.FindGameObjectWithTag("sp3");
        }

        if (sp4 == null) {
            sp4 = GameObject.FindGameObjectWithTag("sp4");
        }

        if (sp5 == null){
            sp5 = GameObject.FindGameObjectWithTag("sp5");
        }

    }

    // Use this for initialization
    void Start() {

        timer = 0.9f;
        findObjs();
        deathBoxPos();
        canvas = GameObject.FindGameObjectWithTag("canvas");
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        
        sp1.transform.localPosition = new Vector3(-(screenWidth / 6) + 40, 0f, 0f);
        sp5.transform.localPosition = new Vector3((screenWidth / 6) - 40, 0f, 0f);
        sp3.transform.localPosition = new Vector3(0f, 0f, 0f);
        sp2.transform.localPosition = new Vector3(-(screenWidth / 10) + 40, 0f, 0f);
        sp4.transform.localPosition = new Vector3((screenWidth / 10) - 40, 0f, 0f);

    }

    // Update is called once per frame
    void Update() {

        if (timer <= 0) {
            timer = 0;
            spawn();
        }
        else {
            timer -= 1 * Time.deltaTime;
        }


    }

    void FixedUpdate() {



    }
}
