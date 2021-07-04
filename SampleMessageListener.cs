/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using UnityEditor;
using System;
using System.Globalization;
using UnityEngine.UI;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;
using Valve.VR;
/**
* When creating your message listeners you need to implement these two methods:
*  - OnMessageArrived
*  - OnConnectionEvent
*/
public class SampleMessageListener : MonoBehaviour
{
    public ManagerConnection managerConnection;

    private GameObject This_Object;

    private string Sensor_Position = "Left_Arm";

    public string output;
    public Single[,] Rcal = new Single[3, 3];
    Single[,] Rs = new Single[3, 3];
    Single[,] RT = new Single[3, 3];

    public float alfa, beta, gamma;
    public bool first_time = true;
    public bool Sensorlisto = false;
    public bool DCMRecibido = false;

    public bool counter = true;
    public float previous = 0;

    private Thread threadEuler; // Calcula los ángulos de Euler en un hilo (empieza una vez ENLAZA está listo)
    public Single previousTime2 = 0;

    private void OnEnable()
    {
        threadEuler = new Thread(CalculateEuler);

        first_time = true;
        Sensorlisto = false;
        DCMRecibido = false;
    }

    private void OnDisable()
    {
        threadEuler.Abort();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { first_time = true; }

        if (This_Object)
        {
            if (Sensor_Position == "Left_Upper_Arm" || Sensor_Position == "Right_Upper_Arm")
            {
                This_Object.transform.rotation = Quaternion.Euler(new Vector3(gamma, alfa, -beta)); //Works only for "upper" body and feet
            }
            else if (Sensor_Position == "Left_Fore_Arm" || Sensor_Position == "Right_Fore_Arm")
            {
                This_Object.transform.rotation = Quaternion.Euler(new Vector3(gamma, alfa, -beta)); //Works only for "upper" body and feet
            }
            else if (Sensor_Position == "Left_Hand" || Sensor_Position == "Right_Hand")
            {
                This_Object.transform.rotation = Quaternion.Euler(new Vector3(gamma, alfa, -beta)); //Works only for "upper" body and feet
            }
            else if (Sensor_Position == "Left_Thigh" || Sensor_Position == "Right_Thigh")
            {
                This_Object.transform.rotation = Quaternion.Euler(new Vector3(alfa, -gamma, -beta)); //Works only for "lower" body
            }
            else if (Sensor_Position == "Left_Calf" || Sensor_Position == "Right_Calf")
            {
                This_Object.transform.rotation = Quaternion.Euler(new Vector3(alfa, -gamma, -beta)); //Works only for "lower" body
            }
            else if (Sensor_Position == "Left_Foot" || Sensor_Position == "Right_Foot")
            {
                This_Object.transform.rotation = Quaternion.Euler(new Vector3(gamma, alfa, -beta)); //Works only for "upper" body and feet
            }
            else { This_Object.transform.rotation = Quaternion.Euler(new Vector3(gamma, alfa, -beta)); }
        }
    }

