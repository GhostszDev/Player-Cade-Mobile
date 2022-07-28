using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

[System.Serializable]
public class ship
{
    public string shipName;
    public Sprite shipImage;
    public int[] cannons;
    public float speed;
    public int level;
    public Vector3 shipRectTransform;
    public bool unlocked;
    public string desc;
    public string unlockDesc;
    public Vector2 offset;
    public Vector2 size;

    public ship(string name, Sprite img, int[] cannonsNum, float spd, int lvl, Vector3 shipRectTrans, string des,
        string unlockDes, bool unlock, Vector2 _offset, Vector2 _size)
    {
        shipName = name;
        shipImage = img;
        cannons = new int[cannonsNum.Length];
        int i = 0;
        foreach (var num in cannonsNum)
        {
            cannons[i] = num;
            i++;
        }

        speed = spd;
        level = lvl;
        shipRectTransform = shipRectTrans;
        desc = des;
        unlockDesc = unlockDes;
        unlocked = unlock;
        offset = _offset;
        size = _size;
    }
}

[System.Serializable]
public class tempFile
{
    public string selectedMode;
    public int selectedShip;
}

public class manager : MonoBehaviour
{
    private static manager _instance;
    private int totalShips = 2, currentPOS = 0;
    private GHS_Utility _ghsUtility;
    private String storyMode = "Story", endlessMode = "Endless", settingMode = "", disabledBtnText = "";
    private Color btnNormalColor, btnDisableColor;
    private string currentScene, tempFile;
    private GameObject leftTurn, rightTurn, deathBoxTop, deathBox;
    public string purpleShiSprite;

    public Font fontAwesome, fontAwesomeBrands, fontAwesomeSolid, pressStartFont;
    public Sprite buttonBg;
    public ship[] ship;
    public GameObject storyBtn, endlessBtn, settingsBtn, buttonGroup, controller;
    public Button shootBtn, leftBtn, rightBtn;
    public Vector2 move;
    
    //SelectScreen
    private TextMeshProUGUI selectScreenName, selectScreenDesc;
    private Image selectScreenShipImage;

    private PlayerInputActions ghsInputs;

    public static manager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void shipList()
    {
        ship = new ship[totalShips];
        ship[0] = new ship("Purple Ship", Resources.Load<Sprite>("sprites/shipShooter/player/playerShip"), new int[]{1,2,3} , 1, 1, new Vector3(3,3,0), "A standard ship given to every pilot.", "", true, new Vector2(-0.423172f,15.65697f), new Vector2(149.0197f,81.31393f));
        ship[1] = new ship("Ducky Ship", Resources.Load<Sprite>("sprites/shipShooter/player/Ducky_Ship"), new int[]{1,2,3} , 3, 1, new Vector3(2,2,0), "Strange Ship made of rubber", "", true, new Vector2(-0.4231415f,38.08452f), new Vector2(201.1553f,75.8995f));
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        ghsInputs = new PlayerInputActions();
        ghsInputs.GamePlay.Enable();
        
        shipList();
    }

    private void OnEnable()
    {
        // ghsInputs.GamePlay.Fire.performed += _actionBtn();
    }

    private bool IsPortrait()
    {
        if (Screen.height > Screen.width)
        {
            return true;
        }
        return false;
    }

    public bool isDemo()
    {

        if (currentScene == "titleScrn")
        {
            return true;
        }

        return false;
    }

    private void SelectedMode(String nextScene)
    {
        if (nextScene != "settings")
        {
            PlayerPrefs.SetString("GameMode", nextScene);
            SceneManager.LoadScene("selectScreen");
        }
        else
        {
            SceneManager.LoadScene(nextScene);
        }

    }

    public void SelectedShip(int pos)
    {
        PlayerPrefs.SetInt("shipPOS", pos);
        SceneManager.LoadScene(PlayerPrefs.GetString("GameMode"));
        
    }

    public void SaveGame()
    {
        
    }

