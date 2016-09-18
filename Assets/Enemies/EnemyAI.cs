using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour {
	public float speed = 1;
	public float health = 20;
	public int moneyValue = 3;
	public Queue<Vector2> path = new Queue<Vector2>();

	void Start() {
	}

	void Update() {
		if (health <= 0) {
			GameObject.Find("FloorController").GetComponent<TowerBuilder>().AddMoney(moneyValue);
			Destroy(this.gameObject);
		} else if (path.Count > 0) {
			Vector2 target = path.Peek();
			if ((new Vector2(transform.position.x, transform.position.y) - target).magnitude < 0.1) {
				Debug.Log("Reached waypoint");
				path.Dequeue();
				Update();
			} else {
				transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		BaseController baseController = other.gameObject.GetComponent<BaseController>();
		if (baseController != null) {
			Debug.Log("Hit a base!");
			Destroy(this.gameObject);
			baseController.TakeDamage();
		}
	}

	public void damage(float f) {
		health -= f;
	}
}