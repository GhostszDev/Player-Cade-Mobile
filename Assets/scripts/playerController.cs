using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    private manager _manager;
    private GHS_Utility _ghsUtility;
    private float speed;
    private Rigidbody2D fb2d;
    public int currentLvl;

    public GameObject player;
    public Image playerShip;
    public ship currentShip;
    public bool turnRight = false;
    public GameObject bulletPrefab;
    public GameObject[] cannon;
    public float maxShootTimer = 5f;
    public float shootTimer = 5f;
    public int currentPOS = 0;

    void ShipShot()
    {

        for (int i = 0; i < cannon.Length; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, cannon[i].transform.position + (Vector3.up*50f),  
                Quaternion.identity,
                player.transform.parent);
        }
        
    }
    
    public void ShipMovement(float movement)
    {
        fb2d.velocity = new Vector2(speed * movement * Time.fixedDeltaTime, 0f);
    }
    
    void DemoMovement()
    {
        if (turnRight)
        {
            ShipMovement(1);
        }
        else
        {
            ShipMovement(-1);
        }
        
    }

    void CreateCannons(int lvl, int amountOfCannons)
    {
        Debug.Log(lvl);
        cannon = new GameObject[amountOfCannons];

        for (int i = 0; i < cannon.Length; i++)
        {
            cannon[i] = new GameObject("cannon_" + i);
            cannon[i].transform.SetParent(player.transform);
            cannon[i].transform.localPosition = new Vector3(0,0,0);
        }

        switch (lvl)
        {
            case 2:
                cannon[0].transform.localPosition = new Vector3(-66.4000015f,11.3000002f,0);
                cannon[1].transform.localPosition = new Vector3(66.4000015f,11.3000002f,0);
                break;
            case 3:
                cannon[0].transform.localPosition = new Vector3(0, 70f, 0);
                cannon[1].transform.localPosition = new Vector3(-66.4000015f,11.3000002f,0);
                cannon[2].transform.localPosition = new Vector3(66.4000015f,11.3000002f,0);
                break;
            case 1:
                cannon[0].transform.localPosition = new Vector3(0, 70f, 0);
                break;
        }
    }

    void CreateBoxColliders(ship s)
    {
        BoxCollider2D box2d = player.AddComponent<BoxCollider2D>();
        box2d.offset = s.offset;
        box2d.size = s.size;
    }

    // Start is called before the first frame update
    void Start()
    {
        _manager = manager.Instance;
        _ghsUtility = GHS_Utility.Instance;
        currentShip = _manager.GetSelectedShip();
        speed = Screen.width * (currentShip.speed * 10f);
        player = this.gameObject;

        if (player)
        {
            if (!fb2d)
            {
                this.gameObject.AddComponent<Rigidbody2D>();
            }

            fb2d = this.gameObject.GetComponent<Rigidbody2D>();
            fb2d.constraints = RigidbodyConstraints2D.FreezePositionY;

            if (_manager.isDemo())
            {
                player.transform.localPosition = new Vector3(0, 200, 0);
            }

            playerShip = player.transform.GetChild(0).gameObject.GetComponent<Image>();

            CreateBoxColliders(currentShip);
            
            playerShip.sprite = currentShip.shipImage;
            playerShip.GetComponent<RectTransform>().localScale = currentShip.shipRectTransform;
            CreateCannons(currentLvl, currentShip.cannons[currentLvl-1]);
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (_manager.isDemo())
        {

            if (shootTimer <= 0)
            {
                ShipShot();
                shootTimer = maxShootTimer;
            }
            else
            {
                shootTimer -= 1 * Time.deltaTime;
            }

        }
    }

    private void FixedUpdate()
    {
        if (_manager.isDemo())
        {
            DemoMovement();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("leftTurn"))
        {
            turnRight = true;
        }

        if (other.CompareTag("rightTurn"))
        {
            turnRight = false;
        }
    }
}
