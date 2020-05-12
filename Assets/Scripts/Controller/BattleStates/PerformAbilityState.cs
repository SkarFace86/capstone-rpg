using UnityEngine;
using System.Collections;

public class PerformAbilityState : BattleState
{
    private Animator animator;
    public override void Enter()
    {
        base.Enter();
        animator = turn.actor.GetComponentInChildren<Animator>();
        turn.hasUnitActed = true;
        if (turn.hasUnitMoved)
            turn.lockMove = true;
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        // TODO play animations, etc
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(2);
        
        ApplyAbility();
        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(2f);
        if (IsBattleOver())
        {
            owner.ChangeState<CutSceneState>();
            //Debug.Log("GAME OVER");
        }
        else if (!UnitHasControl())
            owner.ChangeState<SelectUnitState>();
        else if (turn.hasUnitMoved)
            owner.ChangeState<EndFacingState>();
        else
            owner.ChangeState<CommandSelectionState>();
    }

    bool UnitHasControl()
    {
        return turn.actor.GetComponentInChildren<KnockOutStatusEffect>() == null;
    }

    void ApplyAbility()
    {
        turn.ability.Perform(turn.targets);
    }
}