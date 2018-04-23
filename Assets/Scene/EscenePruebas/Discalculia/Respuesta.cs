using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respuesta : MonoBehaviour {

    HUD scriptHUD;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        scriptHUD = GameObject.Find("Canvas").GetComponent<HUD>();
	}

    private void SiguentePregunta()
    {
        scriptHUD.setNext(true);
    }
}
