using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetsRotate : MonoBehaviour {

	public GameObject Earth;
	public GameObject Moon;
	public GameObject Venus;
	public GameObject Mars;
	public GameObject Mercury;
	public GameObject Jupiter;
	public GameObject Saturn;

	private float x;

	// Use this for initialization
	void Start () {
		x = 3;
	}
	
	// Update is called once per frame
	void Update () {
		Earth.transform.Rotate (new Vector3 (0, x * 1/24f, 0));
		Moon.transform.Rotate (new Vector3 (0, x * 1/(27*24f), 0));
		Venus.transform.Rotate (new Vector3 (0, x * 1 / (243*24f), 0));
		Mars.transform.Rotate (new Vector3 (0, x * 1/25f, 0));
		Mercury.transform.Rotate (new Vector3 (0, x * 1 / (59*24f), 0));
		Jupiter.transform.Rotate (new Vector3 (0, x * 1/9f, 0));
		Saturn.transform.Rotate (new Vector3 (0, x * 1/10f, 0));
	}
}
