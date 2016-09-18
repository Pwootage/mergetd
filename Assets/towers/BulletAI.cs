using UnityEngine;
using System.Collections;

public class BulletAI : MonoBehaviour {
    public float speed = 10;
    public float damage = 10;
    public GameObject target;

    void Start() {
    }

    void Update() {
        if (target == null) {
            Destroy(this.gameObject);
        } else {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target.transform.position,
                speed * Time.deltaTime);
        }
    }

	void OnTriggerEnter2D(Collider2D other) {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();
        if (enemy != null) {
            enemy.damage(damage);
            Destroy(this.gameObject);
        }
    }
}