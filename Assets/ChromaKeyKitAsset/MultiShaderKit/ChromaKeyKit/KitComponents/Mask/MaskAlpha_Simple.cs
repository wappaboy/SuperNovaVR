using UnityEngine;

[RequireComponent(typeof(KitControllerBase))]
public class MaskAlpha_Simple : KitComponentBase
{
	[Tooltip("Update shader properties in play mode")]
	public bool isUpdateProperties = true;

	[Tooltip("MaskAlpha_Simple.shader")]
	public Shader maskShader = null;
	
	private RenderTexture _rtM; //render texture Mask
	private Material _shaderMat;

	void Awake() {
		if (maskShader == null) {
			maskShader = Shader.Find("ChromaKeyKit/Mask/MaskAlpha_Simple");
		}
	}

	// Use this for initialization
	void Start() {
		_shaderMat = createMaterial(maskShader);
		updateShaderProperties();
    }

	private void updateShaderProperties() {
		//
	}

	override public RenderTexture getRender(RenderTexture rt_src) {
		if (_rtM == null) {
			_rtM = new RenderTexture(_textureWidth, _textureHeight, 0);
		}
		//if (isUpdateProperties) {
		//	updateShaderProperties();
		//}
		_shaderMat.SetTexture("_MaskTex", rt_src);
		_rtM.DiscardContents();
		Graphics.Blit(_tA, _rtM);
		Graphics.Blit(_rtS, _rtM, _shaderMat);
		return _rtM;
	}
}
