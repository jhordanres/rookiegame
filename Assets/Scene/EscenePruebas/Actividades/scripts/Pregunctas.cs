using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pregunctas : MonoBehaviour {
    
    public Transform[] Preguntas; /**Aqui se deben cargar las preguntas que se van a deplegar*/    
    private Transform preActual; /**Cada pregunta instanciada se almacena en la variable preActual*/    
    private Transform _btn1, _btn2; /**Se declaran los botones que seran buscados en cada prefab instanciado*/    
    private Button btn1, btn2; /**Se declara el componente Button para procesar las respuestas obtenidas*/    
    private Transform score; /**se declara el atributo score para buscarlo en el canvas*/    
    private Text txtScore; /**se el componente text para settear la puntuacion*/
    /**Se declara el componente tranform para ocultar o mostrar el score y nombre de estudiante*/
    private Transform scorePanel;

    private int numPregunta; /**Este numero indica la posicion en el arreglo de preguntas[]*/
    private bool mostrarPregunta; /**Permite mostrar solo una vez la interfaz de la pregunta*/
    private bool mostrarScore; /**Permite mostrar el resultado despues de haber contestado*/
    private bool ocultarPregunta;
    private bool ocultarScore;
    private int scoreCount;
    private float tiempoEspera;

    // Use this for initialization
    void Start() {
        score = transform.Find("scorePanel/score"); /*Se busca el objeto dentro de los hijos de Canvas*/
        scorePanel = transform.Find("scorePanel"); /*Se busca el panel el cual contiene el nombre del estudiante, score y ultima puntuacion*/
        scorePanel.localPosition = new Vector2(0, -701.1f); /*Se le asigna una posicion por fuera del GUI*/
        txtScore = score.GetComponent<Text>(); /* se obtiene el atributo Text para poder modificar el score con cada pregunta*/
        scoreCount = 0; /*Se inicializa el contador en cero para empezar la actividad de un estudiante*/
        numPregunta = 0; /*Se inicia por la primer pregunta del arreglo preguntas[]*/
        mostrarPregunta = true;
        ocultarPregunta = false;
        mostrarScore = false;
        ocultarScore = false;
    }

    // Update is called once per frame
    void Update() {
        if (mostrarPregunta)
        {
            mostrarPregunta = false;
            MostrarPregunta(numPregunta);
        }
        if (ocultarPregunta) PasarPregunta();
        if (mostrarScore) MostrarResultado();
        if (ocultarScore) OcultarResultado();

        /*Permite mostrar el resultado por un tiempo de 5 segundos*/
        if(Time.time-tiempoEspera >= 5)
        {
            ocultarScore = true;
        }
            
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
        /*El boton 1 "btn1 siempre contendra la respuesta correcta"*/
        btn1.onClick.AddListener(()=> { CalcularRespuesta(true); });
        btn2.onClick.AddListener(()=> { CalcularRespuesta(false); });
    }

    private void CalcularRespuesta(bool isCorrect)
    {
        btn1.interactable = false;
        btn2.interactable = false;
        mostrarScore = true;
        ocultarPregunta = true;
        tiempoEspera = Time.time;
        if (isCorrect) txtScore.text = (scoreCount += 5).ToString();
        else txtScore.text = (scoreCount -= 5).ToString();
    }

    private void MostrarResultado()
    {
        scorePanel.localPosition = Vector2.MoveTowards(scorePanel.localPosition, new Vector2(0, -395.5f), 700 * Time.deltaTime);
        if (scorePanel.localPosition.y >= -395.5f) mostrarScore = false;
    }

    private void OcultarResultado()
    {
        scorePanel.localPosition = Vector2.MoveTowards(scorePanel.localPosition, new Vector2(0, -701), 700 * Time.deltaTime);
        if (scorePanel.localPosition.y <= -700)
        {
            ocultarScore = false;
        }
    }

    private void PasarPregunta()
    {
        Destroy(preActual.gameObject);
        ocultarPregunta = false;
        numPregunta++;
        if (Preguntas.Length > numPregunta) mostrarPregunta = true;
        
    }
}
