using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour {
    #region Constants

    private const int DEFAULT_RANGE = 5;
    private const int DEFAULT_DAMAGE = 1;
    private const int DEFAULT_SPEED = 5;

    #endregion

    #region Editor Properties

    // Basic stats
    public float Range = DEFAULT_RANGE;
    public float Damage = DEFAULT_RANGE;
    public float Speed = DEFAULT_SPEED;

    // Homing
    [Range(0, 30)] public float HomingStrength;

    // Slow Effect
    public float SlowMultiplier = 0;
    public float SlowDuration = 0;

    // Piercing
    public int PierceCount = 0;
    public bool AllowMultipleHitsOnSameEnemy = false;

    // Splash Effect
    public float SplashDamageMultiplier = 0;
    public float SplashRadius = 0;

    // DoT Effect
    public float DoTDamage = 0;
    public float DoTDuration = 0;

    #endregion

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

    private HashSet<EnemyAI> enemiesHit = new HashSet<EnemyAI>();

    #endregion

    #region Methods

    public Projectile() {
    }

    public void Start() {
    }


    public void Update() {
        //Homing
        if (Target != null && HomingStrength > 0) {
            Vector2 directionToTarget = (Target.transform.position - gameObject.transform.position).normalized;
            Direction = Vector2.Lerp(Direction, directionToTarget, HomingStrength * Time.deltaTime);
        }

        Vector3 moveAmount = Direction * Speed * Time.deltaTime;
        gameObject.transform.position += moveAmount;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        var enemy = other.gameObject.GetComponent<EnemyAI>();
        if (enemy != null && (AllowMultipleHitsOnSameEnemy || !enemiesHit.Contains(enemy))) {
            enemy.damage(Damage);
            enemiesHit.Add(enemy);
            if (PierceCount > 0) {
                PierceCount--;
            } else {
                Destroy(this.gameObject);
            }
        }
    }

    public void OnDestroy() {
        //TODO
        //If splash_damage > 0 create "explosion" in a circle around the projectile's current position with a radius of splash_radius
        //destroy the Projectile
    }

    #endregion
}
