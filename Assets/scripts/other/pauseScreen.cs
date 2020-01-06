using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseScreen : MonoBehaviour {

    public GameObject confirmPanel;
    public GameObject preCP;
    public GameObject _manager;
    public Text preHS;
    public Text preGOHS;
    public Image userIconPS;
    public Image userIconGOS;
    public Text userNamePS;
    public Text userNameGOS;
    public Button rewardBtn;
    public ghsUtility gu;
    public ghsObj ghs;
    public controller con;
    public scoreManager sm;

	// Use this for initialization
	void Start () {
	    
	    if (_manager == null) {
	        _manager = GameObject.FindGameObjectWithTag("manager");
	    }
	    
	    if (sm == null) {
	        sm = _manager.GetComponent<scoreManager>();
	    }

	    if (con == null) {
	        con = GameObject.FindGameObjectWithTag("Player").GetComponent<controller>();
	    }
	    
        gu = this.gameObject.transform.parent.gameObject.GetComponent<ghsUtility>();
        ghs = gu.getData();
        preHS.text = sm.scoreTXT.text;
        preGOHS.text = sm.scoreTXT.text;
	    getUserIcon(ghs.useBlob, ghs.user_icon, userIconPS);
	    getUserIcon(ghs.useBlob, ghs.user_icon, userIconGOS);
	    
	    if (ghs.name != null){
	        userNamePS.text = ghs.name;
	        userNameGOS.text = ghs.name;
	    } else {
	        userNamePS.text = "username";
	        userNameGOS.text = "username";
	    }

	}
    
    public void getUserIcon(string useBlob, string blob, Image userIcon) {

        if (useBlob == "true") {

	        Texture2D txt2d;
	        byte[] bytes = System.Convert.FromBase64String(blob);
	        txt2d = new Texture2D(1,1);
	        txt2d.LoadImage( bytes);
	        userIcon.sprite = Sprite.Create(txt2d, new Rect(0.0f, 0.0f, txt2d.width, txt2d.height), new Vector2(0.5f, 0.5f));

        } else {

//            ghs.urlToBase64(blob);

        }

    }

    public void resumeBtn() {

        if (Time.timeScale == 0f) {
            Time.timeScale = 1f;
        }

        Destroy(this.gameObject);
    }

    public void quitBtn() {

        if (preCP == null){
            preCP = Instantiate(confirmPanel, this.gameObject.transform.position, Quaternion.identity,
                this.gameObject.transform);
        }

    }

    public void closeConfirm() {

        Destroy(preCP);

    }

    public void gameOverQuit() {
	    
	    int score = int.Parse(sm.scoreTXT.text);
	    ghs = gu.getData();
        ghs.score = score;
        ghsUtility.saveData(ghs);
        
        Debug.Log("Score: " + score);
	    gu.addScoreToLB(GPGSIds.leaderboard_world_defenders, score);

        quitGame();
        
    }

    public void quitGame() {
        SceneManager.LoadScene("titleScrn");
        if (Time.timeScale == 0f) {
            Time.timeScale = 1f;
        }
    }
}
