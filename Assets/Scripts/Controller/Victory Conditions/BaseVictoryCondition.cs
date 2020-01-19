using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class BaseVictoryCondition : MonoBehaviour
{
    private Alliances victor = Alliances.None;
    protected BattleController bc;

    public Alliances Victor
    {
        get { return victor; }
        protected set { victor = value; }
    }

    protected virtual void Awake()
    {
        bc = GetComponent<BattleController>();
    }

    protected virtual void OnEnable()
    {
        this.AddObserver(OnHPDidChangeNotification, Stats.DidChangeNotification(StatTypes.HP));
    }

    protected virtual void OnDisable()
    {
        this.RemoveObserver(OnHPDidChangeNotification, Stats.DidChangeNotification(StatTypes.HP));
    }

    protected virtual void OnHPDidChangeNotification(object sender, object args)
    {
        CheckForGameOver();
    }

    // Check if defeated a specific enemy
    protected virtual bool IsDefeated(Unit unit)
    {
        Health health = unit.GetComponent<Health>();
        if (health)
            return health.minHP == health.HP;

        Stats stats = unit.GetComponent<Stats>();
        return stats[StatTypes.HP] == 0;
    }

    // Check if any party of units is defeated
    protected virtual bool PartyDefeated(Alliances type)
    {
        for (int i = 0; i < bc.units.Count; i++)
        {
            Alliance a = bc.units[i].GetComponent<Alliance>();
            if (a == null)
                continue;

            if (a.type == type && !IsDefeated(bc.units[i]))
                return false;
        }
        return true;
    }

    // Check when all of the hero units are dead
    protected virtual void CheckForGameOver()
    {
        if (PartyDefeated(Alliances.Hero))
            Victor = Alliances.Enemy;
    }
}
