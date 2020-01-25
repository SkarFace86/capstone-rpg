using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderController : MonoBehaviour
{
    #region Constants

    /*
     * this is the minimum threshold value that the CTR stat must reach
     * before a unit is eligible for a turn (except under special conditions)
     */
    private const int turnActivation = 1000;
    /*
     * this is the minimum debit to the CTR stat when a unit takes its turn
     */
    private const int turnCost = 500;
    /*
     * this is an optional debit to the CTR stat, if the unit chooses not to move,
     * it might get another turn more quickly
     */
    private const int moveCost = 300;
    /*
     * this is an optional debit to the CTR stat,
     * if the unit chooses not to take an action (like attack), it might get another turn more quickly
     */
    private const int actionCost = 200;

    public const string TurnBeganNotification = "TurnOrderController.TurnBeganNotification";

    #endregion

    #region Notifications

    public const string RoundBeganNotification = "TurnOrderController.roundBegan";
    public const string TurnCheckNotification = "TurnOrderController.turnCheck";
    public const string TurnCompletedNotification = "TurnOrderController.turnCompleted";
    public const string RoundEndedNotification = "TurnOrderController.roundEnded";

    #endregion

    #region Public

    public IEnumerator Round()
    {
        BattleController bc = GetComponent<BattleController>();
        while (true)
        {
            this.PostNotification(RoundBeganNotification);

            List<Unit> units = new List<Unit>(bc.units);
            for (int i = 0; i < units.Count; i++)
            {
                Stats s = units[i].GetComponent<Stats>();
                s[StatTypes.CTR] += s[StatTypes.SPD];
            }

            units.Sort((a, b) => GetCounter(a).CompareTo(GetCounter(b)));

            for (int i = units.Count - 1; i >= 0; i--)
            {
                if (CanTakeTurn(units[i]))
                {
                    bc.turn.Change(units[i]);
                    units[i].PostNotification(TurnBeganNotification);
                    yield return units[i];

                    int cost = turnCost;
                    if (bc.turn.hasUnitMoved)
                        cost += moveCost;
                    if (bc.turn.hasUnitActed)
                        cost += actionCost;

                    Stats s = units[i].GetComponent<Stats>();
                    s.SetValue(StatTypes.CTR, s[StatTypes.CTR] - cost, false);

                    units[i].PostNotification(TurnCompletedNotification);
                }
            }

            this.PostNotification(RoundEndedNotification);
        }
    }

    #endregion

    #region Private

    bool CanTakeTurn(Unit target)
    {
        Alliance a = target.GetComponentInChildren<Alliance>();
        // OPTIONAL === Add this bit to skip the player turns so you can just watch

        //if (a.type == Alliances.Hero)
        //    return false;

        // OPTIONAL === Add this bit to skip the Computer turns so only the player is playing
        if (a.type == Alliances.Enemy)
            return false;

        // END OPTIONAL
        BaseException exc = new BaseException(GetCounter(target) >= turnActivation);
        target.PostNotification(TurnCheckNotification, exc);
        return exc.toggle;
    }

    int GetCounter(Unit target)
    {
        return target.GetComponent<Stats>()[StatTypes.CTR];
    }

    #endregion
}
