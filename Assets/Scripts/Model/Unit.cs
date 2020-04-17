using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Tile tile { get; protected set; }
    public Directions dir;
    private float halfExtents;

    public void Place(Tile target)
    {
        // Make sure old tile location is not still pointing to this unit
        if (tile != null && tile.content == gameObject)
            tile.content = null;

        // Link unit and tile references
        tile = target;

        if (target != null)
            target.content = gameObject;
    }

    public void Match()
    {
        //halfExtents = gameObject.transform.GetChild(0).GetChild(0).localScale.y;
        //transform.localPosition = tile.center + new Vector3(0, halfExtents, 0);
        transform.localPosition = tile.center;
        transform.localEulerAngles = dir.ToEuler();
    }
}
