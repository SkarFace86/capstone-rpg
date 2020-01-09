using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Note that this implementation doesn’t check whether or not to lock any menu entries,
 * but in a fully implemented game, there might be conditions which could. For example,
 * some sort of status ailment might prevent you from Attacking. Those sorts of scenarios
 * could be addressed in the LoadMenu method
 */
public class CategorySelectionState : BaseAbilityMenuState
{
    protected override void LoadMenu()
    {
        if (menuOptions == null)
        {
            menuTitle = "Action";
            menuOptions = new List<string>(3);
            menuOptions.Add("Attack");
            menuOptions.Add("White Magic");
            menuOptions.Add("Black Magic");
        }

        abilityMenuPanelController.Show(menuTitle, menuOptions);
    }

    protected override void Confirm()
    {
        switch (abilityMenuPanelController.selection)
        {
            case 0:
                Attack();
                break;
            case 1:
                SetCategory(0);
                break;
            case 2:
                SetCategory(1);
                break;
        }
    }

    protected override void Cancel()
    {
        owner.ChangeState<CommandSelectionState>();
    }

    void Attack()
    {
        //turn.hasUnitActed = true;
        //if (turn.hasUnitMoved)
        //    turn.lockMove = true;
        //owner.ChangeState<CommandSelectionState>();
        turn.ability = turn.actor.GetComponentInChildren<AbilityRange>().gameObject;
        owner.ChangeState<AbilityTargetState>();
    }

    void SetCategory(int index)
    {
        ActionSelectionState.category = index;
        owner.ChangeState<ActionSelectionState>();
    }

    public override void Enter()
    {
        base.Enter();
        statPanelController.ShowPrimary(turn.actor.gameObject);
    }

    public override void Exit()
    {
        base.Exit();
        statPanelController.HidePrimary();
    }
}
