using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

	List<EnemyEffect> getEffects();
}

[Serializable]
public class TowerStatsBasic : TowerStats {
	public string descriptionStr = "Basic Tower";
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
	public List<EnemyEffect> effects = new List<EnemyEffect>();

    public TowerStatsBasic() {
    }

	public TowerStatsBasic(String description,int cost, float range, float rateOfFire, float damage, int pierceCount, bool hitSameTargetMultipleTimes, float projectileSpeed, float splashRadius, float splashDamageMultiplier, float homingStrength, List<EnemyEffect> effects) {
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
		this.effects = effects;
    }

    public TowerStatsBasic(TowerStats toCopy) {
        this.descriptionStr = toCopy.description();
        this.cost = toCopy.getCost();
        this.range = toCopy.getRange();
        this.rateOfFire = toCopy.getRateOfFire();
        this.damage = toCopy.getDamage();
        this.pierceCount = toCopy.getPierceCount();
        this.hitSameTargetMultipleTimes = toCopy.canHitSameTargetMultipleTimes();
		this.projectileSpeed = toCopy.getProjectileSpeed();
        this.splashRadius = toCopy.getSplashRadius();
        this.splashDamageMultiplier = toCopy.getSplashDamageMultiplier();
        this.homingStrength = toCopy.getHomingStrength();
		this.effects = new List<EnemyEffect>();
		foreach (EnemyEffect effect in toCopy.getEffects()) {
			this.effects.Add(effect.clone());
		}
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

	public List<EnemyEffect> getEffects() {
		return effects;
	}
}

public abstract class TowerStatModifier {
    public abstract TowerStats applyModifier(TowerStats toModify);
	public abstract string getDescription();
}

public class PercentAttackDamageBoost: TowerStatModifier {
    private readonly float amount;

	public PercentAttackDamageBoost(float amount) {
        this.amount = amount;
    }

    public override TowerStats applyModifier(TowerStats toModify) {
        TowerStatsBasic ret = new TowerStatsBasic(toModify);
		ret.damage *= (1f + amount);
		ret.cost += getCost();
        return ret;
    }

	public int getCost() {
		return Mathf.CeilToInt(amount * 100f * 0.5f);
	}

	public override string getDescription() {
		return "+" + (amount * 100f) + "% damage, +" + getCost() + " cost";
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
		ret.cost += getCost();
        return ret;
    }
		
	public int getCost() {
		return Mathf.CeilToInt(amount * 20);
	}

	public override string getDescription() {
		return "+" + amount + " range, +" + getCost() + " cost";
	}
}


public class PercentAttackSpeedBoost : TowerStatModifier {
    private readonly float amount;

    public PercentAttackSpeedBoost(float amount) {
        this.amount = amount;
    }

    public override TowerStats applyModifier(TowerStats toModify) {
        TowerStatsBasic ret = new TowerStatsBasic(toModify);
		ret.rateOfFire *= (1 - amount);
		ret.cost += getCost();
        return ret;
	}

	public int getCost() {
		return Mathf.CeilToInt(amount * 100f * 1f);
	}

	public override string getDescription() {
		return "+" + (amount * 100f) + "% attack speed, +" + getCost() + " cost";
	}
}

public class CostReduction: TowerStatModifier {
	private readonly int amount;

	public CostReduction(int amount) {
		this.amount = amount;
	}

	public override TowerStats applyModifier(TowerStats toModify) {
		TowerStatsBasic ret = new TowerStatsBasic(toModify);
		if (ret.cost > 15) {
			ret.cost -= amount;
		}
		return ret;
	}

	public override string getDescription() {
		return -amount + " cost (if >15)";
	}
}

public class TilePercentAttackDamageBoost: TowerStatModifier {
	private readonly float amount;

	public TilePercentAttackDamageBoost(float amount) {
		this.amount = amount;
	}

	public override TowerStats applyModifier(TowerStats toModify) {
		TowerStatsBasic ret = new TowerStatsBasic(toModify);
		ret.damage *= amount;
		return ret;
	}
		
	public override string getDescription() {
		return "Tile Stat Boost";
	}
}

public class TilePercentAttackSpeedBoost: TowerStatModifier {
	private readonly float amount;

	public TilePercentAttackSpeedBoost(float amount) {
		this.amount = amount;
	}

	public override TowerStats applyModifier(TowerStats toModify) {
		TowerStatsBasic ret = new TowerStatsBasic(toModify);
		ret.rateOfFire *= amount;
		return ret;
	}

	public override string getDescription() {
		return "Tile Stat Boost";
	}
}

public class TilePercentAttackRangeBoost: TowerStatModifier {
	private readonly float amount;

	public TilePercentAttackRangeBoost(float amount) {
		this.amount = amount;
	}

	public override TowerStats applyModifier(TowerStats toModify) {
		TowerStatsBasic ret = new TowerStatsBasic(toModify);
		ret.range *= amount;
		return ret;
	}

	public override string getDescription() {
		return "Tile Stat Boost";
	}
}
