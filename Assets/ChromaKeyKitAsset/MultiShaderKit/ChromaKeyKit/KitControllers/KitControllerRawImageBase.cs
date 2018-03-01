using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class KitControllerRawImageBase : KitControllerBase, ITextureUpdatable
{
	protected RawImage _rawImage;

	protected virtual void Awake() {
		_rawImage = GetComponent<RawImage>();
	}

	public virtual void SetTexture(Texture tex) {
		if (_rawImage.texture != tex) {
			_rawImage.texture = tex;
		}
		Init(tex);
	}

	public virtual void UpdateRender() {
		_rawImage.texture = GetRender();
	}

}
