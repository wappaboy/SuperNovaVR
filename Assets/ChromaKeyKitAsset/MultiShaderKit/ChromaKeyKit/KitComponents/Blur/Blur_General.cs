using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(KitControllerBase))]
public class Blur_General : KitComponentBase
{
	[Range(0.0f, 100.0f)]
	public float blurX = 3.0f;
	[Range(0.0f, 100.0f)]
	public float blurY = 3.0f;
	[Range(1, 4)]
	public int blurIterations = 2;

	[Tooltip("Update shader properties in play mode")]
	public bool isUpdateProperties = true;

	[Tooltip("Any shader ../ChromaKeyKit/Blur/Blur_..")]
	public Shader blurShader = null;

	//private float _blurX = -1;
	//private float _blurY = -1;
	private int _blurIterations = -1;
	private List<RenderTexture> _rtBs; //render textures Blur
	private Material _blurShaderMat;

	void Awake() {
		if (blurShader == null) {
			blurShader = Shader.Find("ChromaKeyKit/Blur/Blur_010-101-010");
		}
	}

	// Use this for initialization
	void Start() {
		_blurShaderMat = createMaterial(blurShader);
		_rtBs = new List<RenderTexture>();
	}

	private void updateShaderProperties() {
		if (_blurIterations != blurIterations) {
			_blurIterations = blurIterations;
			_rtBs.Clear();
			RenderTexture rt = null;
			for (int i = 0; i < _blurIterations; i++) {
				rt = new RenderTexture(_textureWidth, _textureHeight, 0);
				rt.filterMode = FilterMode.Bilinear;
				_rtBs.Add(rt);
			}
		}
	}

	override public RenderTexture getRender(RenderTexture rt_src) {
		if (isUpdateProperties) {
			updateShaderProperties();
		}
		RenderTexture rt = null;
		RenderTexture rtS = rt_src;
		for (var i = 1; i <= _blurIterations; i++) {
			_blurShaderMat.SetFloat("_BlurOffsetX", blurX / Mathf.Pow(2, i));
			_blurShaderMat.SetFloat("_BlurOffsetY", blurY / Mathf.Pow(2, i));
			rt = _rtBs[i-1];
			Graphics.Blit(rtS, rt, _blurShaderMat);
			rtS = rt;
		}
		return rt;
	}
}
