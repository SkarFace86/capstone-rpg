using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlaceUnitsState : BattleState
{
    private Queue<Unit> placeUnits;

    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        board.Load(levelData);
        Point p = new Point((int) levelData.tiles[0].x, (int) levelData.tiles[0].z);
        SelectTile(p);

        string[] recipes = new string[]
        {
            "Alaois",
            "Hania",
            "Kamau"
        };

        GameObject unitContainer = new GameObject("Units");
        unitContainer.transform.SetParent(owner.transform);

        List<Tile> locations = new List<Tile>(board.tiles.Values);
        placeUnits = new Queue<Unit>();
        for (int i = 0; i < recipes.Length; i++)
        {
            int level = 1;
            GameObject instance = UnitFactory.Create(recipes[i], level);
            instance.SetActive(false);
            instance.transform.SetParent(unitContainer.transform);

            Unit unit = instance.GetComponent<Unit>();

            units.Add(unit);
            placeUnits.Enqueue(unit);
        }

        while (placeUnits.Count > 0)
        {
            yield return null;
        }

        SelectTile(units[0].tile.pos);

        AddVictoryCondition();
        owner.round = owner.gameObject.AddComponent<TurnOrderController>().Round();
        yield return null;
        owner.ChangeState<SelectUnitState>();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        Unit unit = placeUnits.Dequeue();
        unit.Place(board.GetTile(pos));
        unit.dir = (Directions) UnityEngine.Random.Range(0, 4);
        unit.Match();
        unit.gameObject.SetActive(true);
    }

    void AddVictoryCondition()
    {
        DefeatAllEnemiesVictoryCondition vc = owner.gameObject.AddComponent<DefeatAllEnemiesVictoryCondition>();
    }
}
