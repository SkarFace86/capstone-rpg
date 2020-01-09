using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityRange : MonoBehaviour
{
    // defines the number of tiles away from the user which can be reached
    public int horizontal = 1;
    // defines the height difference between the user’s tile and the target tiles which are within reach
    public int vertical = int.MaxValue;
    /*
     * When it is true, we will use the movement input buttons to change
     * the user’s facing direction so that the effected tiles change.
     * When the directionOriented property is false, you may move the cursor
     * to select tiles within the highlighted range
     */
    public virtual bool directionOriented { get { return false; } }

    protected Unit unit { get { return GetComponentInParent<Unit>(); } }

    /*
     * Every concrete subclass will be required to implement the GetTilesInRange method,
     * which will return a List of Tile(s) which can be reached by the selected ability.
     * This is how we will know what tiles to highlight on the board, and in the future
     * will be used to determine if there are targets within reach
     */
    public abstract List<Tile> GetTilesInRange(Board board);
}
