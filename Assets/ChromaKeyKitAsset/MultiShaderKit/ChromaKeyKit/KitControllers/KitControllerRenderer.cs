using UnityEngine;

public class KitControllerRenderer : KitControllerRendererBase
{
	protected override void Awake() {
		base.Awake();
	}

	// Use this for initialization
	void Start() {

		var tex = _material.GetTexture("_MainTex");
		if (tex != null) {
			SetTexture(tex);
		} else {
			if (_material.name == "Default-Material (Instance)") {
				Debug.LogWarning("KitControllerRenderer: Create other material instead Default-Material");
			}
			Debug.LogError("KitControllerRenderer: Material requires mainTexture (propertyName = _MainTex)");
		}
	}

	// Update is called once per frame
	void Update() {
		UpdateRender();
    }
}
