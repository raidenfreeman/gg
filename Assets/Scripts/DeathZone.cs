using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	void OnCollisionEnter(Collision collision)
    {
        Debug.LogWarning("Object:" + collision.gameObject.ToString() + ", destroyed because it fell off the world.");
        Destroy(collision.gameObject);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
