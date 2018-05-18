using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class login : MonoBehaviour {

	public string nomreEstudiante;
	public string codigoEstudiante;

	string LoginURL = "https://jhordanres.000webhostapp.com/login.php";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		StartCoroutine(LoginToDB(nomreEstudiante, codigoEstudiante));
	}

	IEnumerator LoginToDB(string nombreE, string codigoE){
		WWWForm form = new WWWForm ();
		form.AddField("nombreEPost",nombreE);
		form.AddField("codigoEPost",codigoE);

		WWW www = new WWW (LoginURL, form);
		yield return www;

		Debug.Log (www.text);
	}
}
