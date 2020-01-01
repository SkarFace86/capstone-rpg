using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxValueModifier : ValueModifier
{
    public float max;
    public MaxValueModifier(int sortOrder, float max) : base(sortOrder)
    {
        this.max = max;
    }
    public override float Modify(float value)
    {
        return Mathf.Max(value, max);
    }
}
