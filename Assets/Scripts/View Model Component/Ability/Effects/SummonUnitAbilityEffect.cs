using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class SummonUnitAbilityEffect : BaseAbilityEffect
{
    public string combatTextName;
    public int duration;

public override int Predict(Tile target)
    {
        return 100;
    }

    protected override int OnApply(Tile target)
    {
        BattleController bc = GetComponentInParent<BattleController>();
        if (target.content != null) {
            Debug.Log("Could not place skeleton");
            return 0;
        }

        Stats stats = gameObject.GetComponentInParent<Stats>();
        int level = stats[StatTypes.LVL];

        GameObject instance = UnitFactory.Create("Summoned Skeleton", level);
        instance.transform.SetParent(GameObject.Find("Battle Controller").transform);
        Unit unit = instance.GetComponent<Unit>();
        unit.Place(target);
        unit.dir = (Directions)UnityEngine.Random.Range(0, 4);
        unit.Match();
        bc.units.Add(unit);

        return 0;
    }
}
