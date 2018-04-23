using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public Transform[] PanelPreguntas;
    //atributo basico
    private Transform Banner, Titulo, score, lastScore, nombreEstudiante;
    //atributos especificos
    private Text txtTitulo, txtScore, txtLastScore, txtNombreEstudiante;
    // Use this for initialization
    private float t1;
    private Color transparent, fullColor;
    private string[] preguntas;
    private int numPregunta;
    bool UIcompletado;
    private Vector3 topTitulo;
    private Vector3 PanelPP;
    private Transform PreguntaActual;

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
        UIcompletado = false;
        numPregunta = 0;
        preguntas = new string[10];
        preguntas[0] = "¿Cúal número es mayor?";
        preguntas[1] = "¿la cantidad de puntos es igual a e ste número?";
        preguntas[2] = "¿Cuantos puntos hay?";
        preguntas[3] = "¿Cúal es el resultado?";
        PreguntaActual = Instantiate(PanelPreguntas[numPregunta], PanelPP, Quaternion.identity);
        PreguntaActual.parent = transform;

    }

    // Update is called once per frame
    void Update() {
        Preguntar(numPregunta);
    }

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
            MostrarResultado();
            PreguntaActual.localPosition = Vector2.MoveTowards(PreguntaActual.localPosition, new Vector2(0, 0), 1000 * Time.deltaTime);
        }
    }

    private void OcultarPregunta(int pregunta)
    {
        txtTitulo.text = preguntas[numPregunta];
        txtTitulo.color = Color.Lerp(fullColor, transparent, t1);
        Destroy(PreguntaActual.gameObject);
        if (t1 <= 1) t1 += Time.deltaTime / 2.5f;
    }

    private void MostrarResultado()
    {
        Banner.localPosition = Vector2.MoveTowards(Banner.localPosition, new Vector2(0, -395.5f), 700 * Time.deltaTime);
    }

    private void OcultarResultado()
    {
        Banner.localPosition = Vector2.MoveTowards(Banner.localPosition, new Vector2(0, -701), 700 * Time.deltaTime);
    }
}
