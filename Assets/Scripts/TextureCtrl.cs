using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextureCtrl : MonoBehaviour {

	public GameObject RawImage;
	private RawImage mask;
	private int check = 0;
	public GalaxyCtrl GalaxyCtrl;
	public RawImage BackGround;

	public Texture[] textures;

	// Use this for initialization
	void Start () {
		mask = RawImage.GetComponent<RawImage>();
		mask.material.SetTexture ("_MaskTex", textures [0]);
		mask.material.SetFloat("_Range", 0);
		BackGround.material.color = new Color(1, 1, 1, 1);

	}
	
	// Update is called once per frame
	void Update () {

		if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && check == 0) {
			GetComponent<AudioSource> ().Play ();
			GalaxyCtrl.SoundOn ();
			StartCoroutine("FadeOut");
			check = 1;
			GalaxyCtrl.SolarSystem.SetActive(true);

		}

		if (Input.GetKeyDown(KeyCode.P) && check == 0) {
			GetComponent<AudioSource>().Play();
			GalaxyCtrl.SoundOn();
			StartCoroutine("FadeOut");
			check = 1;
			GalaxyCtrl.SolarSystem.SetActive(true);
		}

		if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && check == 1) {
			SceneManager.LoadScene("Main");
		}

		if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) && check == 2) {
			SceneManager.LoadScene("Main");
		}

	}

	private IEnumerator FadeOut() {
		//シースルーカメラを徐々に非表示
		for(float x=0f; x<=1f; x+=0.01f) {
			mask.material.SetFloat("_Range", x);
			BackGround.material.color = new Color (1,1,1,1-x);
			yield return null;
		}

	}

	private IEnumerator FadeIn() {
		//シースルーカメラを徐々に表示
		for (float x = 1f; x >=0f; x-=0.01f) {
			mask.material.SetFloat("_Range", x);
			BackGround.material.color = new Color(1, 1, 1, 1 - x);
			GalaxyCtrl.MovieFrame.material.color = new Color(1, 1, 1, x);
			yield return null;
		}
		check = 2;
	}

	public void FadeInOn(){
		mask.material.SetTexture ("_MaskTex", textures [1]);
		StartCoroutine ("FadeIn");
	}
}
