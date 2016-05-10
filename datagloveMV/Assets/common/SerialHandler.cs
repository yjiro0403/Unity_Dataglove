using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class SerialHandler : MonoBehaviour
{
    public delegate void SerialDataReceivedEventHandler(string message);
    public event SerialDataReceivedEventHandler OnDataReceived;

    public string portName = "COM3";
    public int baudRate = 115200;

    private SerialPort serialPort_;
    private Thread thread_;
    private bool isRunning_ = false;

    private string message_;
    private bool isNewMessageReceived_ = false;

    //public float[] fing = new float[] { 0, 0, 0 };

    void Awake()
    {
        Open();
    }

    
    void Update()
    {
        if (isNewMessageReceived_)
        {
            OnDataReceived(message_);
        }
        
    }

    void OnApplicationQuit()
    {
        Close();
    }

    private void Open()
    {
        serialPort_ = new SerialPort(portName, baudRate);
        serialPort_.Open();

        Debug.Log("Open Serial port");

        isRunning_ = true;

        thread_ = new Thread(ReadData);
        thread_.Start();
        //StartCoroutine("ReadData");
    }

    private void Close()
    {
        isRunning_ = false;
        Debug.Log("Close Serial port");
        if (thread_ != null && thread_.IsAlive)
        {
            thread_.Join();
        }

        if (serialPort_ != null && serialPort_.IsOpen)
        {
            serialPort_.Close();
            serialPort_.Dispose();
        }
    }

    private void ReadData()
    {
        while (isRunning_ && serialPort_ != null && serialPort_.IsOpen)
        {
            try
            {
                message_ = serialPort_.ReadLine();
                isNewMessageReceived_ = true;
                
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Read Data Error" + e.Message);
            }

            if (message_ != "") {
                Debug.Log("Serial out:" + message_);
            }

        }
    }

    public void Write(string message)
    {
        try
        {
            serialPort_.Write(message);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    public void changeScene()
    {
        Close();
    }
}