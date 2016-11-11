using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RedInfantryAI : EnemyAI {
	override public void dotDamage(float f) {
		damage(f * 1.2f);
	}
}