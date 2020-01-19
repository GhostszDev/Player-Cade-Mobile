using System;
using UnityEngine;
using UnityEngine.Monetization;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class adManager : MonoBehaviour {
    private string placementId = "rewardedVideo";
    private Button adBtn;
    private GameObject player;
    private controller ctrl;
    
    #if UNITY_IOS
        private string gameID = "1308889";
    
    #elif UNITY_ANDROID
        private string gameID = "1308888";
    
    #endif

    void Start() {
        adBtn = GetComponent<Button>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (adBtn) {
            adBtn.onClick.AddListener(showAd);
        }

        if (player) {
            ctrl = player.GetComponent<controller>();
        }

        if (Monetization.isSupported) {
            Monetization.Initialize (gameID, true);
        }
    }

    void Update() {
        if (adBtn) {
            adBtn.interactable = Monetization.IsReady (placementId);
        }
    }
    
    void showAd () {
        ShowAdCallbacks options = new ShowAdCallbacks ();
        options.finishCallback = HandleShowResult;
        ShowAdPlacementContent ad = Monetization.GetPlacementContent (placementId) as ShowAdPlacementContent;
        ad.Show (options);
    }

    void HandleShowResult (ShowResult result) {
        if (result == ShowResult.Finished) {
            // Reward the player
            if (ctrl) {
                ctrl.adRewardRespawn();
            }
            
        } else if (result == ShowResult.Skipped) {
            Debug.LogWarning ("The player skipped the video - DO NOT REWARD!");
        } else if (result == ShowResult.Failed) {
            Debug.LogError ("Video failed to show");
        }
    }
}