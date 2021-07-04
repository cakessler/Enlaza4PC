/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Threading;
using System.IO.Ports; // ADDED
using System.IO;
using UnityEngine.UI;
using System;
using TMPro;

/**
 * This class allows a Unity program to continually check for messages from a
 * serial device.
 *
 * It creates a Thread that communicates with the serial port and continually
 * polls the messages on the wire.
 * That Thread puts all the messages inside a Queue, and this SerialController
 * class polls that queue by means of invoking SerialThread.GetSerialMessage().
 *
 * The serial device must send its messages separated by a newline character.
 * Neither the SerialController nor the SerialThread perform any validation
 * on the integrity of the message. It's up to the one that makes sense of the
 * data.
 */
public class SerialController : MonoBehaviour
{

    [Tooltip("Port name with which the SerialPort object will be created.")]
    public string portName; 

    [Tooltip("Baud rate that the serial device is using to transmit data.")]
    public int baudRate = 57600;

    [Tooltip("Reference to an scene object that will receive the events of connection, " +
             "disconnection and the messages from the serial device.")]
    public GameObject messageListener;

    [Tooltip("After an error in the serial communication, or an unsuccessful " +
             "connect, how many milliseconds we should wait.")]
    public int reconnectionDelay = 1000;

    [Tooltip("Maximum number of unread data messages in the queue. " +
             "New messages will be discarded.")]
    public int maxUnreadMessages = 1;

    // Constants used to mark the start and end of a connection. There is no
    // way you can generate clashing messages from your serial device, as I
    // compare the references of these strings, no their contents. So if you
    // send these same strings from the serial device, upon reconstruction they
    // will have different reference ids.
    public const string SERIAL_DEVICE_CONNECTED = "__Connected__";
    public const string SERIAL_DEVICE_DISCONNECTED = "__Disconnected__";

    // Internal reference to the Thread and the object that runs in it.
    protected Thread thread;
    private Thread _t1;
    private Thread _t2;

    public Menu menu;

    protected SerialThreadLines serialThread;
    public static SerialPort _serialPort;
    public SampleMessageListener sampleMessage;
    public GameObject messageListener_object;

    public bool firstTime=true;
    public bool found = false;
    
    public TextMeshPro estadoSensor;

    public string previous_port = "";
   
     void Awake()
     {
        sampleMessage = messageListener_object.GetComponent<SampleMessageListener>();
        if (Application.systemLanguage == SystemLanguage.Spanish) estadoSensor.text = "Esperando conexión con el sensor...";
        if (Application.systemLanguage == SystemLanguage.Portuguese) estadoSensor.text = "Aguardando conexão com o sensor...";
        else estadoSensor.text = "Waiting for the sensor...";
        
        firstTime = true;
        found = false;
        previous_port = "";
    }

    public void Start()
    {
        if(this.gameObject.layer == 8)
        {
            portName = "";
            _t1 = new Thread(_func1);
            if (!_t1.IsAlive)
                _t1.Start();
        }
        else
        {
            portName = "";
            _t2 = new Thread(_func2);
            if (!_t2.IsAlive)
                _t2.Start();
        }
    }
    // ------------------------------------------------------------------------
    // Invoked whenever the user clicks on Connect
    // It creates a new thread that tries to connect to the serial device
    // and start reading from it.
    // ------------------------------------------------------------------------

