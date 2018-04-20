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
	public int ejeX = -1;
	public int ejeY = 0;
	public int pulsador = 1;

	public float velocidad = 10f;

	// Use this for initialization
	void Start () {
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

	/**
	 * Loop principal de escucha de puerto arduino 
	 */
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
				string[] datoArray = dataRaw.Split(",".ToCharArray(),2);
				((MoviCursor) parent).angulox = float.Parse(datoArray[0]);
				((MoviCursor) parent).anguloy = float.Parse(datoArray[1]);
				//((MoviCursor) parent).anguloz = float.Parse(datoArray[2]);


				//((MoviCursor) parent).ejeX = int.Parse(datoArray[3]);
				//((MoviCursor) parent).ejeY = int.Parse(datoArray[4]);
				//((MoviCursor) parent).pulsador = int.Parse(datoArray[5]);
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
		//this.transform.localEulerAngles = new Vector2 (0,0);
		//Quaternion target = Quaternion.Euler(angulox,anguloy,0);
		//transform.rotation = target;

		if (angulox > 25f) {
			this.transform.Translate (Vector2.left * Time.deltaTime, Space.World);
			Debug.Log ("Estoy dentro del rango de angulo x");
		} else {
			this.transform.Translate (Vector2.right * 0);
		}
		if(angulox < -25f){
			this.transform.Translate (-Vector2.left * Time.deltaTime, Space.World);
			Debug.Log ("Estoy dentro del rango de angulo x");
		
		} else {
			this.transform.Translate (Vector2.right * 0);
		}
		if (anguloy > 25f) {
			this.transform.Translate (Vector2.down * Time.deltaTime, Space.World);
		}
		if(anguloy < -25f){
			this.transform.Translate (-Vector2.down * Time.deltaTime, Space.World);
		} else {
			this.transform.Translate (Vector2.up * 0);
		}


	//	if (ejeY > 540 && ejeY != 0) {
	//		this.transform.position = this.transform.position - Camera.main.transform.forward * velocidad;
	//	} else if (ejeY < 440 && ejeY != 0) {
	//		this.transform.position = this.transform.position + Camera.main.transform.forward * velocidad;	
	//	} else {
			//Esta centrado en eje Y
	//	}

	//	if (ejeX > 540 && ejeY != 0) {
	//		this.transform.position = this.transform.position + Camera.main.transform.right * velocidad;
	//	} else if (ejeX < 440 && ejeY != 0) {
	//		this.transform.position = this.transform.position - Camera.main.transform.right * velocidad;
	//	} else {
			//Esta centrado en eje Y
	//	}
	}
}
