using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlaceUnitsState : BattleState
{
    protected string[] recipes;
    public override void Enter()
    {
        base.Enter();
        Debug.Log("GOT ghereherheh");
            recipes = new string[]
        {
            "Alaois",
            "Hania",
            "Kamau"
        };
    }
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        Debug.Log("GOT HERE");
        SelectTile(e.info + pos);
    }
}
