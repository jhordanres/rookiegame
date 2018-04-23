using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public Transform[] PanelPreguntas;
    //atributo basico
    private Transform Banner, Titulo, score, lastScore, nombreEstudiante,_btnL, _btnR;
    private Button btnL,btnR;
    //atributos especificos
    private Text txtTitulo, txtScore, txtLastScore, txtNombreEstudiante;
    // Use this for initialization
    private float t1,delay;
    private Color transparent, fullColor;
    private string[] preguntas;
    private int numPregunta;
    bool UIcompletado;
    private Vector3 topTitulo;
    private Vector3 PanelPP;
    private Transform PreguntaActual;
    private bool Next;
    private bool MostrarResult;

    //Setters
    public void setNext(bool next)
    {
        this.Next = next;
    }
    //Getters
    public bool getNext()
    {
        return this.Next;
    }

    void Awake() {
        //instanciando hijos
        Banner = transform.Find("scorePanel");
        Titulo = transform.Find("title");
        score = transform.Find("scorePanel/score");
        lastScore = transform.Find("scorePanel/lastScore");
        nombreEstudiante = transform.Find("scorePanel/estudiante");

        txtTitulo = Titulo.GetComponent<Text>();
        txtScore = score.GetComponent<Text>();
        txtLastScore = lastScore.GetComponent<Text>();
        txtNombreEstudiante = nombreEstudiante.GetComponent<Text>();
        //Definiendo posicion inicial
        Banner.localPosition = new Vector2(0, -701);
        txtScore.text = "0";

        transparent = new Color(255, 255, 255, 0);
        fullColor = new Color(255, 255, 255, 1);
        topTitulo = new Vector3(0, 464,0);
        PanelPP = new Vector3(960, -900, 0);
        t1 = 0;
        Next = true;
        UIcompletado = false;
        MostrarResult = false;
        numPregunta = 0;
        preguntas = new string[10];
        preguntas[0] = "¿Cúal número es mayor?";
        preguntas[1] = "¿la cantidad de puntos es igual a e ste número?";
        preguntas[2] = "¿Cuantos puntos hay?";
        preguntas[3] = "¿Cúal es el resultado?";
        CargarPregunta();

    }

    // Update is called once per frame
    void Update() {
        //este parametro es ejecutado por los botones de respuesta
        //si uno de los botones es precionado entonces pasará a la sigueinte pregunta
        Preguntar(numPregunta);
        if (MostrarResult) MostrarResultado();
    }


    //Este metodo muestra la pregunta segun el numero que se le envie
    //el numero conrresponde a la posicion del array preguntas[]
    private void Preguntar(int pregunta)
    {

        txtTitulo.text = preguntas[numPregunta];
        txtTitulo.color = Color.Lerp(transparent, fullColor, t1);
        if (t1 <= 1 && !UIcompletado) t1 += Time.deltaTime / 2.5f;
        else
        {
            if (Titulo.position == topTitulo) UIcompletado = true;
            txtTitulo.fontSize = 80;
            Titulo.localPosition = Vector2.MoveTowards(Titulo.localPosition, topTitulo, 700 * Time.deltaTime);
            PreguntaActual.localPosition = Vector2.MoveTowards(PreguntaActual.localPosition, new Vector2(0, 0), 1000 * Time.deltaTime);
        }
    }

    private void MostrarResultado()
    {
        Banner.localPosition = Vector2.MoveTowards(Banner.localPosition, new Vector2(0, -395.5f), 700 * Time.deltaTime);
        if (Banner.localPosition.y >= -395.5f) MostrarResult = false;
    }

    private void OcultarPregunta(int pregunta)
    {
        txtTitulo.text = preguntas[numPregunta];
        txtTitulo.color = Color.Lerp(fullColor, transparent, t1);
        Destroy(PreguntaActual.gameObject);
        if (t1 <= 1) t1 += Time.deltaTime / 2.5f;
    }

    private void OcultarResultado()
    {
        Banner.localPosition = Vector2.MoveTowards(Banner.localPosition, new Vector2(0, -701), 700 * Time.deltaTime);
    }

    private void CargarPregunta()
    {
        PreguntaActual = Instantiate(PanelPreguntas[numPregunta], PanelPP, Quaternion.identity);
        PreguntaActual.parent = transform;
        PreguntaActual.name = "preguntaActual";
        _btnL = transform.Find("preguntaActual/Aresp/Button");
        _btnR = transform.Find("preguntaActual/Bresp/Button");
        btnL = _btnL.GetComponent<Button>();
        btnR = _btnR.GetComponent<Button>();
        btnL.onClick.AddListener(TaskOnClick);
        btnR.onClick.AddListener(TaskOnClick);

    }

    void TaskOnClick()
    {
        delay = Time.time;
        MostrarResult = true;
        Debug.Log("pos me tocó");
    }
}
