using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public interface TowerStats {
    string description();

    int getCost();

    float getRange();
    float getRateOfFire();
    float getDamage();

    int getPierceCount();
    bool canHitSameTargetMultipleTimes();

    float getProjectileSpeed();

    float getSplashRadius();
    float getSplashDamageMultiplier();

    float getHomingStrength();
}

[Serializable]
public class TowerStatsBasic : TowerStats {
    public int cost = 10;
    public float range = 4;
    public float rateOfFire = 1;
    public float damage = 5;
    public int pierceCount = 0;
    public bool hitSameTargetMultipleTimes = false;
    public float projectileSpeed = 12;
    public float splashRadius = 0;
    public float splashDamageMultiplier = 0;
    [Range(0, 30)] public float homingStrength = 0;

    public string description() {
        return "Tower base stats";
    }

    public int getCost() {
        return cost;
    }

    public float getRange() {
        return range;
    }

    public float getRateOfFire() {
        return rateOfFire;
    }

    public float getDamage() {
        return damage;
    }

    public int getPierceCount() {
        return pierceCount;
    }

    public bool canHitSameTargetMultipleTimes() {
        return hitSameTargetMultipleTimes;
    }

    public float getProjectileSpeed() {
        return projectileSpeed;
    }

    public float getSplashRadius() {
        return splashRadius;
    }

    public float getSplashDamageMultiplier() {
        return splashDamageMultiplier;
    }

    public float getHomingStrength() {
        return homingStrength;
    }
}

public interface TowerStatModifier {
    TowerStats applyModifier(TowerStats toModify);
}
