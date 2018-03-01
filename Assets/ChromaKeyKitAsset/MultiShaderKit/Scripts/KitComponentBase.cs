using UnityEngine;

abstract public class KitComponentBase : MonoBehaviour
{
	protected KitControllerBase _kitController;

	protected int _textureWidth = 4;
	protected int _textureHeight = 4;

	protected Texture _tA; // Texture clear (Alpha = 0)
	protected RenderTexture _rtS; // RenderTexture controller source (_MainTex)

	protected Material createMaterial(Shader shader) {
		Material material = new Material(shader);
		material.hideFlags = HideFlags.DontSave;
		return material;
	}

	public void setController(KitControllerBase kitController) {
		_kitController = kitController;

		_textureWidth = _kitController.texture.width;
		_textureHeight = _kitController.texture.height;

		_tA = _kitController.tA;
		_rtS = _kitController.rtS;
	}

	virtual public RenderTexture getRender(RenderTexture rt_src) {
		return rt_src;
	}
}