    public void LoadGame()
    {
        
    }

    public void GotoScene(string sceneName)
    {
        if (sceneName != null)
        {
            SelectedMode(sceneName);
        }
    }

    public void SelectShip(int selectShip)
    {
        SelectedShip(selectShip);
    }

    public ship GetSelectedShip(int pos = 0)
    {
        if (isDemo())
        {
            return ship[pos];
        }
        else
        {
            return ship[PlayerPrefs.GetInt("shipPOS")];
        }

        return null;
    }

    void AddTitleScreenBtns()
    {
        if (!endlessBtn)
        {
            // Nav button group
            buttonGroup = new GameObject("buttonGroup");
            buttonGroup.transform.SetParent(this.transform.parent);
            buttonGroup.transform.localPosition = Vector3.zero;

            // Story Mode Button
            storyBtn = DefaultControls.CreateButton(new DefaultControls.Resources());
            storyBtn.name = "storyBtn";
            Button storyBtnButton = storyBtn.GetComponent<Button>();
            storyBtnButton.onClick.AddListener(delegate {GotoScene("story"); });
            storyBtnButton.interactable = false;
            ColorBlock storyBtnColorBlock = storyBtnButton.colors;
            storyBtnColorBlock.normalColor = btnNormalColor;
            storyBtnColorBlock.disabledColor = btnDisableColor;
            RectTransform storyBtnRect = storyBtn.GetComponent<RectTransform>();
            Image storyBtnImage = storyBtn.GetComponent<Image>();
            
            if (IsPortrait())
            {
                storyBtnRect.sizeDelta = new Vector2(Screen.width / 2f, Screen.height / 15);
            }
            else
            {
                storyBtnRect.sizeDelta = new Vector2(Screen.width / 2f, Screen.height / 6f);
            }

            storyBtn.transform.SetParent(buttonGroup.transform);
            if (IsPortrait())
            {
                storyBtn.transform.localPosition = new Vector3(0, -(15f + storyBtnRect.rect.height), 0);
            }
            else
            {
                storyBtn.transform.localPosition = new Vector3(0, -(15f + storyBtnRect.rect.height / 2), 0);
            }

            Text storyText = storyBtn.transform.GetChild(0).GetComponent<Text>();
            storyText.fontSize = Screen.width / 15;
            storyText.color = Color.white;
            storyText.font = pressStartFont;
            storyBtnImage.sprite = buttonBg;
            storyText.text = storyMode;
            
            Vector3 storyBtnPos = storyBtn.transform.localPosition;
            
            // Endless Mode button
            endlessBtn = DefaultControls.CreateButton(new DefaultControls.Resources());
            endlessBtn.name = "endlessBtn";
            Button endlessBtnButton = endlessBtn.GetComponent<Button>();
            endlessBtnButton.onClick.AddListener(delegate {GotoScene("endless"); });
            ColorBlock endlessBtnColorBlock = endlessBtnButton.colors;
            endlessBtnColorBlock.normalColor = btnNormalColor;
            endlessBtnColorBlock.disabledColor = btnDisableColor;
            RectTransform endlessBtnRect = endlessBtn.GetComponent<RectTransform>();
            Image endlessBtnImage = endlessBtn.GetComponent<Image>();
            endlessBtnImage.sprite = buttonBg;
            if (IsPortrait())
            {
                endlessBtnRect.sizeDelta = new Vector2(Screen.width / 2f, Screen.height / 15);
            }
            else
            {
                endlessBtnRect.sizeDelta = new Vector2(Screen.width / 2f, Screen.height / 6);
            }

            endlessBtn.transform.SetParent(this.transform.parent);
            endlessBtn.transform.localPosition = new Vector3(0,
                (storyBtnPos.y - endlessBtnRect.rect.height) - Screen.height /20, 0);
            
            Text endlessText = endlessBtn.transform.GetChild(0).GetComponent<Text>();
            endlessText.font = pressStartFont;
            endlessText.fontSize = Screen.width / 16;
            endlessText.text = endlessMode;
            endlessText.color = Color.white;

            Vector3 endlessBtnPos = endlessBtn.transform.localPosition;
            
            // Settings Button
            settingsBtn = DefaultControls.CreateButton(new DefaultControls.Resources());
            settingsBtn.name = "settingsBtn";
            Button settingBtnButton = settingsBtn.GetComponent<Button>();
            settingBtnButton.onClick.AddListener(delegate {GotoScene("settings"); });
            ColorBlock settingBtnColorBlock = settingBtnButton.colors;
            settingBtnColorBlock.normalColor = btnNormalColor;
            settingBtnColorBlock.disabledColor = btnDisableColor;
            RectTransform settingBtnRect = settingsBtn.GetComponent<RectTransform>();
            Image settingBtnImage = settingsBtn.GetComponent<Image>();
            settingBtnImage.sprite = buttonBg;
            if (IsPortrait())
            {
                settingBtnRect.sizeDelta = new Vector2(Screen.height / 15, Screen.height / 15);
            }
            else
            {
                settingBtnRect.sizeDelta = new Vector2(Screen.height / 8, Screen.height / 8);
            }

            settingsBtn.transform.SetParent(this.transform.parent);
            settingsBtn.transform.localPosition = new Vector3(Screen.width/2 - settingBtnRect.rect.width, -Screen.height/2 + settingBtnRect.rect.height, 0);

            Text settingText = settingsBtn.transform.GetChild(0).GetComponent<Text>();
            settingText.font = fontAwesomeSolid;
            if (IsPortrait())
            {
                settingText.fontSize = Screen.width / 15;
            }
            else
            {
                settingText.fontSize = Screen.width / 25;
            }

            settingText.text = settingMode;
            settingText.color = Color.white;
            
            // Set Color Blocks to buttons
            storyBtnButton.colors = storyBtnColorBlock;
            endlessBtnButton.colors = endlessBtnColorBlock;
            settingBtnButton.colors = settingBtnColorBlock;
            
            // Group all created buttons
            storyBtn.transform.SetParent(buttonGroup.transform);
            endlessBtn.transform.SetParent(buttonGroup.transform);
            settingsBtn.transform.SetParent(buttonGroup.transform);
        }
    }

