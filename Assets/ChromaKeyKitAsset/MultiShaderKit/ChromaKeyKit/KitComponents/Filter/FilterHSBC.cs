using UnityEngine;

[RequireComponent(typeof(KitControllerBase))]
public class FilterHSBC : KitComponentBase
{
	public Color baseColor = Color.white;
	[Range(0, 360)]
	public int hue = 0;
	[Range(-1.0f, 2.0f)]
	public float saturation = 0.0f;
	[Range(-1.0f, 10.0f)]
	public float brightness = 0.0f;
	[Range(0.0f, 10.0f)]
	public float contrast = 1.0f;

	[Tooltip("Update shader properties in play mode")]
	public bool isUpdateProperties = true;

	[Tooltip("FilterHSBC.shader")]
	public Shader filterShader = null;

	private Color _baseColor;
	private int _hue = -1;
	private float _saturation = -1;
	private float _brightness = -1;
	private float _contrast = -1;
	private RenderTexture _rtF; //render texture Filter
	private Material _shaderMat;

	void Awake() {
		if (filterShader == null) {
			filterShader = Shader.Find("ChromaKeyKit/Filter/FilterHSBC");
		}
	}

	// Use this for initialization
	void Start() {
		_shaderMat = createMaterial(filterShader);
		updateShaderProperties();
	}

	private void updateShaderProperties() {
		if (_baseColor != baseColor) {
			_baseColor = baseColor;
			_shaderMat.SetColor("_BaseColor", _baseColor);
		}
		if (_hue != hue) {
			_hue = hue;
			_shaderMat.SetFloat("_Hue", _hue);
		}
		if (_saturation != saturation) {
			_saturation = saturation;
			_shaderMat.SetFloat("_Saturation", _saturation);
		}
		if (_brightness != brightness) {
			_brightness = brightness;
			_shaderMat.SetFloat("_Brightness", _brightness);
		}
		if (_contrast != contrast) {
			_contrast = contrast;
			_shaderMat.SetFloat("_Contrast", _contrast);
		}
	}

	override public RenderTexture getRender(RenderTexture rt_src) {
        if (_rtF == null) {
			_rtF = new RenderTexture(_textureWidth, _textureHeight, 0);
		}
		if (isUpdateProperties) {
			updateShaderProperties();
		}
		
		Graphics.Blit(rt_src, _rtF); //clear rt
		Graphics.Blit(rt_src, _rtF, _shaderMat);
        return _rtF;
	}
}