using UnityEngine;

[RequireComponent(typeof(KitControllerBase))]
public class ChromaKey_Alpha_General : KitComponentBase
{
	public Color keyColor = Color.green;
	[Range(0.0f, 1.0f), Tooltip("Chroma max difference")]
	public float dChroma = 0.5f;
	[Range(0.0f, 1.0f), Tooltip("Chroma tolerance")]
	public float dChromaT = 0.05f;
	[Range(0.0f, 1.0f), Tooltip("Luma max difference")]
	public float dLuma = 0.5f;
	[Range(0.0f, 1.0f), Tooltip("Luma tolerance")]
	public float dLumaT = 0.05f;

	[Tooltip("Update shader properties in play mode")]
	public bool isUpdateProperties = true;

	[Tooltip("ChromaKey_Alpha_General.shader")]
	public Shader chromaKeyShader = null;

	private Color _keyColor;
	private float _dChroma = -1;
	private float _dChromaT = -1;
	private float _dLuma = -1;
	private float _dLumaT = -1;
	private RenderTexture _rtC; //render texture Chroma
	private Material _shaderMat;

	void Awake() {
		if (chromaKeyShader == null) {
			chromaKeyShader = Shader.Find("ChromaKeyKit/ChromaKey/ChromaKey_Alpha_General");
		}
	}

	// Use this for initialization
	void Start() {
		_shaderMat = createMaterial(chromaKeyShader);
		updateShaderProperties();
	}

	private void updateShaderProperties() {
		if (_keyColor != keyColor) {
			_keyColor = keyColor;
			_shaderMat.SetColor("_KeyColor", _keyColor);
		}
		if (_dChroma != dChroma) {
			_dChroma = dChroma;
			_shaderMat.SetFloat("_DChroma", _dChroma);
		}
		if (_dChromaT != dChromaT) {
			_dChromaT = dChromaT;
			_shaderMat.SetFloat("_DChromaT", _dChromaT);
		}
		if (_dLuma != dLuma) {
			_dLuma = dLuma;
			_shaderMat.SetFloat("_DLuma", _dLuma);
		}
		if (_dLumaT != dLumaT) {
			_dLumaT = dLumaT;
			_shaderMat.SetFloat("_DLumaT", _dLumaT);
		}
	}

	override public RenderTexture getRender(RenderTexture rt_src) {
		if(_rtC == null) {
			_rtC = new RenderTexture(_textureWidth, _textureHeight, 0);
		}
		if (isUpdateProperties) {
			updateShaderProperties();
		}
		_rtC.DiscardContents();
		Graphics.Blit(_tA, _rtC); //clear rt
		Graphics.Blit(rt_src, _rtC, _shaderMat);
        return _rtC;
	}
}