using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;
using Valve.VR;

public class Buttons : MonoBehaviour
{
    public Menu menu;

    public GameObject Werium_1_interface;
    public GameObject Werium_2_interface;
    public GameObject Vive_1_interface;
    public GameObject Vive_2_interface;
    public GameObject Vive_3_interface;
    public GameObject Vive_4_interface;

    public Text Input_Text_N_Werium_Sensors;
    public Text Input_Text_N_Vive_Trackers;
    public Text Input_Text_Vive_1_index;

    private string N_Werium_Sensors = "1" + "";
    private string N_Vive_Trackers = "1" + "";
    private string Vive_Index = "3" + "";

    private string Sensor = "Werium_Sensor_1"; //Available sensors: Werium_Sensor_1, Werium_Sensor_2, Vive_Tracker_1, Vive_Tracker_2, Vive_Tracker_3, Vive_Tracker_4

    private void Awake()
    {
        Werium_2_interface.SetActive(false);
        Vive_2_interface.SetActive(false);
        Vive_3_interface.SetActive(false);
        Vive_4_interface.SetActive(false);
    }

    public void Get_N_Werium_Sensors()
    {
        if (Input_Text_N_Werium_Sensors.text != null || Input_Text_N_Werium_Sensors.text != "")
        {
            N_Werium_Sensors = Input_Text_N_Werium_Sensors.text;

            //Activate or Deactivate Werium Sensors Interfaces
            if (N_Werium_Sensors == "0")
            {
                Werium_1_interface.SetActive(false);
                Werium_2_interface.SetActive(false);
                if (Sensor == "Werium_Sensor_1" || Sensor == "Werium_Sensor_2")
                {
                    Sensor = "";
                }
                menu.Get_N_Werium_Sensors("0");
            }
            else if (N_Werium_Sensors == "1")
            {
                Werium_1_interface.SetActive(true);
                Werium_2_interface.SetActive(false);
                menu.Get_N_Werium_Sensors(N_Werium_Sensors);
            }
            else if (N_Werium_Sensors == "2")
            {
                Werium_1_interface.SetActive(true);
                Werium_2_interface.SetActive(true);
                menu.Get_N_Werium_Sensors(N_Werium_Sensors);
            }
            else
            {
                Werium_1_interface.SetActive(true);
                Werium_2_interface.SetActive(false);
                menu.Get_N_Werium_Sensors("1");
            }
        }
    }

    public void Get_N_Vive_Trackers()
    {
        if (Input_Text_N_Vive_Trackers.text != null || Input_Text_N_Vive_Trackers.text != "")
        {
            N_Vive_Trackers = Input_Text_N_Vive_Trackers.text;

            //Activate or Deactivate Vive Trackers Interfaces
            if (N_Vive_Trackers == "0")
            {
                Vive_1_interface.SetActive(false);
                Vive_2_interface.SetActive(false);
                Vive_3_interface.SetActive(false);
                Vive_4_interface.SetActive(false);
                if (Sensor == "Vive_Tracker_1" || Sensor == "Vive_Tracker_2")
                {
                    Sensor = "";
                }
                menu.Get_N_Vive_Trackers("0");
            }
            else if (N_Vive_Trackers == "1")
            {
                Vive_1_interface.SetActive(true);
                Vive_2_interface.SetActive(false);
                Vive_3_interface.SetActive(false);
                Vive_4_interface.SetActive(false);
                menu.Get_N_Vive_Trackers(N_Vive_Trackers);
            }
            else if (N_Vive_Trackers == "2")
            {
                Vive_1_interface.SetActive(true);
                Vive_2_interface.SetActive(true);
                Vive_3_interface.SetActive(false);
                Vive_4_interface.SetActive(false);
                menu.Get_N_Vive_Trackers(N_Vive_Trackers);
            }
            else if (N_Vive_Trackers == "3")
            {
                Vive_1_interface.SetActive(true);
                Vive_2_interface.SetActive(true);
                Vive_3_interface.SetActive(true);
                Vive_4_interface.SetActive(false);
                menu.Get_N_Vive_Trackers(N_Vive_Trackers);
            }
            else if (N_Vive_Trackers == "4")
            {
                Vive_1_interface.SetActive(true);
                Vive_2_interface.SetActive(true);
                Vive_3_interface.SetActive(true);
                Vive_4_interface.SetActive(true);
                menu.Get_N_Vive_Trackers(N_Vive_Trackers);
            }
            else
            {
                Vive_1_interface.SetActive(true);
                Vive_2_interface.SetActive(false);
                Vive_3_interface.SetActive(false);
                Vive_4_interface.SetActive(false);
                menu.Get_N_Vive_Trackers("1");
            }
        }
    }

