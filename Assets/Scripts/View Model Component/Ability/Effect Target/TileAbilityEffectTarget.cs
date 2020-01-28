using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAbilityEffectTarget : AbilityEffectTarget
{
    public override bool IsTarget(Tile tile)
    {
        if (tile == null)
            return false;

        return true;
    }
}
