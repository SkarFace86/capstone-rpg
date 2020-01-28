using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxAbilityRange : AbilityRange
{
    // defines the min number of tiles away from the user (for archers etc.)
    public int minDistance = 0;
    public override List<Tile> GetTilesInRange(Board board)
    {
        return board.Search(unit.tile, minDistance, ExpandSearch);
    }

    bool ExpandSearch(Tile from, Tile to)
    {
        return (from.distance + 1) <= horizontal && 
               Mathf.Abs(to.height - unit.tile.height) <= vertical;
    }
}
