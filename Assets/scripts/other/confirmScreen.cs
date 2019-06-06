using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class confirmScreen : MonoBehaviour {

    public pauseScreen ps;
    public GameObject go;

    void Start() {

        go = this.gameObject.transform.parent.gameObject;
        ps = this.gameObject.transform.parent.gameObject.GetComponent<pauseScreen>();
        
    }

    public void confirmBtn() {
        ps.closeConfirm();
    }

    public void quitBtn() {
        ps.quitGame();
    }
}
