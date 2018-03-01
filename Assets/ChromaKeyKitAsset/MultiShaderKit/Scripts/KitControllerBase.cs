using UnityEngine;
using System.Collections.Generic;

abstract public class KitControllerBase : MonoBehaviour
{
	public List<KitComponentBase> components = new List<KitComponentBase>();

	private Texture _tA; // Texture clear (Alpha = 0)
	public Texture tA {
		get { return _tA; }
	}
	private RenderTexture _rtS; // RenderTexture source	(_MainTex)
	public RenderTexture rtS {
		get { return _rtS; }
	}
	protected Texture _texture = null;
	public Texture texture {
		get { return _texture; }
	} 
	
	protected void Init(Texture tex) {
		_texture = tex;
		
		_rtS = new RenderTexture(_texture.width, _texture.height, 0);

		Texture2D tex2D = new Texture2D(1, 1);
		tex2D.SetPixels(new Color[1] { Color.clear });
		tex2D.Apply();
		_tA = tex2D;

		foreach (var component in components) {
			if(component != null) {
				component.setController(this);
			} else {
				Debug.LogError("ChromaKeyKit: Kit component should not be null");
			}
		}
	}
	
	protected RenderTexture GetRender() {
		_rtS.DiscardContents();
		Graphics.Blit(_texture, _rtS);
		
		RenderTexture rt = _rtS;
		foreach (var component in components) {
			if (component != null && component.enabled) {
				rt = component.getRender(rt);
			}
		}
		return rt;
	}
}
