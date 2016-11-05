using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class EnemyStats {
	public float health = 20;
	public float speed = 1;
	public int value = 3;
}

[Serializable]
public class EnemyEffect {
	public float slowMultiplier = 1;
	public float damagePerSecond = 0;
	public float duration = 0;

	public EnemyEffect clone() {
		return (EnemyEffect)this.MemberwiseClone();
	}
}

public class EnemyAI : MonoBehaviour {
	public EnemyStats stats = new EnemyStats();
	public Queue<Vector2> path = new Queue<Vector2>();
	public List<EnemyEffect> effects = new List<EnemyEffect>();
	private GameState state;
	private float damageTaken = 0;

	void Start() {
		state = GameState.FindInScene();
	}

	void Update() {
		float deltaTime = Time.deltaTime;
		float speed = stats.speed;
		// Process Effects
		foreach (EnemyEffect effect in effects) {
			effect.duration -= deltaTime;
			this.damage(effect.damagePerSecond * deltaTime);
			speed *= effect.slowMultiplier;
		}
		effects = effects.Where(effect => effect.duration > 0).ToList();


		if (damageTaken > stats.health) {
			state.GiveMoney(stats.value);
			Destroy(this.gameObject);
		} else if (path.Count > 0) {
			Vector2 target = path.Peek();
			if ((new Vector2(transform.position.x, transform.position.y) - target).magnitude < 0.1) {
				Debug.Log("Reached waypoint");
				path.Dequeue();
				Update();
			} else {
                RotateTowards(target);
				transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		BaseController baseController = other.gameObject.GetComponent<BaseController>();
		if (baseController != null) {
			Debug.Log("Hit a base!");
			Destroy(this.gameObject);
			state.TakeDamage(1);
		}
	}

	public void damage(float f) {
		damageTaken += f;
	}

    public void RotateTowards(Vector2 targetPos) {
        float rotateSpeed = 1;
        targetPos.y = transform.position.y;
        var rotation = Quaternion.LookRotation(targetPos - (Vector2)transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
    }
}