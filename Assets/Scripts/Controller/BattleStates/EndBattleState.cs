using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This state only reloads the first level for now.
 * Change it up so it goes to another level or cutscene or whatever
 */
public class EndBattleState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        Application.LoadLevel(0);
    }
}
