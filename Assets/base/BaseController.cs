using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BaseController : MonoBehaviour {
	public Light light;
	public GameObject ring;

	void Start() {
	}

	void Update() {
		float t = Time.realtimeSinceStartup * 5;
		float x = Mathf.Cos(t);
		float y = Mathf.Sin(t);
		light.transform.localPosition = new Vector3(x * 0.15f, y * 0.15f, -0.4f);
		ring.transform.localRotation = Quaternion.Euler(y * 30f - 90f, 0, 0) * Quaternion.Euler(new Vector3(0, x * 30f, 0));
	}
}