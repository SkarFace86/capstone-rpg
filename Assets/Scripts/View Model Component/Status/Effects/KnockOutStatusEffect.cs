using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockOutStatusEffect : StatusEffect
{
    private Unit owner;
    private Stats stats;

    void Awake()
    {
        owner = GetComponentInParent<Unit>();
        stats = owner.GetComponent<Stats>();
    }

    void OnEnable()
    {
        // Squish the model
        owner.transform.localScale = new Vector3(0.75f, 0.1f, 0.75f);

        //if (anim != null)
        //    anim.Play("Death1");
        //else
        //    owner.transform.localScale = new Vector3(0.75f, 0.1f, 0.75f);
        this.AddObserver(OnTurnCheck, TurnOrderController.TurnCheckNotification, owner);
        this.AddObserver(OnStatCounterWillChange, Stats.WillChangeNotification(StatTypes.CTR), stats);
    }

    void OnDisable()
    {
        // Unsquish the model
        owner.transform.localScale = Vector3.one;

        //if (anim != null)
        //    anim.Play("Up");
        //else
        //    owner.transform.localScale = Vector3.one;
        this.RemoveObserver(OnTurnCheck, TurnOrderController.TurnCheckNotification, owner);
        this.RemoveObserver(OnStatCounterWillChange, Stats.WillChangeNotification(StatTypes.CTR), stats);
    }

    void OnTurnCheck(object sender, object args)
    {
        // Dont allow a KO'd unit to take turns
        BaseException exc = args as BaseException;
        if (exc.defaultToggle == true)
            exc.FlipToggle();
    }

    void OnStatCounterWillChange(object sender, object args)
    {
        // Dont allow a KO'd unit to increment the turn order counter
        ValueChangeException exc = args as ValueChangeException;
        if (exc.toValue > exc.fromValue)
            exc.FlipToggle();
    }
}