    void _func1()
    {
        string[] ports;
        string message="";

        if (message != "")
        {          
            serialThread = new SerialThreadLines(portName,
                                            baudRate,
                                            reconnectionDelay,
                                            maxUnreadMessages);

            thread = new Thread(new ThreadStart(serialThread.RunForever));
            thread.Priority = System.Threading.ThreadPriority.Highest;
            thread.Start();
            
            firstTime = true;
            found = true;
            return;
        }
        else
        {
            while (portName == "" && !found)
            {
                _serialPort = new SerialPort();
                ports = SerialPort.GetPortNames();
                foreach (string portName1 in ports)
                {
                    _serialPort.PortName = portName1;
                    if (_serialPort.IsOpen)
                            _serialPort.Close();
                    int open = 0;
                    int set = 0;

                Debug.Log(portName1);
                    try
                    {
                        set = setPort(portName1);
                    }
                    catch { continue; }
                    try
                    {
                        open = OpenPort(portName1);
                    }
                    catch
                    {
                        continue;
                    }

                    if (open == 1 & set == 1)
                    {
                        try
                        {
                            bool flag = _serialPort.IsOpen;
                            _serialPort.ReadTimeout = 100;
                            _serialPort.WriteTimeout = 100;

                            message = _serialPort.ReadLine();
                            if (message != "")
                            {
                                found = true;
                                portName = portName1;
                                menu.Set_Previous_Port(portName1);
                                break;
                            }
                        }
                        catch
                        {
                            _serialPort.Close();
                            continue;
                        };
                    }
                }
            }

            if (_serialPort.IsOpen)
                _serialPort.Close();

            serialThread = new SerialThreadLines(portName,
                                                baudRate,
                                                reconnectionDelay,
                                                maxUnreadMessages);
            thread = new Thread(new ThreadStart(serialThread.RunForever));
            thread.Priority = System.Threading.ThreadPriority.Highest;
            thread.Start();
        }
    }

    public void Set_Previous_Port(string previous_p)
    {
        previous_port = previous_p;
    }

    void _func2()
    {
        string[] ports;
        string message = "";

        if (message != "")
        {
            serialThread = new SerialThreadLines(portName,
                                            baudRate,
                                            reconnectionDelay,
                                            maxUnreadMessages);

            thread = new Thread(new ThreadStart(serialThread.RunForever));
            thread.Priority = System.Threading.ThreadPriority.Highest;
            thread.Start();

            firstTime = true;
            found = true;
            return;
        }
        else
        {
            while (portName == "" && !found)
            {
                _serialPort = new SerialPort();
                ports = SerialPort.GetPortNames();
                foreach (string portName1 in ports)
                {
                    if (portName1 != previous_port)
                    {
                        _serialPort.PortName = portName1;
                        if (_serialPort.IsOpen)
                            _serialPort.Close();
                        int open = 0;
                        int set = 0;

                        Debug.Log(portName1);
                        try
                        {
                            set = setPort(portName1);
                        }
                        catch { continue; }
                        try
                        {
                            open = OpenPort(portName1);
                        }
                        catch
                        {
                            continue;
                        }

                        if (open == 1 & set == 1)
                        {
                            try
                            {
                                bool flag = _serialPort.IsOpen;
                                _serialPort.ReadTimeout = 100;
                                _serialPort.WriteTimeout = 100;

                                message = _serialPort.ReadLine();
                                if (message != "")
                                {
                                    found = true;
                                    portName = portName1;
                                    break;
                                }
                            }
                            catch
                            {
                                _serialPort.Close();
                                continue;
                            };
                        }
                    }
                }
            }

            if (_serialPort.IsOpen)
                _serialPort.Close();

            serialThread = new SerialThreadLines(portName,
                                                baudRate,
                                                reconnectionDelay,
                                                maxUnreadMessages);
            thread = new Thread(new ThreadStart(serialThread.RunForever));
            thread.Priority = System.Threading.ThreadPriority.Highest;
            thread.Start();
        }
    }


    public static int OpenPort(string portName)
    {
        try
        {
            if (!_serialPort.IsOpen)
            _serialPort.Open();
            return 1; // Success
        }
        catch
        {
            return 0; // Failure
        }
    }

    public static int setPort(string COM)
    {
        string portName = COM;
        int baudRate = 57600;
        Parity parity = System.IO.Ports.Parity.None;
        int dataBits = 8;
        StopBits stopBits = System.IO.Ports.StopBits.One;
        Handshake handshake = System.IO.Ports.Handshake.None;
        try
        {
            _serialPort.PortName = portName;
            _serialPort.BaudRate = baudRate;
            _serialPort.Parity = parity;
            _serialPort.DataBits = dataBits;
            _serialPort.StopBits = stopBits;
            _serialPort.Handshake = handshake;

            return 1;
        }
        catch
        {
            return 0;
        }
    }

