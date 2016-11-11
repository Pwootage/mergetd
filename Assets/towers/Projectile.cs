using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Projectile : MonoBehaviour {
    #region Non-Editor Properties

    // Movement (not set in editor)
    public Vector2 Direction {
        get { return _Direction; }
        set { _Direction = value.normalized; }
    }

    private Vector2 _Direction = Vector2.up;

    public GameObject Target {
        get {return _Target; }
        set {
            _Target = value;
            Direction = value.transform.position - gameObject.transform.position;
        }
    }

    private GameObject _Target = null;

    public TowerStats Stats;

    private HashSet<EnemyAI> enemiesHit = new HashSet<EnemyAI>();
    private int pierceCount = 0;

    #endregion

    #region Methods

    public Projectile() {
    }

    public void Start() {
    }

    public void Update() {
        //Homing
        if (Target != null && Stats.getHomingStrength() > 0) {
            Vector2 directionToTarget = (Target.transform.position - gameObject.transform.position).normalized;
            Direction = Vector2.Lerp(Direction, directionToTarget, Stats.getHomingStrength() * Time.deltaTime);
        }

        Vector3 moveAmount = Direction * Stats.getProjectileSpeed() * Time.deltaTime;
        gameObject.transform.position += moveAmount;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        var enemy = other.gameObject.GetComponent<EnemyAI>();
        if (enemy == null) {
            return;
        }

        if (Stats.canHitSameTargetMultipleTimes() || !enemiesHit.Contains(enemy)) {
            enemy.damage(Stats.getDamage());
			enemy.effects.AddRange(Stats.getEffects().Select(effect => effect.clone()));
            enemiesHit.Add(enemy);
            pierceCount++;
        }

        if (pierceCount > Stats.getPierceCount()) {
            Destroy(this.gameObject);
        }
    }

    public void OnDestroy() {
		GameState.FindInScene().getAudioPlayer().playBulletDie();
        //TODO
        //If splash_damage > 0 create "explosion" in a circle around the projectile's current position with a radius of splash_radius
        //destroy the Projectile
    }

    #endregion
}
