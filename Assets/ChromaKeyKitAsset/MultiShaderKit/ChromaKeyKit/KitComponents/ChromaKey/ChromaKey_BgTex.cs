using UnityEngine;

[RequireComponent(typeof(KitControllerBase))]
public class ChromaKey_BgTex : KitComponentBase
{
    public Texture bgTex = null;
	public Color keyColor = Color.green;

	[Range(0.0f, 1.0f), Tooltip("Chroma max difference")]
	public float dChroma = 0.5f;
	[Range(0.0f, 1.0f), Tooltip("Chroma tolerance")]
	public float dChromaT = 0.05f;
	[Range(0.0f, 1.0f), Tooltip("Main(0) -> Bg(1)")]
	public float chroma = 0.5f;
	[Range(0.0f, 1.0f), Tooltip("Main(0) -> Bg(1)")]
	public float luma = 0.5f;
	[Range(0.0f, 1.0f), Tooltip("0 -> Chroma(1)")]
	public float saturation = 1.0f;
	[Range(0.0f, 1.0f), Tooltip("0 -> 1")]
	public float alpha = 1.0f;

	[Tooltip("Update shader properties in play mode")]
	public bool isUpdateProperties = true;

	[Tooltip("ChromaKey_BgTex.shader")]
	public Shader chromaKeyShader = null;

	private Texture _bgTex;
	private Color _keyColor;
	private float _dChroma = -1;
	private float _dChromaT = -1;
	private float _chroma = -1;
	private float _luma = -1;
	private float _saturation = -1;
	private float _alpha = -1;
	private RenderTexture _rtC; //render texture Chroma
	private Material _shaderMat;

	void Awake() {
		if (chromaKeyShader == null) {
			chromaKeyShader = Shader.Find("ChromaKeyKit/ChromaKey/ChromaKey_BgTex");
		}
	}

	// Use this for initialization
	void Start() {
		_shaderMat = createMaterial(chromaKeyShader);
		updateShaderProperties();
    }

	private void updateShaderProperties() {
		if (_bgTex != bgTex) {
			_bgTex = bgTex;
			_shaderMat.SetTexture("_BgTex", _bgTex);
		}
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
		if (_chroma != chroma) {
			_chroma = chroma;
			_shaderMat.SetFloat("_Chroma", _chroma);
		}
		if (_luma != luma) {
			_luma = luma;
			_shaderMat.SetFloat("_Luma", _luma);
		}
		if (_saturation != saturation) {
			_saturation = saturation;
			_shaderMat.SetFloat("_Saturation", _saturation);
		}
		if (_alpha != alpha) {
			_alpha = alpha;
			_shaderMat.SetFloat("_Alpha", _alpha);
		}
	}

	override public RenderTexture getRender(RenderTexture rt_src) {
		if (_rtC == null) {
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