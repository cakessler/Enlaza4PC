using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;

public class ManagerConnection : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int x, int y);
    public bool ENLAZA = false;
    public SerialController serial_controller;

    public void Call4RM()
    {
        serial_controller.SendSerialMessage("#om");
    }

    public void Call4AMG()
    {
        serial_controller.SendSerialMessage("#osct");
    }
}
