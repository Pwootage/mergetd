using UnityEngine;
using System.Collections;

public class TowerAI : MonoBehaviour {
    public GameObject turret;
    public GameObject bullet;
    public float attackSpeed = 1;
    public float range = 4;
    float attackTimer;

	void Start () {
	    attackTimer = attackSpeed;
	}
	
	void Update () {
	    if (attackTimer <= 0) {
	        Collider2D[] objects = Physics2D.OverlapCircleAll(gameObject.transform.position, range);
	        foreach (Collider2D obj in objects) {
	            EnemyAI enemy = obj.gameObject.GetComponent<EnemyAI>();
	            if (enemy != null) {
	                GameObject newBullet = GameObject.Instantiate(bullet);
	                newBullet.transform.position = gameObject.transform.position;
	                BulletAI bulletAi = newBullet.GetComponent<BulletAI>();
	                bulletAi.target = obj.gameObject;
	                attackTimer = attackSpeed;
					SetTurretLookAt(obj.gameObject);
	                break;
	            }
	        }
	    } else {
	        attackTimer -= Time.deltaTime;
	    }
	}

	void SetTurretLookAt(GameObject obj) {
		Vector3 look = transform.position - obj.transform.position;
		float rot = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg + 90f;
		turret.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rot));
	}
}
