using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalaxyCtrl : MonoBehaviour {

	public GameObject MainCamera;
	public GameObject pivot;
	public GameObject SolarSystem; 

	public GameObject MovieObj;
	public RawImage MovieFrame;
	public TextureCtrl TextureCtrl;

	private MediaPlayerCtrl mpc;
	public AudioSource[] SpaceBGM;
	//public float speed;

	private RawImage MovieScreen;

	// Use this for initialization
	void Start () {
		mpc = GetComponent<MediaPlayerCtrl> ();
		MovieScreen = MovieObj.GetComponent<RawImage>();
		MovieScreen.color = new Color(1, 1, 1, 0);
		MovieFrame.material.color = new Color(1, 1, 1, 0);
		SolarSystem.SetActive(false);

		RenderSettings.skybox.SetFloat("_Rotation",90);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Viewer()
	{
		StartCoroutine (DowntoEarth ());

	}

	void MoviePlay(){
		Debug.Log ("動画再生開始");
		mpc.Play();
		StartCoroutine (Fade());
	}
		

	IEnumerator Rotation(){
		for (float i = 0; i <= 1300; i++) {
			pivot.transform.Rotate (new Vector3 (0, 0.2f, 0));

			if (i >= 1300) {
				StartCoroutine (DowntoEarth ());
			}
		
			yield return null;
		}
	}

	IEnumerator Fade(){
		Debug.Log ("フェードイン開始");
		for (int x = 0; x <= 50; x++) {
			MovieScreen.color = new Color(1,1,1,x/50f);
			MovieFrame.material.color = new Color(1, 1, 1, x/50f);
			yield return null;
		}
	}

	IEnumerator DowntoEarth(){
		Debug.Log ("地球への接近開始");
		for (float i = 0; i <= 2500; i++) {
			MainCamera.transform.localPosition = MainCamera.transform.localPosition + new Vector3 (0, 0, 0.114f);
			//pivot.transform.position = pivot.transform.position + new Vector3 (0.065f*speed, -0.036f*speed, 0.1f*speed);

			if (i >= 2500) {
				MoviePlay ();
				yield return new WaitForSeconds (8.5f);
				GetComponent<AudioSource> ().Play ();
				for (int x = 0; x < SpaceBGM.Length; x++) {
					SpaceBGM [x].Stop ();
				}
				TextureCtrl.FadeInOn();
				yield return new WaitForSeconds(2);
				MovieObj.SetActive(false);
				SolarSystem.SetActive(false);
			}

			yield return null;
		}
	}

	public void SoundOn(){
		for (int x = 0; x < SpaceBGM.Length; x++) {
			SpaceBGM [x].Play ();
		}
		Viewer ();
	}
}
