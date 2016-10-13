using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerAI : MonoBehaviour {
    public GameObject turret;
    public GameObject bullet;
    public TowerStatsBasic stats;
    public List<TowerStatModifier> statModifiers = new List<TowerStatModifier>();

    private float attackTimer;

	void Start () {
	    attackTimer = stats.getRateOfFire();
	}
	
	void Update () {
	    if (attackTimer <= 0) {
	        Collider2D[] objects = Physics2D.OverlapCircleAll(gameObject.transform.position, stats.getRange());
	        foreach (Collider2D obj in objects) {
	            EnemyAI enemy = obj.gameObject.GetComponent<EnemyAI>();
	            if (enemy != null) {
	                GameObject newBullet = GameObject.Instantiate(bullet);
	                newBullet.transform.position = gameObject.transform.position;

	                Projectile projectile = newBullet.GetComponent<Projectile>();
                    projectile.Target = obj.gameObject;

	                attackTimer = stats.getRateOfFire();
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
