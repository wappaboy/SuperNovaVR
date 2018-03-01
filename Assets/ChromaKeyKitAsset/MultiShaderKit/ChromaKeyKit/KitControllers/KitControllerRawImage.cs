using UnityEngine;

public class KitControllerRawImage : KitControllerRawImageBase
{
	protected override void Awake() {
		base.Awake();
	}

	// Use this for initialization
	void Start() {

		var tex = _rawImage.texture;
		if (tex != null) {
			if (_rawImage.material != _rawImage.defaultMaterial) {
				Debug.LogWarning("KitControllerRawImage: Simultaneous use of texture and material may work unstable. Use texture(at RawImage) and KitController(with kitComponents) OR material(at RawImage) and KitShader at this material");
			}
			SetTexture(tex);
		} else {
			Debug.LogError("KitControllerRawImage: RawImage requires texture");
		}
	}

	// Update is called once per frame
	void Update() {
		UpdateRender();
	}

}
