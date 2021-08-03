using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class GameManager : MonoBehaviour
{
    SerialPort serialPort = new SerialPort("COM12", 9600); //Inicializamos el puerto serie
    public GameObject escopeta;
    float smooth = 5.0f;

    public float factor =7f;
    public float gyro_normalizer_factor = 1.0f;//1.0f / 32768.0f;
    void Start()
    {

        serialPort.Open(); //Abrimos una nueva conexión de puerto serie
        serialPort.ReadTimeout = 1; //Establecemos el tiempo de espera cuando una operación de lectura no finaliza
    }

    void Update()
    {
        if (serialPort.IsOpen) //comprobamos que el puerto esta abierto
        {
            try //utilizamos el bloque try/catch para detectar una posible excepción.
            {

                string value = serialPort.ReadLine(); //leemos una linea del puerto serie y la almacenamos en un string
                //print(value); //printeamos la linea leida para verificar que leemos el dato que manda nuestro Arduino
                string[] vec6 = value.Split(','); //Separamos el String leido valiendonos 
                                                  //de las comas y almacenamos los valores en un array.
                                                  //escopeta.transform.eulerAngles = new Vector3(float.Parse(vec6[0]), float.Parse(vec6[1]), 0f);
                                                  //Quaternion target = Quaternion.Euler(float.Parse(vec6[0]), float.Parse(vec6[1]),0f);

                // Dampen towards the target rotation
                //escopeta.transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
                //escopeta.transform.rotation.Set(float.Parse(vec6[0]), float.Parse(vec6[1]), 0f,0f);
                //escopeta.transform.rotation.x = 13;
                string ax="";float x=0f;
                string ay = ""; float y = 0f;
                for (int i = 0; i < vec6[0].Length; i++)
                {
                    if (vec6[0].Substring(i, 1) != ".")
                        ax = ax + vec6[0].Substring(i, 1);
                    else
                    { x = float.Parse(ax)*gyro_normalizer_factor; ax = "0,"; }
                }
                //x += float.Parse(ax);
                //print(float.Parse(ax));
                for (int i = 0; i < vec6[1].Length; i++)
                {
                    if (vec6[1].Substring(i, 1) != ".")
                        ay = ay + vec6[1].Substring(i, 1);
                    else
                    { y = float.Parse(ay)*gyro_normalizer_factor; ay = "0,"; }
                }
                if (Mathf.Abs(x) < 0.025f) x = 0f;
                if (Mathf.Abs(y) < 0.025f) y = 0f;
                //y += float.Parse(ay);
                escopeta.transform.rotation = Quaternion.Euler(x * factor, (-1f)*y * factor, 0 * factor);
                //vec6[0].Replace('.',',');
                //float x = float.Parse(vec6[0]);
                //float y = float.Parse(vec6[1]);
                /*if (!(escopeta.transform.rotation.x >= x - 0.5f && escopeta.transform.rotation.x <= x + 0.5f))
                {
                    print("x:   "+x);
                    print("x22:   " + escopeta.transform.rotation.x);
                    if (escopeta.transform.rotation.x >= x - 0.5f)
                        escopeta.transform.Rotate(new Vector3(-0.25f, 0f, 0f),Space.World);
                    else
                        escopeta.transform.Rotate(new Vector3(0.25f, 0f, 0f), Space.World);
                }
                else
                {
                    escopeta.transform.Rotate(new Vector3(0f, 0f, 0f), Space.World);
                }*/
            }

            catch
            {

            }

        }

    }
}
