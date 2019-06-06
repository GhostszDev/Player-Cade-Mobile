using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour {
	
	// Update is called once per frame
	void Start () {

        Destroy(gameObject, 0.5f);
		
	}
}
