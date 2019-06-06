using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checks : MonoBehaviour {

    public bool isTVMode() {

#if !UNITY_ANDROID || UNITY_EDITOR
        return false;
#else
 
        // Essentially this code is doing some java stuff to detect if the UI is in TV mode or not
        // What it does is it gets the Android Activity that is running Unity,
        // gets the value of android.content.Context.UI_MODE_SERVICE so we can call getSystemService on
        // the activity, passing in the UI_MODE_SERVICE, which gets us the UiModeManager.  Next we
        // call getCurrentModeType on our UiModeManager instance which gives us some integer that represents the UI mode.
        // We then have to get the value of android.content.res.Configuration.UI_MODE_TYPE_TELEVISION as an integer and then
        // finally we can compare that with our mode type and if they match, it is android tv.
        AndroidJavaClass unityPlayerJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject androidActivity = unityPlayerJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass contextJavaClass = new AndroidJavaClass("android.content.Context");
        AndroidJavaObject modeServiceConst = contextJavaClass.GetStatic<AndroidJavaObject>("UI_MODE_SERVICE");
        AndroidJavaObject uiModeManager = androidActivity.Call<AndroidJavaObject>("getSystemService", modeServiceConst);
        int currentModeType = uiModeManager.Call<int>("getCurrentModeType");
        AndroidJavaClass configurationAndroidClass = new AndroidJavaClass("android.content.res.Configuration");
        int modeTypeTelevisionConst = configurationAndroidClass.GetStatic<int>("UI_MODE_TYPE_TELEVISION");
 
        return (modeTypeTelevisionConst == currentModeType);
#endif
    }
}