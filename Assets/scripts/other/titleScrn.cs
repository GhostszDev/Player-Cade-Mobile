using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class titleScrn : MonoBehaviour {

    public void story(bool s){

		if (s) {
            //Application.LoadLevel("story");
            SceneManager.LoadScene("story");
        }
	}
	
	public void endless(bool e){

		if (e) {
            SceneManager.LoadScene("selectScreen");
		}

	}

	public void setting(bool set){

        if (set){
            SceneManager.LoadScene("settings");
        }

	}

    public void sound(bool snd){

        if (snd){
        }else{
        }

    }

    public void createDirs() {
        if (!Directory.Exists(Application.persistentDataPath + "/WorldDefer/")) {
            Directory.CreateDirectory(Application.persistentDataPath + "/WorldDefer/");
        }

        if (!Directory.Exists(Application.persistentDataPath + "/WorldDefer/image/")) {
            Directory.CreateDirectory(Application.persistentDataPath + "/WorldDefer/image/");
        }

        if (!Directory.Exists(Application.persistentDataPath + "/WorldDefer/temp/")) {
            Directory.CreateDirectory(Application.persistentDataPath + "/WorldDefer/temp/");
        }

        if (!Directory.Exists(Application.persistentDataPath + "/WorldDefer/saves/")) {
            Directory.CreateDirectory(Application.persistentDataPath + "/WorldDefer/saves/");
            ghsObj g = new ghsObj();
            ghsUtility.saveData(g);
        }
    }

    public void Start() {
        createDirs();
        ghsUtility.Instance.initGSignIn();
    }
}
