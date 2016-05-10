using UnityEngine;
using System.Collections;

public class Aidol : MonoBehaviour {
    public AudioClip audioClip;
    AudioSource audioSource;


	// Use this for initialization
	void Start () {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        //audioSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void OnCollisionEnter(Collision collision) {
        Debug.Log("Collision");
        audioSource.Play();
    }

    void OnTriggerEnter(Collider collider)
    {
       // Debug.Log("cube1 hit OnTriggerEnter with " + collider.gameObject);
        audioSource.Play();
    }
}
