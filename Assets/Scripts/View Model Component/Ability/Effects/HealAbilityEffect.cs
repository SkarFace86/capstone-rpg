using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbilityEffect : BaseAbilityEffect
{
    public override int Predict(Tile target)
    {
        Unit attacker = GetComponentInParent<Unit>();
        Unit defender = target.content.GetComponent<Unit>();
        return GetStat(attacker, defender, GetPowerNotification, 0);
    }

    protected override int OnApply(Tile target)
    {
        bc = target.content.GetComponentInParent<BattleController>();
        Unit defender = target.content.GetComponent<Unit>();

        // Start with the predicted value
        int value = Predict(target);

        // Add some random variance
        value = Mathf.FloorToInt(value * UnityEngine.Random.Range(0.9f, 1.1f));

        // Clamp the amount to a range
        value = Mathf.Clamp(value, minDamage, maxDamage);

        // Apply the amound to the target
        Stats s = defender.GetComponent<Stats>();
        s[StatTypes.HP] += value;
        bc.popupDamageController.DisplayAbilityHeal(value.ToString(), defender);

        return value;
    }
}
