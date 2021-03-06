﻿using UnityEngine;
using System.Collections;

/**
 * Base behavior class. Inherit this when implementing flocking, wander, etc.
 */
public abstract class BaseBehavior : BaseStudent {
    public float scale = 0;
    public bool vital = false;
    public abstract Vector3 ComputeVelocity();
}