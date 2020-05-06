using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
    private BattleController owner;

    Color selectedTileColor = new Color(0, 0.5f, 1, 1);
    Color attackTileColor = new Color(1, 0, 0, 1);
    Color defaultTileColor = new Color(1, 1, 1, 1);

    Point[] dirs = new Point[4]
    {
        new Point(0, 1),
        new Point(0, -1),
        new Point(1, 0),
        new Point(-1, 0)
    };

    // Used for LineAbilityRange
    public Point min { get { return _min; } }
    public Point max { get { return _max; } }
    private Point _min;
    private Point _max;

    public void Load(LevelData data)
    {
        _min = new Point(int.MaxValue, int.MaxValue);
        _max = new Point(int.MinValue, int.MinValue);

        // Place tiles
        for (int i = 0; i < data.tiles.Count; ++i)
        {
            GameObject instance = Instantiate(tilePrefab) as GameObject;
            instance.transform.SetParent(transform);
            Tile t = instance.GetComponent<Tile>();
            t.Load(data.tiles[i]);
            tiles.Add(t.pos, t);

            _min.x = Mathf.Min(_min.x, t.pos.x);
            _min.y = Mathf.Min(_min.y, t.pos.y);
            _max.x = Mathf.Max(_max.x, t.pos.x);
            _max.y = Mathf.Max(_max.y, t.pos.y);
        }

        // Place units
        owner = GetComponentInParent<BattleController>();
        GameObject unitContainer = new GameObject("NPC Units");
        unitContainer.transform.SetParent(owner.transform);

        for (int i = 0; i < data.nonPlayerCharacters.Count; ++i)
        {
            // Create gameobject based on the unit recipe
            GameObject obj = UnitFactory.Create(data.nonPlayerCharacters[i].unitRecipe, data.nonPlayerCharacters[i].level);
            /*
             * They are not active, we could change this so that they become active
             * after the player has placed his units
             */
            //obj.SetActive(false);

            obj.transform.SetParent(unitContainer.transform);
            
            // Get tile point
            Point point = new Point(data.nonPlayerCharacters[i].x, data.nonPlayerCharacters[i].y);
            // Search the dictionary for the Tile with 'point'
            Tile tile;
            if (!tiles.TryGetValue(point, out tile))
                return;
            // Get unit
            Unit unit = obj.GetComponent<Unit>();
            owner.units.Add(unit);
            unit.Place(tile);
            unit.dir = Directions.South;
            unit.Match();
        }
    }

    public List<Tile> Search(Tile start, Func<Tile, Tile, bool> addTile)
    {
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);

        ClearSearch();
        Queue<Tile> checkNext = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();

        start.distance = 0;
        checkNow.Enqueue(start);

        while (checkNow.Count > 0)
        {
            Tile t = checkNow.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                Tile next = GetTile(t.pos + dirs[i]);
                if (next == null || next.distance <= t.distance + 1)
                    continue;

                if (addTile(t, next))
                {
                    next.distance = t.distance + 1;
                    next.prev = t;
                    checkNext.Enqueue(next);
                    retValue.Add(next);
                }
            }
            if (checkNow.Count == 0)
                SwapReference(ref checkNow, ref checkNext);
        }

        return retValue;
    }

    public List<Tile> Search(Tile start, int minDistance, Func<Tile, Tile, bool> addTile)
    {
        List<Tile> retValue = new List<Tile>();

        ClearSearch();
        Queue<Tile> checkNext = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();

        start.distance = 0;
        checkNow.Enqueue(start);

        while (checkNow.Count > 0)
        {
            Tile t = checkNow.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                Tile next = GetTile(t.pos + dirs[i]);
                if (next == null || next.distance <= t.distance + 1)
                    continue;

                if (addTile(t, next))
                {
                    next.distance = t.distance + 1;
                    next.prev = t;
                    checkNext.Enqueue(next);
                    Debug.Log("next.distance: " + next.distance);
                    if(next.distance >= minDistance)
                        retValue.Add(next);
                }
            }
            if (checkNow.Count == 0)
                SwapReference(ref checkNow, ref checkNext);
        }

        return retValue;
    }

    public Tile GetTile(Point p)
    {
        return tiles.ContainsKey(p) ? tiles[p] : null;
    }

    void SwapReference(ref Queue<Tile> a, ref Queue<Tile> b)
    {
        Queue<Tile> temp = a;
        a = b;
        b = temp;
    }

    public void SelectTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", selectedTileColor);
    }

    public void SelectAttackTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; i--)
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", attackTileColor);
    }
    public void DeSelectTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
    }

    void ClearSearch()
    {
        foreach (Tile t in tiles.Values)
        {
            t.prev = null;
            t.distance = int.MaxValue;
        }
    }
}
