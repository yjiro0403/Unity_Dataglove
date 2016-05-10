using UnityEngine;
using System.Collections;

public class VibMoterController : MonoBehaviour {

    public SerialHandler serialHandler;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            serialHandler.Write("0");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            serialHandler.Write("1");
        }
    }
}
