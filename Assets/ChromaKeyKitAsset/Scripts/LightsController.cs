using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightsController : MonoBehaviour {

	public List<Light> lights = null;

	private int _currentIndex = 0;

	void Start() {
		foreach (var light in lights) {
			light.enabled = false;
		}
		StartCoroutine(RepeatingFunction());
	}


	IEnumerator RepeatingFunction() {
		while (true) {
			lights[_currentIndex].enabled = false;
			_currentIndex++;
			if (_currentIndex > lights.Count - 1) {
				_currentIndex = 0;
            }
			lights[_currentIndex].enabled = true;
			yield return new WaitForSeconds(0.5f);
		}
	}
}
