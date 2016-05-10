using UnityEngine;
using System.Collections;

public class cubescript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Box Collider -> Is Trigger OFF
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("cube1 hit OnCollisionEnter with " + collision.gameObject);
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("cube1 hit OnCollisionExit with " + collision.gameObject);
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("cube1 hit OnCollisionStay with " + collision.gameObject);
    }

    //Box Collider -> Is Trigger ON
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("cube1 hit OnTriggerEnter with " + collider.gameObject);
    }

    void OnTriggerExit(Collider collider)
    {
        Debug.Log("cube1 hit OnTriggerExit with " + collider.gameObject);
    }

    void OnTriggerStay(Collider collider)
    {
        Debug.Log("cube1 hit OnTriggerStay with " + collider.gameObject);
    }
}