    void AddOnScreenControls()
    {
        if (!controller)
        {
            controller = GameObject.FindWithTag("GameController");
        }
        
        leftBtn = controller.transform.GetChild(0).gameObject.GetComponent<Button>();
        rightBtn = controller.transform.GetChild(1).gameObject.GetComponent<Button>();
        shootBtn = controller.transform.GetChild(2).gameObject.GetComponent<Button>();

    }
    
    void AddTiggers()
    {
        if (GameObject.FindWithTag("leftTurn"))
        {
            leftTurn = GameObject.FindWithTag("leftTurn");
        }
        else
        {
            leftTurn = new GameObject("leftTurn");
        }
        
        if (GameObject.FindWithTag("rightTurn"))
        {
            rightTurn = GameObject.FindWithTag("rightTurn");
        }
        else
        {
            rightTurn = new GameObject("rightTurn");
        }

        if (GameObject.FindWithTag("deathBoxTop"))
        {
            deathBoxTop = GameObject.FindWithTag("deathBoxTop");
        }
        else
        {
            deathBoxTop = new GameObject("deathBoxTop");
        }
        
        if (GameObject.FindWithTag("deathBox"))
        {
            deathBox = GameObject.FindWithTag("deathBox");
        }
        else
        {
            deathBox = new GameObject("deathBox");
        }

        leftTurn.tag = "leftTurn";
        leftTurn.transform.SetParent(Camera.main.transform.GetChild(0).transform);
        leftTurn.transform.localScale = new Vector3(50f, 5000f, 0);
        float leftTurnHalfWidth = leftTurn.transform.localScale.x / 2;
        leftTurn.transform.localPosition = new Vector3(-(Screen.width / 2)+leftTurnHalfWidth, -(Screen.height / 2), 0f);
        leftTurn.AddComponent<BoxCollider2D>().isTrigger = isDemo();

        rightTurn.tag = "rightTurn";
        rightTurn.transform.SetParent(Camera.main.transform.GetChild(0).transform);
        rightTurn.transform.localScale = new Vector3(50f, 5000f, 0);
        float rightTurnHalfWidth = rightTurn.transform.localScale.x / 2;
        rightTurn.transform.localPosition = new Vector3((Screen.width / 2)-rightTurnHalfWidth, -(Screen.height / 2), 0f);
        rightTurn.AddComponent<BoxCollider2D>().isTrigger = isDemo();

        deathBoxTop.tag = "deathBoxTop";
        deathBoxTop.transform.SetParent(Camera.main.transform.GetChild(0).transform);
        deathBoxTop.transform.localScale = new Vector3(5000f, 50f, 0);
        float deathBoxTopHeight = deathBoxTop.transform.localScale.y * 4;
        deathBoxTop.transform.localPosition = new Vector3(0, Screen.height/2 + deathBoxTopHeight, 0);
        deathBoxTop.AddComponent<BoxCollider2D>().isTrigger = true;
        
        deathBox.tag = "deathBox";
        deathBox.transform.SetParent(Camera.main.transform.GetChild(0).transform);
        deathBox.transform.localScale = new Vector3(5000f, 50f, 0);
        float deathBoxHeight = deathBox.transform.localScale.y * 4;
        deathBox.transform.localPosition = new Vector3(0, -(Screen.height / 2 + deathBoxHeight), 0);
        deathBox.AddComponent<BoxCollider2D>().isTrigger = true;
    }

