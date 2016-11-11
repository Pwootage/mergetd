using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour {
	public AudioClip bulletDie;
	public AudioClip enemyDie;
	public AudioClip selectBlip;
	public AudioClip[] music;
	private AudioSource source;

	void Start() {
		source = GetComponent<AudioSource>();
		PlayNextSong();
	}

	void Update() {
	
	}

	public void playBulletDie() {
		source.PlayOneShot(bulletDie);
	}

	public void playEnemyDie() {
		source.PlayOneShot(enemyDie);
	}

	public void playSelectBlip() {
		source.PlayOneShot(selectBlip);
	}

	void PlayNextSong(){
		AudioClip song = music[Random.Range(0, music.Length)];
		source.PlayOneShot(song);
		Invoke("PlayNextSong", song.length);
	}
}
