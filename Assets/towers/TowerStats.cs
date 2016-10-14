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
    public string descriptionStr = "Basic Tower";

    public TowerStatsBasic() {
    }

    public TowerStatsBasic(String description,int cost, float range, float rateOfFire, float damage, int pierceCount, bool hitSameTargetMultipleTimes, float projectileSpeed, float splashRadius, float splashDamageMultiplier, float homingStrength) {
        this.descriptionStr = description;
        this.cost = cost;
        this.range = range;
        this.rateOfFire = rateOfFire;
        this.damage = damage;
        this.pierceCount = pierceCount;
        this.hitSameTargetMultipleTimes = hitSameTargetMultipleTimes;
        this.projectileSpeed = projectileSpeed;
        this.splashRadius = splashRadius;
        this.splashDamageMultiplier = splashDamageMultiplier;
        this.homingStrength = homingStrength;
    }

    public TowerStatsBasic(TowerStats toCopy) {
        this.descriptionStr = toCopy.description();
        this.cost = toCopy.getCost();
        this.range = toCopy.getRange();
        this.rateOfFire = toCopy.getRateOfFire();
        this.damage = toCopy.getDamage();
        this.pierceCount = toCopy.getPierceCount();
        this.hitSameTargetMultipleTimes = toCopy.canHitSameTargetMultipleTimes();
        this.splashRadius = toCopy.getSplashRadius();
        this.splashDamageMultiplier = toCopy.getSplashDamageMultiplier();
        this.homingStrength = toCopy.getHomingStrength();
    }

    public string description() {
        return descriptionStr;
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

public abstract class TowerStatModifier {
    public abstract TowerStats applyModifier(TowerStats toModify);
}

public class FlatAttackDamageBoost: TowerStatModifier {
    private readonly float amount;

    public FlatAttackDamageBoost(float amount) {
        this.amount = amount;
    }

    public override TowerStats applyModifier(TowerStats toModify) {
        TowerStatsBasic ret = new TowerStatsBasic(toModify);
        ret.damage += amount;
        ret.cost += Mathf.CeilToInt(amount * 5);
        return ret;
    }
}

public class FlatRangeBoost : TowerStatModifier {
    private readonly float amount;

    public FlatRangeBoost(float amount) {
        this.amount = amount;
    }

    public override TowerStats applyModifier(TowerStats toModify) {
        TowerStatsBasic ret = new TowerStatsBasic(toModify);
        ret.range += amount;
        ret.cost += Mathf.CeilToInt(amount * 20);
        return ret;
    }
}


public class PercentAttackSpeedBoost : TowerStatModifier {
    private readonly float amount;

    public PercentAttackSpeedBoost(float amount) {
        this.amount = amount;
    }

    public override TowerStats applyModifier(TowerStats toModify) {
        TowerStatsBasic ret = new TowerStatsBasic(toModify);
        ret.rateOfFire *= amount;
        ret.cost += Mathf.CeilToInt((1 - amount) * 20);
        return ret;
    }
}
