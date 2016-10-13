using System;
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

}

[Serializable]
public class TowerStatsBasic : TowerStats {
    public int cost;
    public float range;
    public float rateOfFire;
    public float damage;
    public int pierceCount;
    public bool hitSameTargetMultipleTimes;

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
}


public interface  TowerStatModifier {
    TowerStats applyModifier(TowerStats toModify);
}

