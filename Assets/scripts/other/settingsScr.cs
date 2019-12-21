using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class settingsScr : MonoBehaviour {

    public GameObject canvas;
    public GameObject popup;
    public GameObject popUpInst;
    public GameObject signInObj;
    public GameObject signUpObj;
    public Text popUpTitle;
    public InputField userNameSignIn;
    public InputField passwordSignIn;
    public Button btnSignIn;
    public ghsUtility ghs;
    public bool signInBool;
    public bool signUpBool;
    public string[] signUpData;
    List<string> signInData = new List<string>();
    public ghsObj ghsObj;
    public Text userName;
    public Image userIcon;
    public Texture2D defaultIcon;

    public Text ghsCheck;
    public Text fbCheck;
    public Text googCheck;

    private string socialFail;
    private string socialSuccess;

    public void signInPopUp() {
        popUpObj();
        signUpBool = false;
        signInBool = true;
        signInObj.SetActive(true);
        signUpObj.SetActive(false);
        popUpTitle.text = "Login";

    }

    public void signUpPopUp() {
        popUpObj();
        signUpBool = true;
        signInBool = false;
        signInObj.SetActive(false);
        signUpObj.SetActive(true);
        popUpTitle.text = "Sign Up";
    }

    public void disableAll() {

        Destroy(popUpInst);
        signInBool = false;
        signUpBool = false;
    }

    public void signInFunc() {
        ghs.ghsSignIn(userNameSignIn.text, passwordSignIn.text);

        ghsObj = ghs.getData();

        if (ghsObj.success == "true") {
            disableAll();
            setData(ghsObj);
        }
        
    }

    public void clearGhsData() {

    }

    public void backToTitleScrn() {
        SceneManager.LoadScene("titleScrn");
    }

    public void getUserName(string s) {
        signInData[0] = s;
    }

    void popUpObj() {

        if (popUpInst == null) {
            popUpInst = Instantiate(popup, canvas.transform.position, Quaternion.identity, canvas.transform) as GameObject;
        }

        popUpTitle = popUpInst.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        signInObj = popUpInst.transform.GetChild(1).gameObject;
        signUpObj = popUpInst.transform.GetChild(2).gameObject;
        userNameSignIn = signInObj.transform.GetChild(0).GetComponent<InputField>();
        passwordSignIn = signInObj.transform.GetChild(1).GetComponent<InputField>();
        btnSignIn = signInObj.transform.GetChild(2).GetComponent<Button>();

        btnSignIn.onClick.RemoveAllListeners();
        btnSignIn.onClick.AddListener(delegate { signInFunc(); });

    }

    public void ghsBtn() {
        signInPopUp();
    }

    public void setData(ghsObj g) {
        if (g.userName == null) {
            userName.text = "UserName";
        } else {
            userName.text = g.userName;
        }
 
        if (g.user_icon == null) {
            Debug.Log("No userIcon available!");
        } else {
            getUserIcon(g.useBlob, g.user_icon);
        }

        checkSocialStatus(g);

    }

    public void getUserIcon(string useBlob, string blob) {

        if (useBlob == "true") {

            string base64Decoded;
            byte[] data = Convert.FromBase64String(blob);
            base64Decoded = System.Text.ASCIIEncoding.ASCII.GetString(data);
            string[] base64split = base64Decoded.Split(',');

            byte[] Bytes = System.Convert.FromBase64String(base64split[1]);
            Texture2D tex = new Texture2D(500, 700);
            tex.LoadImage(Bytes);
            ghs.saveImage(tex, "user_icon.png");
            Rect rect = new Rect(0, 0, tex.width, tex.height);
            userIcon.sprite = Sprite.Create(tex, rect, new Vector2(), 100f);

        } else {

            ghs.urlToBase64(blob);

        }

    }

    public void checkSocialStatus(ghsObj g) {

        if (g != null) {
            if (g.token.Length >= 2) {
                ghsCheck.text = socialSuccess;
            } else {
                ghsCheck.text = socialFail;
            }

            if (Social.localUser.authenticated) {
                googCheck.text = socialSuccess;
            } else {
                googCheck.text = socialSuccess;
            }
        }


    }

    public void showGLeaderBoard() { }

    public void showFBLeaderBoard() { }

    public void showGHSLeaderBoard() { }

    public void checkSocialFun() {
        checkSocialStatus(ghsObj);
    }

    // Use this for initialization
    void Start() {

        signInBool = false;
        signUpBool = false;
        socialFail = "";
        socialSuccess = "";

        ghsObj = ghs.getData();
        setData(ghsObj);
        ghsObj = ghs.getData();
        
        if (ghsObj != null) {
            checkSocialStatus(ghsObj);
        }

    }

    public void Update() {

        if (signInBool && !signUpBool) {
            signInPopUp();
        } else if (!signInBool && signUpBool) {
            signUpPopUp();
        } else if (!signUpBool && !signInBool) {
            disableAll();
        }

        InvokeRepeating("checkSocialFun", 300f, 0f);

    }
}
