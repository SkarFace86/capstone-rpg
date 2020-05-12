using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public int range { get { return stats[StatTypes.MOV]; } }
    public int jumpHeight { get { return stats[StatTypes.JMP]; } }
    public bool isMoving = false;
    protected Unit unit;
    protected Transform jumper;
    protected float halfExtents;
    protected Stats stats;
    protected Animation anim;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
        jumper = transform.Find("Jumper");
        //halfExtents = transform.Find("Scale").localScale.y;
    }

    protected virtual void Start()
    {
        stats = GetComponent<Stats>();
    }

    public virtual List<Tile> GetTilesInRange(Board board)
    {
        List<Tile> retValue = board.Search(unit.tile, ExpandSearch);
        Filter(retValue);
        return retValue;
    }

    protected virtual bool ExpandSearch(Tile from, Tile to)
    {
        return (from.distance + 1) <= range;
    }

    /*
     * The Filter method will also be overridable while offering a base implementation.
     * It loops through the list of tiles returned by a board search, and removes any which hold blocking content.
     * This step is required because some search criteria may have allowed the unit to travel over tiles
     * which had content (like an ally) but should not be allowed to stop there. In the future this check may be more complex,
     * for example, we may want to allow a unit to occupy the same location as a trap, but for now, any content will be considered an obstacle
     */
    protected virtual void Filter(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; i--)
            if (tiles[i].content != null)
                tiles.RemoveAt(i);
    }

    /*
     * We will also have a public method which tells the component to handle the animation of actually traversing a path.
     * It will be left as abstract in the base class requiring all concrete subclasses to provide their own implementation
     */
    public abstract IEnumerator Traverse(Tile tile);

    protected virtual IEnumerator Turn(Directions dir)
    {
        TransformLocalEulerTweener t = (TransformLocalEulerTweener)transform.RotateToLocal(dir.ToEuler(), 0.25f, EasingEquations.EaseInOutQuad);

        // When rotating between North and West, we must make an exception so it looks like the unit
        // rotates the most efficient way (since 0 and 360 are treated the same)
        if (Mathf.Approximately(t.startTweenValue.y, 0f) && Mathf.Approximately(t.endTweenValue.y, 270f))
            t.startTweenValue = new Vector3(t.startTweenValue.x, 360f, t.startTweenValue.z);
        else if (Mathf.Approximately(t.startTweenValue.y, 270) && Mathf.Approximately(t.endTweenValue.y, 0))
            t.endTweenValue = new Vector3(t.startTweenValue.x, 360f, t.startTweenValue.z);
        unit.dir = dir;

        while (t != null)
            yield return null;
    }
}
