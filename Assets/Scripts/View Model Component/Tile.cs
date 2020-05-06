using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject content;

    [HideInInspector] public Tile prev;
    [HideInInspector] public int distance;

    // Step height is the height of a cube
    public const float stepHeight = 0.25f;

    // Tiles position and height
    public Point pos;
    public int height;

    /*
     * Returns the center of the tile at the top, allowing you to put things
     * on top of the tile 
     */
    public Vector3 center {
        get { return new Vector3(pos.x, height * stepHeight, pos.y); }
    }

    /*
     * Anytime the position or height of a tile is modified
     * I will want it to be able to visually reflect its new values
     */
    void Match()
    {
        transform.localPosition = new Vector3(pos.x, height * stepHeight / 2f, pos.y);

        transform.localScale = new Vector3(1, height * stepHeight, 1);
    }

    /*
     * Our board creator will create the boards by randomly
     * growing and or shrinking tiles
     */
    public void Grow()
    {
        height++;
        Match();
    }

    public void Shrink()
    {
        height--;
        Match();
    }
    /*
     * This will make it easy for me to persist the Tile data as a Vector3 later
     */
    public void Load(Point p, int h)
    {
        pos = p;
        height = h;
        Match();
    }

    public void Load(Vector3 v)
    {
        Load(new Point((int)v.x, (int)v.z), (int)v.y);
    }
}
