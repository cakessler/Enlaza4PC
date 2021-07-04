using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class Menu : MonoBehaviour
{
    public TextMeshPro wall_text;
    public Text menu_text;

    public GameObject Serial1;
    public GameObject Serial2;
    public GameObject Vive1;
    public GameObject Vive2;
    public GameObject Vive3;
    public GameObject Vive4;

    public SampleMessageListener sampleMessage;
    public SampleMessageListener sampleMessage2;
    public SerialController serial2;
    public Trackers_HTC trackers_script;
    public Trackers_HTC trackers_script2;
    public Trackers_HTC trackers_script3;
    public Trackers_HTC trackers_script4;

    public GameObject Head;
    public GameObject Spine;
    public GameObject Hips;
    public GameObject Left_Upper_Arm;
    public GameObject Right_Upper_Arm;
    public GameObject Left_Fore_Arm;
    public GameObject Right_Fore_Arm;
    public GameObject Left_Hand;
    public GameObject Right_Hand;
    public GameObject Left_Thigh;
    public GameObject Right_Thigh;
    public GameObject Left_Calf;
    public GameObject Right_Calf;
    public GameObject Left_Foot;
    public GameObject Right_Foot;

    private string N_Werium_Sensors = "1" + "";
    private string N_Vive_Trackers = "1" + "";
    private string Sensor1_Position = "Left_Arm";
    private string Sensor2_Position = "Left_Arm";
    private string Vive1_Position = "Left_Leg";
    private string Vive2_Position = "Left_Leg";
    private string previous_port = "";

    private string text_to_show;
    private string Werium_Sensor_1_Status = "Disconnected";
    private string Werium_Sensor_2_Status = "Disconnected";
    private string Vive_1_Status = "Disconnected";
    private string Vive_2_Status = "Disconnected";
    private string Vive_3_Status = "Disconnected";
    private string Vive_4_Status = "Disconnected";



    private void Awake()
    {
        Serial1.SetActive(false);
        Serial2.SetActive(false);
        Vive1.SetActive(false);
        Vive2.SetActive(false);
        Vive3.SetActive(false);
        Vive4.SetActive(false);
    }

    void Update()
    {
        text_to_show =
            "Werium Sensor 1: " + Werium_Sensor_1_Status + '\n' +
            "Werium Sensor 2: " + Werium_Sensor_2_Status + '\n' +
            "Vive 1: " + Vive_1_Status + '\n' +
            "Vive 2: " + Vive_2_Status + '\n' +
            "Vive 3: " + Vive_3_Status + '\n' +
            "Vive 4: " + Vive_4_Status;

        if (wall_text) { wall_text.text = text_to_show; }
        if (menu_text) { menu_text.text = text_to_show; }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Head.transform.rotation = Quaternion.identity;
            Spine.transform.rotation = Quaternion.identity;
            Hips.transform.rotation = Quaternion.identity;

            Left_Upper_Arm.transform.rotation = Quaternion.identity;
            Left_Fore_Arm.transform.rotation = Quaternion.identity;
            Left_Hand.transform.rotation = Quaternion.identity;
            Left_Thigh.transform.rotation = Quaternion.identity;
            Left_Calf.transform.rotation = Quaternion.identity;
            Left_Foot.transform.rotation = Quaternion.identity;

            Right_Upper_Arm.transform.rotation = Quaternion.identity;
            Right_Fore_Arm.transform.rotation = Quaternion.identity;
            Right_Hand.transform.rotation = Quaternion.identity;
            Right_Thigh.transform.rotation = Quaternion.identity;
            Right_Calf.transform.rotation = Quaternion.identity;
            Right_Foot.transform.rotation = Quaternion.identity;
        }
    }

    //__________________________________________________________________________________________________
    //Received from Buttons - Script
    public void Get_N_Werium_Sensors(string number)
    {
        N_Werium_Sensors = number;
        if (N_Werium_Sensors == "0")
        {
            Serial1.SetActive(false);
            Serial2.SetActive(false);
        }
    }

    public void Get_N_Vive_Trackers(string number)
    {
        N_Vive_Trackers = number;
        if(N_Vive_Trackers == "1")
        {
            Vive1.SetActive(true);
            Vive2.SetActive(false);
            Vive3.SetActive(false);
            Vive4.SetActive(false);
        }
        else if (N_Vive_Trackers == "2")
        {
            Vive1.SetActive(true);
            Vive2.SetActive(true);
            Vive3.SetActive(false);
            Vive4.SetActive(false);
        }
        else if (N_Vive_Trackers == "3")
        {
            Vive1.SetActive(true);
            Vive2.SetActive(true);
            Vive3.SetActive(true);
            Vive4.SetActive(false);
        }
        else if (N_Vive_Trackers == "4")
        {
            Vive1.SetActive(true);
            Vive2.SetActive(true);
            Vive3.SetActive(true);
            Vive4.SetActive(true);
        }
        else
        {
            Vive1.SetActive(false);
            Vive2.SetActive(false);
            Vive3.SetActive(false);
            Vive4.SetActive(false);
        }
    }
    
    public void Set_Werium_Sensor1_Position(string position)
    {
        Sensor1_Position = position;
        Serial1.SetActive(true);
        sampleMessage.Set_Werium_Sensor_Position(position, Get_My_GameObject(position));
    }

    public void Set_Previous_Port(string previous_p)
    {
        previous_port = previous_p;
    }

    public void Set_Werium_Sensor_1_Status(string status)
    {
        Werium_Sensor_1_Status = status;
    }

    public void Set_Werium_Sensor2_Position(string position)
    {
        Sensor2_Position = position;
        serial2.Set_Previous_Port(previous_port);
        Serial2.SetActive(true);
        sampleMessage2.Set_Werium_Sensor_Position(position, Get_My_GameObject(position));
    }

    public void Set_Werium_Sensor_2_Status(string status)
    {
        Werium_Sensor_2_Status = status;
    }


    public void Set_Vive_Tracker1_Position(string index, string position)
    {
        Vive1.SetActive(true);
        trackers_script.Set_Vive_Tracker_Position(index, Get_My_GameObject(position));
        Vive_1_Status = "Connected, Index: " + index + ", " + position;
    }

    public void Set_Vive_Tracker2_Position(string index, string position)
    {
        Vive2.SetActive(true);
        trackers_script2.Set_Vive_Tracker_Position(index, Get_My_GameObject(position));
        Vive_2_Status = "Connected, Index: " + index + ", " + position;
    }

    public void Set_Vive_Tracker3_Position(string index, string position)
    {
        Vive3.SetActive(true);
        trackers_script3.Set_Vive_Tracker_Position(index, Get_My_GameObject(position));
        Vive_3_Status = "Connected, Index: " + index + ", " + position;
    }

    public void Set_Vive_Tracker4_Position(string index, string position)
    {
        Vive4.SetActive(true);
        trackers_script4.Set_Vive_Tracker_Position(index, Get_My_GameObject(position));
        Vive_4_Status = "Connected, Index: " + index + ", " + position;
    }

    //Available positions: Left_Upper_Arm, Right_Upper_Arm, Left_Fore_Arm, Right_Fore_Arm, Left_Hand, Right_Hand, Left_Thigh, Right_Thigh, Left_Calf, Right_Calf, Left_Foot, Right_Foot
    private GameObject Get_My_GameObject(string position)
    {
        if (position == "Left_Upper_Arm") { return Left_Upper_Arm; }
        else if(position == "Right_Upper_Arm") { return Right_Upper_Arm; }
        else if (position == "Left_Fore_Arm") { return Left_Fore_Arm; }
        else if (position == "Right_Fore_Arm") { return Right_Fore_Arm; }
        else if (position == "Left_Hand") { return Left_Hand; }
        else if (position == "Right_Hand") { return Right_Hand; }
        else if (position == "Left_Thigh") { return Left_Thigh; }
        else if (position == "Right_Thigh") { return Right_Thigh; }
        else if (position == "Left_Calf") { return Left_Calf; }
        else if (position == "Right_Calf") { return Right_Calf; }
        else if (position == "Left_Foot") { return Left_Foot; }
        else if (position == "Right_Foot") { return Right_Foot; }
        else { return Left_Upper_Arm; }
    }
}
