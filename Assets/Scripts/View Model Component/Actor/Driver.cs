using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The “normal” flag indicates how a unit was loaded initially,
 * and the “special” flag could indicate that the default behavior is overridden,
 * perhaps by a status ailment, etc. I haven’t implemented any of these kinds of abilities yet,
 * but I put the code there as a hint to how it might be handled
 */
public class Driver : MonoBehaviour
{
    public Drivers normal;
    public Drivers special;

    public Drivers Current
    {
        get
        {
            return special != Drivers.None ? special : normal;
        }
    }
}