    private void OnEnable()
    {
        firstTime = true;
        found = false;
        previous_port = "";
    }

    // ------------------------------------------------------------------------
    // Invoked whenever the SerialController gameobject is deactivated.
    // It stops and destroys the thread that was reading from the serial device.
    // ------------------------------------------------------------------------
    void OnDisable()
    {
        // If there is a user-defined tear-down function, execute it before
        // closing the underlying COM port.
        if (userDefinedTearDownFunction != null)
            userDefinedTearDownFunction();

        // The serialThread reference should never be null at this point,
        // unless an Exception happened in the OnEnable(), in which case I've
        // no idea what face Unity will make.
        if (serialThread != null)
        {
            serialThread.RequestStop();
            serialThread = null;
        }

        // This reference shouldn't be null at this point anyway.
        if (thread != null)
        {
            thread.Join();
            thread = null;
        }
}

    // ------------------------------------------------------------------------
    // Polls messages from the queue that the SerialThread object keeps. Once a
    // message has been polled it is removed from the queue. There are some
    // special messages that mark the start/end of the communication with the
    // device.
    // ------------------------------------------------------------------------
    void Update()
    {     
        if (found && firstTime) {
            if (this.gameObject.layer == 8)
            {

                if (_t1.IsAlive)
                    _t1.Abort();
            }
            else
            {
                if (_t2.IsAlive)
                    _t2.Abort();
            }    
        }

        // If the user prefers to poll the messages instead of receiving them
        // via SendMessage, then the message listener should be null.

        if (messageListener == null)
            return;
        if (serialThread == null)
            return;
        // Read the next message from the queue
       string  message = (string)serialThread.ReadMessage();
        if (message == null)
            return;

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SERIAL_DEVICE_CONNECTED))
        {
            messageListener.SendMessage("OnConnectionEvent", true);
            if (this.gameObject.layer == 8) { menu.Set_Werium_Sensor_1_Status("Disconnected"); }
            else if (this.gameObject.layer == 0) { menu.Set_Werium_Sensor_2_Status("Disconnected"); }
        }
        else if (ReferenceEquals(message, SERIAL_DEVICE_DISCONNECTED))
        {
            messageListener.SendMessage("OnConnectionEvent", false);
            if (this.gameObject.layer == 8) { menu.Set_Werium_Sensor_1_Status("Disconnected"); }
            else if (this.gameObject.layer == 0) { menu.Set_Werium_Sensor_2_Status("Disconnected"); }
        }
        else
        {
            messageListener.SendMessage("OnMessageArrived", message);
            if (this.gameObject.layer == 8) { menu.Set_Werium_Sensor_1_Status("Connected"); }
            else if (this.gameObject.layer == 0) { menu.Set_Werium_Sensor_2_Status("Connected"); }
        }
    }

    // ------------------------------------------------------------------------
    // Returns a new unread message from the serial device. You only need to
    // call this if you don't provide a message listener.
    // ------------------------------------------------------------------------
    public string ReadSerialMessage()
    {
        // Read the next message from the queue
        return (string)serialThread.ReadMessage();
    }

    // ------------------------------------------------------------------------
    // Puts a message in the outgoing queue. The thread object will send the
    // message to the serial device when it considers it's appropriate.
    // ------------------------------------------------------------------------
    public void SendSerialMessage(string message)
    {
        serialThread.SendMessage(message);
    }

    // ------------------------------------------------------------------------
    // Executes a user-defined function before Unity closes the COM port, so
    // the user can send some tear-down message to the hardware reliably.
    // ------------------------------------------------------------------------
    public delegate void TearDownFunction();
    private TearDownFunction userDefinedTearDownFunction;
    public void SetTearDownFunction(TearDownFunction userFunction)
    {
        this.userDefinedTearDownFunction = userFunction;
    }
}
