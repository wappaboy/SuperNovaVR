using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class KitControllerRendererBase : KitControllerBase, ITextureUpdatable
{
	protected Material _material;

	protected virtual void Awake() {
		_material = GetComponent<Renderer>().material;
	}
	
	public virtual void SetTexture(Texture tex) {
		if (_material.mainTexture != tex) {
			_material.mainTexture = tex;	
		}
		Init(tex);
	}
	
	public virtual void UpdateRender() {
		_material.SetTexture("_MainTex", GetRender());
	}
}
