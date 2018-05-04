using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pregunctas : MonoBehaviour {
    
    public Transform[] Preguntas; /**Aqui se deben cargar las preguntas que se van a deplegar*/
    private float[] tiempo_respuestas; /**Aqui se guardan los tiempos de respuesta de cada pregunta*/    
    private Transform preActual; /**Cada pregunta instanciada se almacena en la variable preActual*/    
    private Transform _btn1, _btn2; /**Se declaran los botones que seran buscados en cada prefab instanciado*/    
    private Button btn1, btn2; /**Se declara el componente Button para procesar las respuestas obtenidas*/    
    private Transform score; /**se declara el atributo score para buscarlo en el canvas*/    
    private Text txtScore; /**se el componente text para settear la puntuacion*/


    private int numPregunta; /**Este numero indica la posicion en el arreglo de preguntas[]*/
    private bool mostrarPregunta; /**Permite mostrar solo una vez la interfaz de la pregunta*/
    private bool ocultarPregunta; /**Permite ocultar el panel de las preguntas*/
    private int scoreCount; /**Permite asignarle una puntiacion a cada estudiante*/
    private float tiempo; /**permite calcular el tiempo para cada pregunta*/

    // Use this for initialization
    void Start() {
        score = transform.Find("score"); /*Se busca el objeto dentro de los hijos de Canvas*/
        txtScore = score.GetComponent<Text>(); /* se obtiene el atributo Text para poder modificar el score con cada pregunta*/
        scoreCount = 0; /*Se inicializa el contador en cero para empezar la actividad de un estudiante*/
        numPregunta = 0; /*Se inicia por la primer pregunta del arreglo preguntas[]*/
        mostrarPregunta = true;
        ocultarPregunta = false;
        tiempo_respuestas = new float[Preguntas.Length]; /**el tamaño del arreglo de tiempos debe ser igual al de la c antidad de preguntas*/
    }

    // Update is called once per frame
    void Update() {

        if (mostrarPregunta)
        {
            mostrarPregunta = false;
            MostrarPregunta(numPregunta);
        }

        if (ocultarPregunta) PasarPregunta();     
    }

    private void MostrarPregunta(int pregunta)
    {
        
        preActual = Instantiate(Preguntas[pregunta], Vector2.zero, Quaternion.identity);
        preActual.parent = transform;
        preActual.name = "pregunta";
        preActual.localPosition = Vector2.zero;
        _btn1 = transform.Find("pregunta/btn1");
        _btn2 = transform.Find("pregunta/btn2");
        btn1 = _btn1.GetComponent<Button>();
        btn2 = _btn2.GetComponent<Button>();
        /*El boton 1 "btn1" siempre contendra la respuesta correcta*/
        btn1.onClick.AddListener(()=> { CalcularRespuesta(true); });
        btn2.onClick.AddListener(()=> { CalcularRespuesta(false); });
    }

    private void CalcularRespuesta(bool isCorrect)
    {
        btn1.interactable = false;
        btn2.interactable = false;
        ocultarPregunta = true;
        if (isCorrect) txtScore.text = (scoreCount += 5).ToString();
        else txtScore.text = (scoreCount -= 5).ToString();
    }

    private void PasarPregunta()
    {
        Destroy(preActual.gameObject);
        ocultarPregunta = false;
        numPregunta++;
        if (Preguntas.Length > numPregunta) mostrarPregunta = true;
        
    }


}
