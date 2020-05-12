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
        Animator animator = turn.actor.GetComponentInChildren<Animator>();
        Movement m = turn.actor.GetComponent<Movement>();
        animator.SetBool("isMoving", true);
        yield return StartCoroutine(m.Traverse(owner.currentTile));
        animator.SetBool("isMoving", false);
        turn.hasUnitMoved = true;
        owner.ChangeState<CommandSelectionState>();
    }
}
