using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamSwitch : MonoBehaviour {

	public WebCamSample webCamSample;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey(KeyCode.Space)) {
			webCamSample.Play();
		}
		if (Input.GetKey(KeyCode.S)) {
			webCamSample.Stop();
		}

	}
}
