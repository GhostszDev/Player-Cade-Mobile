using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class selectScreen : MonoBehaviour {

    public int shipNum;
    public int listCount;
    public string sn;
    public string[] name;
    public List<Ships> prevList;
    public RawImage shipIcon;
    public Texture2D purpleShipLocked;
    public Texture2D purpleShipUnLocked;
    public Text shipName;
    public Text shipDesc;
    public static selectScreen Instance { get; private set; }
    public temp temp;
    public ghsUtility gu;

    void shipsList() {

        List<Ships> ship = new List<Ships>();
        ship.Add(new Ships("purpleShip", "", "This is the ship that you built with your own hands to aide the army to defeat the incoming threat.", "", false));

        if (File.Exists(Application.persistentDataPath + "/WorldDefer/temp/shipList.ghs") == false) {
            saveShips(ship);
            Debug.Log("Saved");
        }

        getShips();

        if (prevList != ship) {

            saveShips(ship);
            getShips();

        }

    }

    void getShips() {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/WorldDefer/temp/shipList.ghs", FileMode.Open);
        prevList = bf.Deserialize(stream) as List<Ships>;
        stream.Close();
        listCount = prevList.Count - 1;

    }

    void saveShips(List<Ships> s) {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/WorldDefer/temp/shipList.ghs", FileMode.Create);
        bf.Serialize(stream, s);
        stream.Close();

        Debug.Log("Ship List saved!");

    }

    void selectShip(int s) {

        sn = prevList[s].shipName;
        gu.addTmpShip(sn);


    }

    void displayShip(int s) {

        name = Regex.Split(prevList[s].shipName, "Ship");

        if (prevList[s].isLocked == true) {

            switch (prevList[s].shipName) {
                case "purpleShip":
                default:
                    shipName.text = name[0] + " Ship";
                    shipDesc.text = prevList[s].lockedDesc;
                    shipIcon.texture = purpleShipLocked;
                    break;
            }

        } else {

            switch (prevList[s].shipName) {
                case "purpleShip":
                default:
                    shipName.text = name[0] + " Ship";
                    shipDesc.text = prevList[s].desc;
                    shipIcon.texture = purpleShipUnLocked;
                    break;
            }

        }

    }

    public void leftBtn() {

        if (shipNum > 0) {
            shipNum -= 1;
            displayShip(shipNum);

        } else if (shipNum <= 0) {
            shipNum = 0;
            displayShip(shipNum);
        }

    }

    public void rightBtn() {

        if (shipNum < listCount) {
            shipNum += 1;
            displayShip(shipNum);
        }

    }

    public void selectBtn() {

        if (!prevList[shipNum].isLocked) {
            selectShip(shipNum);
            SceneManager.LoadScene("endless");
        }
    }


	// Use this for initialization
	void Start () {
        shipNum = 0;
        shipsList();
        displayShip(0);
        gu = GetComponent<ghsUtility>();

    }
}