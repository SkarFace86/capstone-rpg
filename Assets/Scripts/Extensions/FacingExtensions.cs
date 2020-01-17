using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FacingExtensions
{
    public static Facings GetFacing(this Unit attacker, Unit target)
    {
        Vector2 targetDirection = target.dir.GetNormal();
        Vector2 approachDirection = ((Vector2) (target.tile.pos - attacker.tile.pos)).normalized;
        float dot = Vector2.Dot(approachDirection, targetDirection);
        if (dot >= 0.45f)
            return Facings.Back;
        if (dot <= -0.45f)
            return Facings.Front;
        return Facings.Side;
    }
}
