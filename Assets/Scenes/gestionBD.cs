using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class gestionBD : MonoBehaviour {

	public InputField txtNombreE;
	public InputField txtIdCurso;
	public Text textError;
	public string nomNivel;

	public string nombreUsuario;
	public int codigoEst;

	/*
	 * Respuestas WEB
	 *  error 400 cuando no se tiene onexion
	 *  error 401 cuando no encontro datos
	 *  respuesta 200 se encontraron los datos
	 *  error 402 El usuario ya existe
	 *  respuesta 201 usuario registrado
	 */

	public void iniciarSesion()
	{
		StartCoroutine (loguear());
		StartCoroutine (datos());
	}

	public void CargaNivel(string nombreNivel)
	{
		SceneManager.LoadScene(nombreNivel);
	}

	IEnumerator loguear()
	{
		//WWW conexion = new WWW ("http://localhost:8080/rookiegame/login.php?user="+txtNombreE.text+"&codE="+txtIdCurso.text);
		WWW conexion = new WWW ("http://62878a21.ngrok.io/rookiegame/login.php?user="+txtNombreE.text+"&codE="+txtIdCurso.text);
		yield return(conexion);
		if (conexion.text == "200") {
			Debug.Log ("El usuario si existe");
			CargaNivel ("discalculia");

		} else if (conexion.text == "401") {
			textError.text= "Nombre estudiante o contraseña incorrecto";
		} else {
			Debug.Log ("Error en la conexion");
		}

	}

	IEnumerator datos()
	{
		//WWW conexion = new WWW ("http://localhost:8080/rookiegame/datos.php?user="+txtNombreE.text);
		WWW conexion = new WWW ("http://62878a21.ngrok.io/rookiegame/datos.php?user="+txtNombreE.text);
		yield return(conexion);
		if (conexion.text == "401") {
			textError.text=	"El usuario no existe";
		} else {
			string[] nDatos = conexion.text.Split ('/');
			if (nDatos.Length != 2) {
				textError.text= "Error en la conexion";
			} else {
				nombreUsuario = nDatos[0];
				codigoEst = int.Parse(nDatos[1]);
			}
		}

	}
}
