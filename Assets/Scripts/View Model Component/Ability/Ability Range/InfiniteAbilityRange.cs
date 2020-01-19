using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteAbilityRange : AbilityRange
{
    public override bool positionOriented { get { return false; } }

    public override List<Tile> GetTilesInRange(Board board)
    {
        return new List<Tile>(board.tiles.Values);
    }
}
