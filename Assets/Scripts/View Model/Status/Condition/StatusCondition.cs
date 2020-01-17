using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StatusCondition : MonoBehaviour
{
    public virtual void Remove()
    {
        Status s = GetComponentInParent<Status>();
        if (s)
            s.Remove(this);
    }
}
