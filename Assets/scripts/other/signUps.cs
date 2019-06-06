using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class signUps : MonoBehaviour {

    bool _fbsign;
    bool _gsign;
    bool _gmsign;

#if UNITY_ANDROID
    // Use this for initialization
    void OnAwake () {

        // authenticate user:
        Social.localUser.Authenticate((bool success) => {
            // handle success or failure
        });

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void gSign() {

    }
#endif

}
