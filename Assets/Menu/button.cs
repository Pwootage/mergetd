using UnityEngine;
using System.Collections;

public class button : MonoBehaviour {
	public void playButton () {
		UnityEngine.SceneManagement.SceneManager.LoadScene("BasicLevel");
	}
}
