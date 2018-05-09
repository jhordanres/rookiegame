using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pregunctas : MonoBehaviour {
//<<<<<<< HEAD
 	
//=======

    /**Se inicializan las variables para asignar un nombre de estudiante*/
    private Transform _NombreEstudiante;
    private Text NombreEstudiante;
    public string txtEstudiante = "Karen Pinzón";

//>>>>>>> 2cdc155f2172e9a34b7990e5c60c24658bd3d2bd
    public Transform[] Preguntas; /**Aqui se deben cargar las preguntas que se van a deplegar*/
    private float[] tiempo_respuestas; /**Aqui se guardan los tiempos de respuesta de cada pregunta*/
    private float[] respuestas;
    private Transform preActual; /**Cada pregunta instanciada se almacena en la variable preActual*/
    private Transform _btn1, _btn2; /**Se declaran los botones que seran buscados en cada prefab instanciado*/
    private Button btnCorrecto, btn2; /**Se declara el componente Button para procesar las respuestas obtenidas*/
    private Transform score; /**se declara el atributo score para buscarlo en el canvas*/
    private Text txtScore; /**se el componente text para settear la puntuacion*/
    private Transform _pruebaFinalizada;
    private Text pruebaFinalizada;


    private int numPregunta; /**Este numero indica la posicion en el arreglo de preguntas[]*/
    private bool mostrarPregunta; /**Permite mostrar solo una vez la interfaz de la pregunta*/
    private bool ocultarPregunta; /**Permite ocultar el panel de las preguntas*/
    private int scoreCount; /**Permite asignarle una puntiacion a cada estudiante*/
    private float tiempo; /**permite calcular el tiempo para cada pregunta*/

    // Use this for initialization
    void Start() {
        _NombreEstudiante = transform.Find("estudiante");
        NombreEstudiante = _NombreEstudiante.GetComponent<Text>();
        NombreEstudiante.text = txtEstudiante;

        _pruebaFinalizada = transform.transform.Find("fin");
        pruebaFinalizada = _pruebaFinalizada.GetComponent<Text>();
        pruebaFinalizada.color = new Vector4(0,0,0,0);

        score = transform.Find("score"); /*Se busca el objeto dentro de los hijos de Canvas*/
        txtScore = score.GetComponent<Text>(); /* se obtiene el atributo Text para poder modificar el score con cada pregunta*/
        scoreCount = 0; /*Se inicializa el contador en cero para empezar la actividad de un estudiante*/
        numPregunta = 0; /*Se inicia por la primer pregunta del arreglo preguntas[]*/
        mostrarPregunta = true;
        ocultarPregunta = false;
        tiempo_respuestas = new float[Preguntas.Length]; /**el tamaño del arreglo de tiempos debe ser igual al de la c antidad de preguntas*/
        respuestas = new float[Preguntas.Length];
        tiempo = 0;
    }

    // Update is called once per frame
    void Update() {


        if (mostrarPregunta)
        {
            mostrarPregunta = false;
            MostrarPregunta(numPregunta);
        }

//<<<<<<< HEAD
        if (ocultarPregunta) PasarPregunta();     

//=======
        if (ocultarPregunta) PasarPregunta();
//>>>>>>> 2cdc155f2172e9a34b7990e5c60c24658bd3d2bd
    }

    private void MostrarPregunta(int pregunta)
    {

        tiempo = Time.time;
        preActual = Instantiate(Preguntas[pregunta], Vector2.zero, Quaternion.identity);
        preActual.parent = transform;
        preActual.name = "pregunta";
        preActual.localPosition = Vector2.zero;
        _btn1 = transform.Find("pregunta/btn1");
        _btn2 = transform.Find("pregunta/btn2");
        btnCorrecto = _btn1.GetComponent<Button>();
        btn2 = _btn2.GetComponent<Button>();
        /*El boton 1 "btn1" siempre contendra la respuesta correcta*/
        btnCorrecto.onClick.AddListener(() => { GuardarRespuesta(true); });
        btn2.onClick.AddListener(() => { GuardarRespuesta(false); });
    }

    private void GuardarRespuesta(bool isCorrect)
    {
        //desactiva los bototnes para que no genere mas eventos hasta que pase a la sigueinte pregunta
        btnCorrecto.interactable = false;
        btn2.interactable = false;
        

        float duracionRespueta;
        txtScore.text = (scoreCount += 5).ToString();
        duracionRespueta = Time.time - tiempo;
        tiempo_respuestas[numPregunta] = duracionRespueta;
        if (isCorrect)
        {
            if (duracionRespueta <= 7) respuestas[numPregunta] = 5.0f;
            else if (duracionRespueta > 7 && duracionRespueta <= 20) respuestas[numPregunta] = 4.0f;
            else if (duracionRespueta > 20 && duracionRespueta <= 60) respuestas[numPregunta] = 3.0f;
            else if (duracionRespueta > 60 && duracionRespueta <= 120) respuestas[numPregunta] = 2.0f;
            else if (duracionRespueta > 120) respuestas[numPregunta] = 1.0f;
        }
        else respuestas[numPregunta] = 0.0f;

        Debug.Log("ResultPre" + (numPregunta + 1) +": " + respuestas[numPregunta]+ ", Respondida en: "+tiempo_respuestas[numPregunta]+" segundos");
        
        //Con este atributo activado permite que se pase a la siguente pregunta
        ocultarPregunta = true;
    }
    private void PasarPregunta()
    {
        Destroy(preActual.gameObject);
        ocultarPregunta = false;
        numPregunta++;
        //Evita que el numPregunta se salga de la dimencion del arreglo
        if (Preguntas.Length > numPregunta) mostrarPregunta = true;
        else ResultadoFinal();
        
    }

    private void ResultadoFinal()
    {
        float promedioNotas, promedioTiempo, totalNotas, totalTiempo;
        totalNotas = 0;
        totalTiempo = 0;
        foreach (float nota in respuestas) totalNotas += nota;
        foreach (float tiempos in tiempo_respuestas) totalTiempo += tiempos;
        promedioNotas = totalNotas / respuestas.Length;
        promedioTiempo = totalTiempo / tiempo_respuestas.Length;
        pruebaFinalizada.color = new Vector4(0, 0, 0, 1);
        Debug.Log("nota: " + promedioNotas + " | Tiempo: " + promedioTiempo);
    }


}
