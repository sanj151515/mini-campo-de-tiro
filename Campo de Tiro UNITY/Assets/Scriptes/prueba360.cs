using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;
public class prueba360 : MonoBehaviour

{
    public int id = 0;
    //UI
    public Text textPuntos;
    public Text textTiempo;
    private int puntos = 0;
    private int tiempo=30;
    private bool swT = false;
    public GameObject panelGameOver;
    public Text puntosFinal;
    public Text textColor;
    //escopeta proyectil
    public Rigidbody proyectil;
    public float velocidad = 15.0f;

    public GameObject spa;
    public GameObject spa2;

    private bool swD = true;
    //campo de triro
    public Rigidbody[] peluches;
    public GameObject[] spaunPeluches;
    public float velocidadPeluches=50f;
    //comunicacion y ratacion escopeta
    public float sumitaErrorY;
    SerialPort serialPort;
    public GameObject escopeta;
    
    bool swB = true;
    int a = 0;
    void Start()
    {
        conectarArduino();
        generarID();
    }
    public void conectarArduino()
    {
        string[] ports = SerialPort.GetPortNames();
        string name = ports[ports.Length - 1];        
        serialPort = new SerialPort("\\\\.\\" + name, 9600);
        serialPort.Open(); //Abrimos una nueva conexión de puerto serie
        serialPort.ReadTimeout = 1;//velocidad
    }
    public void comenzar()
    {
        puntos = 0;
        panelGameOver.SetActive(false);
        tiempo = 30;
        swT = true;
        tiempito();
        generar();
    }
    public void generarID()
    {
        id = Random.Range(0, 5);
        switch (id)
        {
            case 0:
                textColor.text = "AZUL";
                break;
            case 1:
                textColor.text = "NEGRO";
                break;
            case 2:
                textColor.text = "ORO";
                break;
            case 3:
                textColor.text = "CAFE";
                break;
            case 4:
                textColor.text = "ROSA";
                break;
            case 5:
                textColor.text = "LILA";
                break;
        }
    }
    public void tiempito()
    {
        swD = true;
        tiempo--;
        textTiempo.text = "Tiempo: "+tiempo;
        if (tiempo == 0)
            detener();
        else
            Invoke("tiempito",1f);
    }
    public void detener()
    {
        panelGameOver.SetActive(true);
        swT = false;
        puntosFinal.text = ""+puntos;
        generarID();
    }
    public void punto()
    {
        puntos++;
        textPuntos.text = "Puntos: "+puntos;
    }
    void generar()
    {
        if (swT)
        {
            for (int i = 0; i < 3; i++)
            {
                Rigidbody instanciaPeluche = Instantiate(peluches[Random.Range(0, peluches.Length)], spaunPeluches[i].transform.position, Quaternion.identity);
                instanciaPeluche.gameObject.GetComponent<peluche>().gm = this.gameObject;
                float a = 1f;
                if (i % 2 != 0)
                    a = -1f;
                instanciaPeluche.AddForce(new Vector3(0f, 0f, velocidadPeluches * a));
            }
            Invoke("generar", 0.75f);
        }
    }    
    void dispara()
    {
        GetComponent<AudioSource>().Play();
        Rigidbody instanciaProyectil = Instantiate(proyectil, spa.transform.position, Quaternion.identity);
        instanciaProyectil.AddForce(spa2.transform.forward*100*velocidad);
    }
    void Update()
    {
        if (serialPort.IsOpen) //comprobamos que el puerto esta abierto
        {
            try //utilizamos el bloque try/catch para detectar una posible excepción.
            {
                string ax = ""; float x = 0f;
                string ay = ""; float y = 0f;

                string value = serialPort.ReadLine(); //leemos una linea del puerto serie y la almacenamos en un string
                string[] vec6 = value.Split(','); 
                //leemos el eje X
                for (int i = 0; i < vec6[0].Length; i++)
                {
                    if (vec6[0].Substring(i, 1) != ".")
                        ax = ax + vec6[0].Substring(i, 1);
                    else
                    { x = float.Parse(ax); ax = "0,"; }
                }
                //leemos el eje Y
                for (int i = 0; i < vec6[1].Length; i++)
                {
                    if (vec6[1].Substring(i, 1) != ".")
                        ay = ay + vec6[1].Substring(i, 1);
                    else
                    { y = float.Parse(ay); ay = "0,"; }
                }
                           
                escopeta.transform.rotation = Quaternion.Euler(0f, (-1)*y +sumitaErrorY,(-1)*x);

                if (vec6[2] == "1")
                {
                    if (swB && swD && swT)
                    {
                        swD = false;
                        dispara();
                        swB = false;
                    }
                }
                else
                    swB = true;                              
            }
            catch
            {}
        }
    }
}
