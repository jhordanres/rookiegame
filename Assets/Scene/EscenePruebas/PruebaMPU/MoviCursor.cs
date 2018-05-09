using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class MoviCursor : MonoBehaviour {
	public static Thread oThread;
	public static SerialPort serial;
	public static string PUERTO = "COM4";

	public float angulox = 0.0f;
	public float anguloy = 0.0f;
	public float anguloz = 0.0f;
	public int pulsador = 1;

	public int velocidad = 2;

	public Animator anim;

	public LayerMask groundLayerMask;

	//Mover objetos con el pulsador
	public GameObject respu; //este era la var mano
	public GameObject mano;
	private Vector3 inicPos;
	private Vector3 finPos;
	public GameObject objetivo;

	//timo para tomar del inicio al fin
	private float lerpTime =5f;
	private float currentLerpTime =0f;

	private bool keyHit = false;





	// Use this for initialization
	void Start () {

		inicPos = respu.transform.position;
		finPos = objetivo.transform.position;

		oThread = new Thread(new ParameterizedThreadStart(escucharPuertoSerialArduino));
		// Start the thread
		oThread.Start(this);
		while(!oThread.IsAlive); // wait for thread to start
		Debug.Log("Thread started : " + Time.realtimeSinceStartup);
	}

	void EndThread() {
		try {
			if(oThread.IsAlive) {
				oThread.Abort();
				Debug.Log("Trying to abort");
			}
		} catch {
			Debug.Log("Aborting thread failed");
		}
	}


	 // Loop principal de escucha de puerto arduino 

	public void escucharPuertoSerialArduino(object parent) {
		string ultimoData = "";

		Debug.Log ("Escuchando puerto "+PUERTO+"...");
		serial = new SerialPort(PUERTO,9600);
		serial.ReadTimeout = 10000;
		while(true) {
			//Debug.Log ("Puerto Abierto "+serial.IsOpen);
			if (!serial.IsOpen) {
				Debug.Log ("Abriendo Puerto...");
				serial.Open ();
			}
			//Debug.Log("["+serial.ReadLine()+"]");


			String dataRaw = serial.ReadLine();
			ultimoData = dataRaw;
			try {
				string[] datoArray = dataRaw.Split(",".ToCharArray(),3);
				((MoviCursor) parent).angulox = float.Parse(datoArray[0]);
				((MoviCursor) parent).anguloy = float.Parse(datoArray[1]);
				((MoviCursor) parent).pulsador = int.Parse(datoArray[2]);
			} catch (Exception e) {
				//Debug.Log ("Error:"+e.Message+ "["+ultimoData+"]");
			}
		}

	}

	void OnDestroy() {
		EndThread();
	}

	void OnApplicationQuit(){
		Debug.Log ("Terminando aplicacion");
		serial.Close ();
		EndThread();
	}

	// Update is called once per frame
	void Update () {

		MoverRespuesta ();
			
		MovMano ();

		if (angulox > 25f) {
			this.transform.Translate (Vector2.left * Time.deltaTime*velocidad, Space.World);
			Debug.Log ("Estoy dentro del rango de angulo x");
		} else {
			this.transform.Translate (Vector2.right * 0);
		}
		if(angulox < -25f){
			this.transform.Translate (-Vector2.left * Time.deltaTime*velocidad, Space.World);
			Debug.Log ("Estoy dentro del rango de angulo -x");
		
		} else {
			this.transform.Translate (Vector2.right * 0);
		}
		if (anguloy > 25f) {
			this.transform.Translate (Vector2.down * Time.deltaTime*velocidad, Space.World);
		}
		if(anguloy < -25f){
			this.transform.Translate (-Vector2.down * Time.deltaTime*velocidad, Space.World);
		} else {
			this.transform.Translate (Vector2.up * 0);
		}
			
	}


	void MovMano(){
		if (pulsador == 0) {
			anim.SetBool("IsMovMano", true);
		} else{
			anim.SetBool("IsMovMano", false);
		}
	}

	bool IsOnTheCorrect(){
		if (Physics2D.Raycast (this.transform.position, Vector2.down, 0.5f, groundLayerMask.value)) {
			return true;
		} else {
			bool rayRes;
			return false;
		}
	}

	public void MoverRespuesta(){
		if (pulsador == 0) {
			keyHit = true;
		}

		if (keyHit == true && IsOnTheCorrect ()) {
			Debug.Log ("Estoy en el rango Objeto");
			currentLerpTime += Time.deltaTime;
			if (currentLerpTime >= lerpTime) {
				currentLerpTime = lerpTime;
			}

			float Perc = currentLerpTime / lerpTime;
			respu.transform.position = Vector3.Lerp (inicPos, finPos, Perc);

		}
			
		}
	}


