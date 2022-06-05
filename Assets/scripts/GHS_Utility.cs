using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
#endif

public class GHS_Utility : MonoBehaviour
{
    private static GHS_Utility _instance;
    private bool onAndroid = false;

    public static GHS_Utility Instance
    {
        get
        {
            return _instance;
        }
    }
    
    #region Google

    public void googleAndroidSignIn()
    {
        if (onAndroid)
        {
            
        }
        else
        {
            Debug.LogError("Not on Android!");
        }
    }

    public void googleAndroidUploadScore(long score)
    {
        
    }

    public void googleAndroidGetScore()
    {
        
    }
    
    public void googleAndroidLeaderboard(){
    
    }

    #endregion

    #region GhostszMusic Network

    

    #endregion

    #region Facebook

    

    #endregion

    #region IOS

    

    #endregion

    private void Start()
    {
        googleAndroidSignIn();
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

#if UNITY_ANDROID
        onAndroid = true;
#endif
    }
}
