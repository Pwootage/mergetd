using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GreenMechAI : EnemyAI  {
	override public float getSlowModifier(float originalModifier) {
		return originalModifier * 0.8f; //This makes it more effective (it's a multiplier)
	}
}