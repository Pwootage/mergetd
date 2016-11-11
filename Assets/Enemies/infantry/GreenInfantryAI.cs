using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GreenInfantryAI : EnemyAI {
	override public void dotDamage(float f) {
		damage(f * 1.2f);
	}
}
