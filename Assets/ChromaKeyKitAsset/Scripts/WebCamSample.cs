using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WebCamSample : MonoBehaviour {

	public int requestWidth = 720;
	public int requestHeight = 405;
	
	public KitControllerRendererBase[] webCamRendererKitTargets;
	public KitControllerRawImageBase[] webCamRawImageKitTargets;

	public Renderer[] webCamRendererTargets;
	public RawImage[] webCamRawImageTargets;

	private WebCamTexture _webcamTexture;
	private int _defaultWebCamTextureWidth = 4;
	private int _defaultWebCamTextureHeight = 4;

	private List<ITextureUpdatable> _webCamTargets = new List<ITextureUpdatable>();
	
	private bool _inited = false;

	// Use this for initialization
	void Start() {
		StartCoroutine(AuthorizeWebCam());
	}

	private IEnumerator AuthorizeWebCam() {
		//デバイスのカメラを認識、使用許可を得て再生
		Debug.Log("WebCamSample: WebCam Authorization...");
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

		if (Application.HasUserAuthorization(UserAuthorization.WebCam)) {
			Debug.Log("WebCamSample: WebCam Authorized");

			_webcamTexture = new WebCamTexture(requestWidth, requestHeight);
			_defaultWebCamTextureWidth = _webcamTexture.width;
			_defaultWebCamTextureHeight = _webcamTexture.height;

			_webcamTexture.Play();
			StartCoroutine(InitWebCamTexture());
		}
	}

	private IEnumerator InitWebCamTexture() {
		Debug.Log("WebCamSample: WebCamTexture initialization...");
		while (_defaultWebCamTextureWidth == _webcamTexture.width && _defaultWebCamTextureHeight == _webcamTexture.height) {
			yield return null;
		}
		Debug.Log("WebCamSample: WebCamTexture initialized ("+_webcamTexture.deviceName+" " +_webcamTexture.width+"x"+_webcamTexture.height+")");

		_inited = true;

		SetWebCamTexture();
	}

	private void SetWebCamTexture() {
		foreach (Renderer target in webCamRendererTargets) {
			if (target != null) {
				target.material.mainTexture = _webcamTexture;
			}
		}
		foreach (RawImage target in webCamRawImageTargets) {
			if (target != null) {
				target.texture = _webcamTexture;
			}
		}
		foreach (KitControllerRendererBase target in webCamRendererKitTargets) {
			if(target != null) {
				target.SetTexture(_webcamTexture);
				_webCamTargets.Add(target);
			}
		}
		foreach (KitControllerRawImageBase target in webCamRawImageKitTargets) {
			if (target != null) {
				target.SetTexture(_webcamTexture);
				_webCamTargets.Add(target);
			}
		}
	}

	void Update() {
		if (_inited && _webcamTexture.didUpdateThisFrame) {
			foreach (ITextureUpdatable target in _webCamTargets) {
				target.UpdateRender();
			}
		}
	}

	public void Play() {
		if(_inited) {
			_webcamTexture.Play();
		}
	}

	public void Stop() {
		if (_inited) {
			_webcamTexture.Stop();
		}
	}

}