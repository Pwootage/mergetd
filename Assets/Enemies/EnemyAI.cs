using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class EnemyStats {
	public float health = 20;
	public float speed = 1;
	public float value = 3;

	public EnemyStats Clone(){
		return (EnemyStats)this.MemberwiseClone();
	}
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
	protected GameState state;
	protected float damageTaken = 0;
	public float rotation;

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
		}
		if (effects.Count() > 0) {
			speed *= effects.Select(effect => effect.slowMultiplier).Min();
		}
		effects = effects.Where(effect => effect.duration > 0).ToList();

		transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
		if (damageTaken > stats.health) {
			state.GiveMoney(stats.value);
			Destroy(this.gameObject);
		} else if (path.Count > 0) {
			Vector2 target = path.Peek();
			if ((new Vector2(transform.position.x, transform.position.y) - target).magnitude < 0.1) {
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
			Destroy(this.gameObject);
			state.TakeDamage(1);
		}
	}

	public void damage(float f) {
		damageTaken += f;
	}

    public void RotateTowards(Vector2 targetPos) {
        float rotateSpeed = 3;
		Vector3 look = transform.position - new Vector3(targetPos.x, targetPos.y, 0);
		float from = rotation;
		float to = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg - 90f;

		from = (from + 360) % 360;
		rotation = from;
		to = (to + 360) % 360;

		float left = (360 - from) + to;
		float right = from - to;
		if(from < to)  {
			if(to > 0) {
				left = to - from;
				right = (360 - to) + from;
			} else {
				left = (360 - to) + from;
				right = to - from;
			}
		}

		if (left < rotateSpeed || right < rotateSpeed) {
			rotation = to;
		} else if (left < right) {
			rotation = rotation - rotateSpeed;
		} else {
			rotation = rotation + rotateSpeed;	
		}
    }
}