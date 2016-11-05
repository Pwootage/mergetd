using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class GreenMechStats
{
    public float health = 40;
    public float speed = 3;
    public int value = 4;
}

[Serializable]
public class GreenMechEffect
{
    public float slowMultiplier = 1;
    public float damagePerSecond = 0;
    public float duration = 0;

    public GreenMechEffect clone()
    {
        return (GreenMechEffect)this.MemberwiseClone();
    }
}

public class GreenMechAI : MonoBehaviour
{
    public GreenMechStats stats = new GreenMechStats();
    public Queue<Vector2> path = new Queue<Vector2>();
    public List<GreenMechEffect> effects = new List<GreenMechEffect>();
    private GameState state;
    private float damageTaken = 0;

    void Start()
    {
        state = GameState.FindInScene();
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        float speed = stats.speed;
        // Process Effects
        foreach (GreenMechEffect effect in effects)
        {
            effect.duration -= deltaTime;
            this.damage(effect.damagePerSecond * deltaTime);
            speed *= effect.slowMultiplier;
        }
        effects = effects.Where(effect => effect.duration > 0).ToList();


        if (damageTaken > stats.health)
        {
            state.GiveMoney(stats.value);
            Destroy(this.gameObject);
        }
        else if (path.Count > 0)
        {
            Vector2 target = path.Peek();
            if ((new Vector2(transform.position.x, transform.position.y) - target).magnitude < 0.1)
            {
                Debug.Log("Reached waypoint");
                path.Dequeue();
                Update();
            }
            else
            {
                RotateTowards(target);
                transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        BaseController baseController = other.gameObject.GetComponent<BaseController>();
        if (baseController != null)
        {
            Debug.Log("Hit a base!");
            Destroy(this.gameObject);
            state.TakeDamage(1);
        }
    }

    public void damage(float f)
    {
        damageTaken += f;
    }

    //rotates enemy to face waypoint
    private void RotateTowards(Vector2 waypoint)
    {
        Vector3 newStartPosition = transform.position;
        Vector3 newEndPosition = waypoint;
        Vector3 newDirection = (newEndPosition - newStartPosition);

        float x = newDirection.x;
        float y = newDirection.y;
        float rotationAngle = Mathf.Atan2(y, x) * 180 / Mathf.PI;

        transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
    }
}