    public void Get_Vive1_index()
    {
        if(Input_Text_Vive_1_index.text != null || Input_Text_Vive_1_index.text != "")
        {
            Vive_Index = Input_Text_Vive_1_index.text;
            if (Vive_Index == "1") { }
            else if (Vive_Index == "2") { }
            else if (Vive_Index == "3") { }
            else if (Vive_Index == "4") { }
            else if (Vive_Index == "5") { }
            else if (Vive_Index == "6") { }
            else if (Vive_Index == "7") { }
            else if (Vive_Index == "8") { }
            else if (Vive_Index == "9") { }
            else { Vive_Index = "3"; }
        }
    }

    //________________________________________
    public void Set_Werium_Sensor_1()
    {
        Sensor = "Werium_Sensor_1";
    }

    public void Set_Werium_Sensor_2()
    {
        Sensor = "Werium_Sensor_2";
    }

    public void Set_Vive_Tracker_1()
    {
        Sensor = "Vive_Tracker_1";
    }

    public void Set_Vive_Tracker_2()
    {
        Sensor = "Vive_Tracker_2";
    }

    public void Set_Vive_Tracker_3()
    {
        Sensor = "Vive_Tracker_3";
    }

    public void Set_Vive_Tracker_4()
    {
        Sensor = "Vive_Tracker_4";
    }
    //________________________________________


    //All Buttons about Sensor's Position
    //Available positions: Left_Upper_Arm, Right_Upper_Arm, Left_Fore_Arm, Left_Hand, Right_Hand, Right_Fore_Arm, Left_Thigh, Right_Thigh, Left_Calf, Right_Calf, Left_Foot, Right_Foot
    public void Set_LeftUpperArm()
    {
        Send_Sensor_and_Position("Left_Upper_Arm");
    }

    public void Set_RightUpperArm()
    {
        Send_Sensor_and_Position("Right_Upper_Arm");
    }

    public void Set_LeftForeArm()
    {
        Send_Sensor_and_Position("Left_Fore_Arm");
    }

    public void Set_RightForeArm()
    {
        Send_Sensor_and_Position("Right_Fore_Arm");
    }

    public void Set_LeftHand()
    {
        Send_Sensor_and_Position("Left_Hand");
    }

    public void Set_RightHand()
    {
        Send_Sensor_and_Position("Right_Hand");
    }

    public void Set_LeftThigh()
    {
        Send_Sensor_and_Position("Left_Thigh");
    }

    public void Set_RightThigh()
    {
        Send_Sensor_and_Position("Right_Thigh");
    }

    public void Set_LeftCalf()
    {
        Send_Sensor_and_Position("Left_Calf");
    }

    public void Set_RightCalf()
    {
        Send_Sensor_and_Position("Right_Calf");
    }

    public void Set_LeftFoot()
    {
        Send_Sensor_and_Position("Left_Foot");
    }

    public void Set_RightFoot()
    {
        Send_Sensor_and_Position("Right_Foot");
    }
    //Available positions: Left_Upper_Arm, Right_Upper_Arm, Left_Fore_Arm, Left_Hand, Right_Hand, Right_Fore_Arm, Left_Thigh, Right_Thigh, Left_Calf, Right_Calf, Left_Foot, Right_Foot


    //Send the senor and position to the Menu
    private void Send_Sensor_and_Position(string position)
    {
        if(Sensor == "Werium_Sensor_1")
        {
            menu.Set_Werium_Sensor1_Position(position);
        }
        else if (Sensor == "Werium_Sensor_2")
        {
            menu.Set_Werium_Sensor2_Position(position);
        }
        else if (Sensor == "Vive_Tracker_1")
        {
            menu.Set_Vive_Tracker1_Position(Vive_Index, position);
        }
        else if(Sensor == "Vive_Tracker_2")
        {
            menu.Set_Vive_Tracker2_Position(Vive_Index, position);
        }
        else if (Sensor == "Vive_Tracker_3")
        {
            menu.Set_Vive_Tracker3_Position(Vive_Index, position);
        }
        else if (Sensor == "Vive_Tracker_4")
        {
            menu.Set_Vive_Tracker4_Position(Vive_Index, position);
        }
    }
}