    // Invoked when a line of data is received from the serial device.
    void OnMessageArrived(string msg)
    {
        output = msg;
        if (first_time)
        {
            DCMRecibido = CalculateMatrix(output);
            if (DCMRecibido)
            {
                Sensorlisto = true;
                first_time = false;
                Calibrar();
            }
            else { managerConnection.Call4RM(); }
        }
        else { CalculateEuler(); }
    }

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        if (success)
            Debug.Log("Connection established");
        else
            Debug.Log("Connection attempt failed or disconnection detected");
    }

    public bool CalculateMatrix(string output)
    {
        string[] words = output.Split('=');
        if (words[0] != "#DCM") { return false; } // Not ready, not receiving DCM yet}
        else
        {
            // Sensor ready, word[1] contains matrix separated by comas
            string[] words2 = words[1].Split(','); // word2 contains the matrix values
            var clone = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            clone.NumberFormat.NumberDecimalSeparator = ".";
            clone.NumberFormat.NumberGroupSeparator = ".";
            Rs[0, 0] = Convert.ToSingle(decimal.Parse(words2[0], clone));
            Rs[0, 1] = Convert.ToSingle(decimal.Parse(words2[1], clone));
            Rs[0, 2] = Convert.ToSingle(decimal.Parse(words2[2], clone));

            Rs[1, 0] = Convert.ToSingle(decimal.Parse(words2[3], clone));
            Rs[1, 1] = Convert.ToSingle(decimal.Parse(words2[4], clone));
            Rs[1, 2] = Convert.ToSingle(decimal.Parse(words2[5], clone));

            Rs[2, 0] = Convert.ToSingle(decimal.Parse(words2[6], clone));
            Rs[2, 1] = Convert.ToSingle(decimal.Parse(words2[7], clone));
            Rs[2, 2] = Convert.ToSingle(decimal.Parse(words2[8], clone));

            return true;
        }
    }

    public void Calibrar()
    {
        Rcal = Rs;
        Rcal = traspuesta(Rcal);
    }

    void CalculateEuler()
    {
        if (CalculateMatrix(output) == false)
            return;
        RT = multiplicaMatrices(Rcal, Rs);

        alfa = (int) Convert.ToSingle(Math.Atan2(-RT[0, 1], RT[1, 1]) * 180 / Math.PI);
        beta = (int) Convert.ToSingle(Math.Atan2(-RT[2, 0], RT[2, 2]) * 180 / Math.PI);
        gamma = (int) Convert.ToSingle(Math.Asin(RT[2, 1]) * 180 / Math.PI);
    }

    private Single[,] multiplicaMatrices(Single[,] matriz1, Single[,] matriz2)
    {
        Single[,] matrizresultante = new Single[3, 3];

        matrizresultante[0, 0] = matriz1[0, 0] * matriz2[0, 0] + matriz1[0, 1] * matriz2[1, 0] + matriz1[0, 2] * matriz2[2, 0];
        matrizresultante[0, 1] = matriz1[0, 0] * matriz2[0, 1] + matriz1[0, 1] * matriz2[1, 1] + matriz1[0, 2] * matriz2[2, 1];
        matrizresultante[0, 2] = matriz1[0, 0] * matriz2[0, 2] + matriz1[0, 1] * matriz2[1, 2] + matriz1[0, 2] * matriz2[2, 2];

        matrizresultante[1, 0] = matriz1[1, 0] * matriz2[0, 0] + matriz1[1, 1] * matriz2[1, 0] + matriz1[1, 2] * matriz2[2, 0];
        matrizresultante[1, 1] = matriz1[1, 0] * matriz2[0, 1] + matriz1[1, 1] * matriz2[1, 1] + matriz1[1, 2] * matriz2[2, 1];
        matrizresultante[1, 2] = matriz1[1, 0] * matriz2[0, 2] + matriz1[1, 1] * matriz2[1, 2] + matriz1[1, 2] * matriz2[2, 2];

        matrizresultante[2, 0] = matriz1[2, 0] * matriz2[0, 0] + matriz1[2, 1] * matriz2[1, 0] + matriz1[2, 2] * matriz2[2, 0];
        matrizresultante[2, 1] = matriz1[2, 0] * matriz2[0, 1] + matriz1[2, 1] * matriz2[1, 1] + matriz1[2, 2] * matriz2[2, 1];
        matrizresultante[2, 2] = matriz1[2, 0] * matriz2[0, 2] + matriz1[2, 1] * matriz2[1, 2] + matriz1[2, 2] * matriz2[2, 2];

        matrizresultante = normalizaMatriz(matrizresultante);

        return matrizresultante;
    }

    private Single[,] normalizaMatriz(Single[,] matriz)
    {
        Single[,] matrizresultante = new Single[3, 3];

        Single[] fila0 = new Single[3];
        Single[] fila1 = new Single[3];
        Single[] fila2 = new Single[3];

        Single modulo0;
        Single modulo1;
        Single modulo2;

        fila0[0] = matriz[0, 0];
        fila0[1] = matriz[0, 1];
        fila0[2] = matriz[0, 2];
        fila1[0] = matriz[1, 0];
        fila1[1] = matriz[1, 1];
        fila1[2] = matriz[1, 2];
        fila2[0] = matriz[2, 0];
        fila2[1] = matriz[2, 1];
        fila2[2] = matriz[2, 2];

        modulo0 = modulo(fila0);
        modulo1 = modulo(fila1);
        modulo2 = modulo(fila2);

        matrizresultante[0, 0] = matriz[0, 0] / modulo0;
        matrizresultante[0, 1] = matriz[0, 1] / modulo0;
        matrizresultante[0, 2] = matriz[0, 2] / modulo0;

        matrizresultante[1, 0] = matriz[1, 0] / modulo1;
        matrizresultante[1, 1] = matriz[1, 1] / modulo1;
        matrizresultante[1, 2] = matriz[1, 2] / modulo1;

        matrizresultante[2, 0] = matriz[2, 0] / modulo2;
        matrizresultante[2, 1] = matriz[2, 1] / modulo2;
        matrizresultante[2, 2] = matriz[2, 2] / modulo2;

        return matrizresultante;
    }

    private Single[,] traspuesta(Single[,] matriz)
    {
        Single[,] matrizresultante = new Single[3, 3];

        matrizresultante[0, 0] = matriz[0, 0];
        matrizresultante[0, 1] = matriz[1, 0];
        matrizresultante[0, 2] = matriz[2, 0];

        matrizresultante[1, 0] = matriz[0, 1];
        matrizresultante[1, 1] = matriz[1, 1];
        matrizresultante[1, 2] = matriz[2, 1];

        matrizresultante[2, 0] = matriz[0, 2];
        matrizresultante[2, 1] = matriz[1, 2];
        matrizresultante[2, 2] = matriz[2, 2];

        return matrizresultante;
    }

    private Single modulo(Single[] vector)
    {
        Single modulo;
        modulo = Convert.ToSingle(Math.Sqrt((vector[0] * vector[0]) + (vector[1] * vector[1]) + (vector[2] * vector[2])));
        return modulo;
    }


    //Received from Menu - Script
    public void Set_Werium_Sensor_Position(string position, GameObject position_GameObject)
    {
        This_Object = position_GameObject;
        Sensor_Position = position;
    }
}