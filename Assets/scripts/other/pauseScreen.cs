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
	    if (ghs.userName != null){
	        userNamePS.text = ghs.userName;
	        userNameGOS.text = ghs.userName;
	    } else {
	        userNamePS.text = "username";
	        userNameGOS.text = "username";
	    }

	    if (rewardBtn != null) {
	        rewardBtn.interactable = !con.rewarded;
	    }

	}
    
    public void getUserIcon(string useBlob, string blob, Image userIcon) {

        if (useBlob == "true") {

            string base64Decoded;
            byte[] data = Convert.FromBase64String(blob);
            base64Decoded = System.Text.ASCIIEncoding.ASCII.GetString(data);
            string[] base64split = base64Decoded.Split(',');

            byte[] Bytes = System.Convert.FromBase64String(base64split[1]);
            Texture2D tex = new Texture2D(500, 700);
            tex.LoadImage(Bytes);
            Rect rect = new Rect(0, 0, tex.width, tex.height);
            userIcon.sprite = Sprite.Create(tex, rect, new Vector2(), 100f);

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
        ghs.score = score;
        ghsUtility.saveData(ghs);
        
        if (Social.localUser.authenticated) {
            gu.addScoreToLB(GPGSIds.leaderboard_world_defer_leaders, score);
        }

        quitGame();
        
    }

    public void ShowRewardedAd(){

        if (Advertisement.IsReady("rewardedVideo")){

            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void HandleShowResult(ShowResult result){

        switch (result){

            case ShowResult.Finished:

                Debug.Log("The ad was successfully shown.");
                con.liveCount(1);
                resumeBtn();
                con.rewarded = true;
                break;

            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;

            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

    public void quitGame() {
        SceneManager.LoadScene("titleScrn");
        if (Time.timeScale == 0f) {
            Time.timeScale = 1f;
        }
    }
}
