using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AbilityTargetState : BattleState
{
    private List<Tile> tiles;
    private List<Tile> attackTiles;
    private AbilityRange ar;
    private AbilityArea aa;

    public override void Enter()
    {
        base.Enter();
        ar = turn.ability.GetComponent<AbilityRange>();
        aa = turn.ability.GetComponent<AbilityArea>();
        SelectTiles();
        SelectAttackTiles();
        statPanelController.ShowPrimary(turn.actor.gameObject);
        if (ar.directionOriented)
        {
            RefreshSecondaryStatPanel(pos);
        }
        if (driver.Current == Drivers.Computer)
            StartCoroutine(ComputerHighlightTarget());
    }

    public override void Exit()
    {
        base.Exit();
        board.DeSelectTiles(tiles);
        board.DeSelectTiles(attackTiles);
        statPanelController.HidePrimary();
        statPanelController.HideSecondary();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        board.DeSelectTiles(attackTiles);
        if (ar.directionOriented)
        {
            ChangeDirection(e.info);
        }
        else
        {
            SelectTiles();
            SelectTile(e.info + pos);
            if(tiles.Contains(board.GetTile(pos)))
                SelectAttackTiles();
            RefreshSecondaryStatPanel(pos);

            // Work on this to get the unit to look at the target
            turn.actor.transform.LookAt(tileSelectionIndicator);
        }
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
        {
            if (ar.directionOriented || tiles.Contains(board.GetTile(pos)))
                owner.ChangeState<ConfirmAbilityTargetState>();
        }
        else
        {
            owner.ChangeState<CategorySelectionState>();
        }
    }

    void ChangeDirection(Point p)
    {
        Directions dir = p.GetDirection();
        if (turn.actor.dir != dir)
        {
            board.DeSelectTiles(tiles);
            turn.actor.dir = dir;
            turn.actor.Match();
        }
        SelectAttackTiles();
    }

    void SelectTiles()
    {
        tiles = ar.GetTilesInRange(board);
        board.SelectTiles(tiles);
    }

    void SelectAttackTiles()
    {
        attackTiles = aa.GetTilesInArea(board, pos);
        board.SelectAttackTiles(attackTiles);
    }

    IEnumerator ComputerHighlightTarget()
    {
        if (ar.directionOriented)
        {
            ChangeDirection(turn.plan.attackDirection.GetNormal());
            yield return new WaitForSeconds(0.25f);
        }
        else
        {
            Point cursorPos = pos;
            while (cursorPos != turn.plan.fireLocation)
            {
                if (cursorPos.x < turn.plan.fireLocation.x) cursorPos.x++;
                if (cursorPos.x > turn.plan.fireLocation.x) cursorPos.x--;
                if (cursorPos.y < turn.plan.fireLocation.y) cursorPos.y++;
                if (cursorPos.y > turn.plan.fireLocation.y) cursorPos.y--;
                SelectTile(cursorPos);
                turn.actor.transform.LookAt(tileSelectionIndicator);
                yield return new WaitForSeconds(0.25f);
            }
        }

        yield return new WaitForSeconds(0.5f);
        owner.ChangeState<ConfirmAbilityTargetState>();
    }
}
