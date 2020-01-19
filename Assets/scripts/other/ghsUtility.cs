using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

[System.Serializable]
public class ghsObj{

    public string success;
    public string token;
    public string name;
    public string userName;
    public string password;
    public string google;
    public string facebook;
    public string user_icon;
    public int score;
    public string useBlob;
    public string error_message;

    //google play services stuff

}

[System.Serializable]
public class Ships {

    public string shipName;
    public string unlocked;
    public string desc;
    public string lockedDesc;
    public bool isLocked;

    public Ships(string newName, string isUnlocked, string newDesc, 
        string newLockedDesc, bool newIsLocked) {
        shipName = newName;
        unlocked = isUnlocked;
        desc = newDesc;
        lockedDesc = newLockedDesc;
        isLocked = newIsLocked;

    }

}

[System.Serializable]
public class gameSettings {

    public int music;
    public int sndEfx;

}

[System.Serializable]
public class temp {

    public string ship;

}

public class ghsUtility : MonoBehaviour {

    public static ghsUtility Instance { get; private set; }
    public ghsObj ghs;
    public temp tmp;
    public gameSettings gs;
    public string site;
    public string dataPath;
    public string userInfo;
    public string info;
    private settingsScr settings;
    public Scene scene;
    public string sceneName;

    #region Achievements
    public static void unlockAchievement(string id) {

        Social.ReportProgress(id, 100, success => { });

    }

    public static void incrementAchievement(string id, int stepsToIncrement) {

        PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, success => { });

    }

    public void showAchievementsUI() {

        Social.ShowAchievementsUI();

    }
    #endregion

    #region LeaderBoard

    public void addScoreToLB(string leaderBoard, long score)
    {

        PlayGamesPlatform.Activate();
        if (Social.localUser.authenticated) {
            Social.ReportScore(score, leaderBoard, (bool success) => {
                if (success) {
                    Debug.Log("Score added to leaderboard!");
                } else {
                   Debug.LogError("Not able to add score to leaderboard!");
                }
            });
        } else {
            Debug.LogError("This user's score can't be posted!");
        }

    }

    public void showLB() {

        Social.ShowLeaderboardUI();

    }

    #endregion

    #region Google

    public void googleSignIn()
    {

        if (Social.localUser.authenticated) {
            PlayGamesPlatform.Instance.SignOut();
        } else {
            PlayGamesPlatform.Instance.Authenticate((bool success) => {
                if (success) {
                    Debug.Log("Signed In");
                    ghs = loadSaveData();
                    Debug.Log("Google Username: " + Social.localUser.userName);
                    unlockAchievement(GPGSIds.achievement_g_membership);
                    saveData(ghs);
                } else {
                  Debug.LogError("Failed to sign in!");  
                }
            });
            
        }
        
    }

    #endregion

    #region facebook
    #endregion

    public void ghsSignIn(string userName, string password) {
        string url = site;
        url += "login";

        WWWForm form = new WWWForm();

        Dictionary<string, string> headers = form.headers;

        form.AddField("user_login", userName);
        form.AddField("user_password", password);
        
        StartCoroutine(getData(url, form, true));

    }

    public void ghsSignUp() { }

    public static void saveData(ghsObj g) {

        BinaryFormatter bf = new BinaryFormatter();
        Stream stream = File.Open(Application.persistentDataPath + "/WorldDefer/saves/save.sav", FileMode.OpenOrCreate);
        bf.Serialize(stream, g);
        stream.Close();

        Debug.Log("Game has saved!");
    }

    public ghsObj loadSaveData() {
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/WorldDefer/saves/save.sav", FileMode.Open);
        ghs = bf.Deserialize(stream) as ghsObj;
        Debug.Log("Found the save file!");
        stream.Close();

        Debug.Log("Save File Loaded :" + ghs);
        return ghs;
    }

    public ghsObj getData() {

        ghs = loadSaveData();

        return ghs;
    }

    public void setTempData(temp t) {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/WorldDefer/temp/temp.tmp", FileMode.Create);
        bf.Serialize(stream, t);
        stream.Close();

    }

    public temp getTempData() {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/WorldDefer/temp/temp.tmp", FileMode.Open);
        tmp = bf.Deserialize(stream) as temp;
        stream.Close();

        return tmp;

    }

    public void deleteTempFile() {

    }

    public void addTmpShip(string s) {

        tmp.ship = s;
        setTempData(tmp);

    }

    public void saveImage(Texture2D texture, string fileName) {
        var bytes = texture.EncodeToPNG();
        FileStream file = new FileStream(Application.persistentDataPath + "/WorldDefer/image/" + fileName, FileMode.Create);
        var binary = new BinaryWriter(file);
        binary.Write(bytes);
        file.Close();
    }

    public void checkImageForDifferences() {

        ghs = loadSaveData();
        string prevIcon = ghs.user_icon;
        StartCoroutine(Application.persistentDataPath + "/WorldDefer/image/user_icon.png");

        if (prevIcon != ghs.user_icon) {
            ghs.useBlob = "true";
            saveData(ghs);

            string base64Decoded;
            byte[] data = System.Convert.FromBase64String(ghs.user_icon);
            base64Decoded = System.Text.ASCIIEncoding.ASCII.GetString(data);
            string[] base64split = base64Decoded.Split(',');

            byte[] Bytes = System.Convert.FromBase64String(base64split[1]);
            Texture2D tex = new Texture2D(500, 700);
            tex.LoadImage(Bytes);
            saveImage(tex, "user_icon.png");

        } else {

            Debug.Log("User Icon is the same!");
        }

    }

    public void urlToBase64(string uri) {
        StartCoroutine(getUserIcon(uri));
    }

    public IEnumerator ghsSendScore(int score)
    {

        string url = site;
        url += "addScore";
        
        WWWForm form = new WWWForm();
        form.AddField("Game", "WorldDefer");
        form.AddField("score", score);
        
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }

    }

    IEnumerator getData(string url, WWWForm form, bool postType) {
        if (postType)
        {
            ghsObj oldGhsObj = ghs;

            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();
 
            if(www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            }
            else {
                Debug.Log(www.downloadHandler.text);
                ghs = JsonUtility.FromJson<ghsObj>(www.downloadHandler.text);
                if (oldGhsObj.score > 0)
                {
                    ghs.score = oldGhsObj.score;
                }

                if (settings)
                {
                    ghs.userName = settings.userNameSignIn.text;
                    ghs.password = settings.passwordSignIn.text;
                    settings.setData(ghs);
                }
                saveData(ghs);
            }

        } else {
            
        }
    }

    IEnumerator getUserIcon(string uri) {
        WWW www = new WWW(uri);
        while (!www.isDone) {
            yield return null;
        }

        byte[] userIcon = www.texture.EncodeToPNG();
        ghs.user_icon = System.Convert.ToBase64String(userIcon);
        ghs.useBlob = "true";
        saveData(ghs);
    }

    void Start() {
        PlayGamesPlatform.Activate();
        site = "https://ghostszmusic.com/api/ghs-api/v1/"; 
        ghs = getData();
        dataPath = Application.persistentDataPath + "/WorldDefer/saves/save.sav";
        Instance = this;
        scene = SceneManager.GetActiveScene();
        sceneName = scene.name;

        if (sceneName == "Settings") {
            settings = GetComponent<settingsScr>();
        }

    }

}