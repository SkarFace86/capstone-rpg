using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SquareAbilityArea : AbilityArea
{
    public int horizontal;
    public int vertical;
    private List<Tile> tiles;

    public override List<Tile> GetTilesInArea(Board board, Point pos)
    {
        if (horizontal == 0)
            return null;

        tiles = new List<Tile>();
        for (int i = -horizontal; i < horizontal + 1; i++)
        {
            for (int j = -horizontal; j < horizontal + 1; j++)
            {
                if(board.GetTile(pos + new Point(i, j)) != null)
                    tiles.Add(board.GetTile(pos + new Point(i, j)));
            }
        }
        tiles.Remove(board.GetTile(pos));

        return tiles;
    }

}