    void fetchAll()
    {
        if (currentScene == "selectScreen") 
        {
            selectScreenName = GameObject.FindWithTag("selectScreen").transform.GetChild(0).gameObject.transform.GetChild(0).gameObject
                .GetComponent<TextMeshProUGUI>();
            selectScreenDesc = GameObject.FindWithTag("selectScreen").transform.GetChild(0).gameObject.transform.GetChild(1).gameObject
                .GetComponent<TextMeshProUGUI>();
            selectScreenShipImage = GameObject.FindWithTag("selectScreen").transform.GetChild(1).gameObject.GetComponent<Image>();
        }
    }
    
    void setShip(int pos)
    {
        selectScreenName.text = ship[pos].shipName;
        selectScreenShipImage.sprite = ship[pos].shipImage;
        if (ship[pos].unlocked)
        {
            selectScreenDesc.text = ship[pos].desc;
        }
        else
        {
            selectScreenDesc.text = ship[pos].unlockDesc;
        }
    }

    void _actionBtn()
    {
        SelectedShip(currentPOS);
    }
    
    void Update()
    {

        if (currentScene == "selectScreen")
        {

            if (currentPOS >= totalShips)
            {
                rightBtn.interactable = false;
            }
            else
            {
                rightBtn.interactable = true;
            }

            if (currentPOS == 0)
            {
                currentPOS = 0;
                leftBtn.interactable = false;
            }
            else
            {
                leftBtn.interactable = true;
            }
        }

    }

    public void Start()
    {
        _ghsUtility = GHS_Utility.Instance;
        currentScene = SceneManager.GetActiveScene().name;
        btnDisableColor = new Color(.4f, .4f, .4f, 0.5f);;
        btnNormalColor = new Color(1f, 1f, 1f, 0.6f);
        
        AddTiggers();

        if (isDemo())
        {
            AddTitleScreenBtns();
        }
        else
        {
            // #if UNITY_ANDROID || UNITY_IOS
            AddOnScreenControls();
            // #endif

            if (currentScene == "endless")
            {
                // gameLoopLoad();
            } else if (currentScene == "selectScreen")
            {
                fetchAll();
                setShip(0);
            }
        }
    }
}
