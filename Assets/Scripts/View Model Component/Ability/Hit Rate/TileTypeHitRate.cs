using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TileTypeHitRate : HitRate
{
    public override int Calculate(Tile target)
    {
        if (target.content == null)
            return Final(0);
        else
            return 0;
    }
}
