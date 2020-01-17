using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultValueModifier : ValueModifier
{
    public readonly float toMultiply;
    public MultValueModifier(int sortOrder, float toMultiply) : base(sortOrder)
    {
        this.toMultiply = toMultiply;
    }
    public override float Modify(float fromValue, float toValue)
    {
        return toValue * toMultiply;
    }
}
