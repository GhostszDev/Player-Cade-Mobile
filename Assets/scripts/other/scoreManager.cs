using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scoreManager : MonoBehaviour {

	public Text scoreTXT;
    public controller con;

	private float score;

	// Use this for initialization
	void Start () {

		score = 0;
        con = GameObject.FindGameObjectWithTag("Player").GetComponent<controller>();
	
	}
	
	// Update is called once per frame
	void Update () {

		addScore (2 * Time.deltaTime);
        scoreTXT.text = Mathf.FloorToInt(score).ToString();
        //scoreTXT.text = con.gamepad.ToString();
	
	}

	public void addScore(float s){

		score += s;

	}
}
