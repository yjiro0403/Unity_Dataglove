using UnityEngine;
using System.Collections;

public class dataglove : MonoBehaviour {
    public SerialHandler serialHandler;

    private static Quaternion _handQuaternion = new Quaternion();

    private static float[] _fingerVal = new float[4];
    private GameObject[,] _fingerObject = new GameObject[5, 3];

    Vector3 obj_pos;

    // Use this for initialization
    void Start () {
        _fingerObject[0, 2] = GameObject.Find("Finger0_3Dummy");
        _fingerObject[0, 1] = GameObject.Find("Finger0_3Dummy/Finger0_2Dummy");
        _fingerObject[0, 0] = GameObject.Find("Finger0_3Dummy/Finger0_2Dummy/Finger0_1Dummy");
        _fingerObject[1, 2] = GameObject.Find("Finger1_3Dummy");
        _fingerObject[1, 1] = GameObject.Find("Finger1_3Dummy/Finger1_2Dummy");
        _fingerObject[1, 0] = GameObject.Find("Finger1_3Dummy/Finger1_2Dummy/Finger1_1Dummy");
        _fingerObject[2, 2] = GameObject.Find("Finger2_3Dummy");
        _fingerObject[2, 1] = GameObject.Find("Finger2_3Dummy/Finger2_2Dummy");
        _fingerObject[2, 0] = GameObject.Find("Finger2_3Dummy/Finger2_2Dummy/Finger2_1Dummy");
        _fingerObject[3, 2] = GameObject.Find("Finger3_3Dummy");
        _fingerObject[3, 1] = GameObject.Find("Finger3_3Dummy/Finger3_2Dummy");
        _fingerObject[3, 0] = GameObject.Find("Finger3_3Dummy/Finger3_2Dummy/Finger3_1Dummy");
        _fingerObject[4, 2] = GameObject.Find("Finger4_3Dummy");
        _fingerObject[4, 1] = GameObject.Find("Finger4_3Dummy/Finger4_2Dummy");
        _fingerObject[4, 0] = GameObject.Find("Finger4_3Dummy/Finger4_2Dummy/Finger4_1Dummy");

        serialHandler.OnDataReceived += OnDataReceived;

        
    }

    // Update is called once per frame
    void Update () {
        transform.rotation = _handQuaternion;

        _fingerObject[0, 2].transform.localRotation = Quaternion.Euler(0, -40, 60);
        _fingerObject[0, 1].transform.localRotation = Quaternion.Euler(0, 0, 0);
        _fingerObject[0, 0].transform.localRotation = Quaternion.Euler(0, 0, 0);

        _fingerObject[1, 2].transform.localRotation = Quaternion.Euler(_fingerVal[1], 0, 0);
        _fingerObject[1, 1].transform.localRotation = Quaternion.Euler(_fingerVal[1], 0, 0);
        _fingerObject[1, 0].transform.localRotation = Quaternion.Euler(_fingerVal[1], 0, 0);

        _fingerObject[2, 2].transform.localRotation = Quaternion.Euler(_fingerVal[2], 0, 0);
        _fingerObject[2, 1].transform.localRotation = Quaternion.Euler(_fingerVal[2], 0, 0);
        _fingerObject[2, 0].transform.localRotation = Quaternion.Euler(_fingerVal[2], 0, 0);

        _fingerObject[3, 2].transform.localRotation = Quaternion.Euler(_fingerVal[3], 0, 0);
        _fingerObject[3, 1].transform.localRotation = Quaternion.Euler(_fingerVal[3], 0, 0);
        _fingerObject[3, 0].transform.localRotation = Quaternion.Euler(_fingerVal[3], 0, 0);

        /*
        if (obj_pos.x > 1) obj_pos.x = 0;
        if (obj_pos.y > 1) obj_pos.y = 0;
        if (obj_pos.z > 1) obj_pos.z = 0;

        obj_pos = new Vector3(0, 0, 0);

        transform.position += obj_pos;
            
        obj_pos = new Vector3(0,0,0);
        */
    }

    void OnDataReceived(string message)
    {
        float x, y, z, w;

        //protocol
        //fingerVal -> rotate -> axix
        var data = message.Split(
                new string[] { "\t" }, System.StringSplitOptions.None);

        if (data.Length != 10) return;

        try
        {
            //_fingerVal[0] = float.Parse(data[0]);
            _fingerVal[1] = float.Parse(data[2]);
            _fingerVal[2] = float.Parse(data[1]);
            _fingerVal[3] = float.Parse(data[0]);
            //Debug.Log(" fing1: " + _fingerVal[1] + " fing2: " + _fingerVal[2]);

            x = -float.Parse(data[5]);
            y = -float.Parse(data[6]);
            z = float.Parse(data[4]);
            w = float.Parse(data[3]);

            _handQuaternion.Set(x, y, z, w);

            /*
            obj_pos.x = -float.Parse(data[8]) / 50.0f;
            obj_pos.y = -float.Parse(data[9]) / 50.0f;
            obj_pos.z = float.Parse(data[7]) / 50.0f;
            */
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
        
    }
}
