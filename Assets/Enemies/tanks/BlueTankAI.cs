﻿using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BlueTankAI : EnemyAI {
	override public float getSlowModifier(float originalModifier) {
		return 1; //immune
	}
}