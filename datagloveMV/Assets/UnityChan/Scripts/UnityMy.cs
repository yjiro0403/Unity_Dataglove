using UnityEngine;
using System.Collections;

public class UnityMy : MonoBehaviour {

    private Animator animator;
    public SerialHandler serialHandler;

    float[] fing = new float[] { 0, 0, 0 };

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        serialHandler.OnDataReceived += OnDataReceived;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey("up"))
        if (fing[1] > 40)
        {
                transform.position += transform.forward * 0.1f;
            //animator.SetBool("is_running", true);
        }
        else {
            //animator.SetBool("is_running", false);
        }
        //if (Input.GetKey("right"))
        if (fing[2] > 40)
        {
            transform.Rotate(0, 3, 0);
        }
        //if (Input.GetKey("left"))
        if (fing[0] > 40)
        {
                transform.Rotate(0, -3, 0);
        }
    }

    void OnDataReceived(string message)
    {
        var data = message.Split(
                new string[] { "\t" }, System.StringSplitOptions.None);
        
        if (data.Length != 10) return;

        try
        {
            fing[0] = float.Parse(data[0]);
            fing[1] = float.Parse(data[1]);
            fing[2] = float.Parse(data[2]);
            Debug.Log("fing0: " + fing[0] + " fing1: " + fing[1] + " fing2: " + fing[2]);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
        //Debug.Log("fing: " + data);
    }
}
