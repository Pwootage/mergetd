using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BaseController : MonoBehaviour {
	public Text livesUI;
	public Text gameOverUI;
	public int maxHP = 20;
	public int currentHP = 20;

	public Light light;
	public GameObject ring;

	void Start() {
		currentHP = maxHP;
		livesUI.text = "Lives: " + currentHP + "/" + maxHP;
	}

	void Update() {
		float t = Time.realtimeSinceStartup * 5;
		float x = Mathf.Cos(t);
		float y = Mathf.Sin(t);
		light.transform.localPosition = new Vector3(x * 0.15f, y * 0.15f, -0.4f);
		ring.transform.localRotation = Quaternion.Euler(y * 30f - 90f, 0, 0) * Quaternion.Euler(new Vector3(0, x * 30f, 0));
	}

	public void TakeDamage() {
		currentHP -= 1;
		livesUI.text = "Lives: " + currentHP + "/" + maxHP;
		if (currentHP == 0) {
			gameOverUI.gameObject.SetActive(true);
			gameOverUI.text = "You lose!";
			Destroy(gameObject);
		}
	}
}