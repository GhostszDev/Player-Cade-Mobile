using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class selectScreen : MonoBehaviour
{
    
    private manager _manager;
    private GHS_Utility _ghsUtility;
    private GameObject _controller;
    public Button m_rightBtn, m_leftBtn, m_actionBtn;
    private int currentPOS, maxLength;
    private TextMeshProUGUI name, desc;
    private Image shipImage;
    
    public  ship[] ships;

    void fetchAll()
    {
        _controller = this.gameObject.transform.GetChild(2).gameObject;
        m_leftBtn = _controller.transform.GetChild(0).gameObject.GetComponent<Button>();
        m_rightBtn = _controller.transform.GetChild(1).gameObject.GetComponent<Button>();
        m_actionBtn = _controller.transform.GetChild(2).gameObject.GetComponent<Button>();
        name = this.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        desc = this.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        shipImage = this.gameObject.transform.GetChild(1).gameObject.GetComponent<Image>();
    }

    void setShip(int pos)
    {
        name.text = ships[pos].shipName;
        shipImage.sprite = ships[pos].shipImage;
        if (ships[pos].unlocked)
        {
            desc.text = ships[pos].desc;
        }
        else
        {
            desc.text = ships[pos].unlockDesc;
        }
    }

    void checkBtn()
    {
        m_rightBtn.interactable = currentPOS != maxLength;
        m_leftBtn.interactable = currentPOS != 0;
        m_actionBtn.onClick.AddListener(delegate { actionBtn(); });
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _manager = manager.Instance;
        _ghsUtility = GHS_Utility.Instance;

        ships = _manager.ship;
        maxLength = ships.Length-1;
        currentPOS = 0;
        fetchAll();
        setShip(currentPOS);
        checkBtn();
        
    }

    void leftBtn()
    {
        currentPOS--;
        checkBtn();
    }

    void rightBtn()
    {
        currentPOS++;
        checkBtn();
    }

    void actionBtn()
    {
        _manager.SelectedShip(currentPOS);
    }
}
