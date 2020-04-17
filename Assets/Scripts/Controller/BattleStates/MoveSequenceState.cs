using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSequenceState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine("Sequence");
    }

    IEnumerator Sequence()
    {
        Animation anim = turn.actor.GetComponentInChildren<Animation>();
        Movement m = turn.actor.GetComponent<Movement>();
        if (anim)
            anim.Play("Move");
        yield return StartCoroutine(m.Traverse(owner.currentTile));
        if (anim)
            anim.Play("Idle");
        turn.hasUnitMoved = true;
        owner.ChangeState<CommandSelectionState>();
    }
}
