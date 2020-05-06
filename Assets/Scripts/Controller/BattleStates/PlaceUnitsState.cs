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
        //board.Load(levelData);
        //Point p = new Point((int)levelData.tiles[0].x, (int)levelData.tiles[0].z);
        //SelectTile(p);
        //AddVictoryCondition();

        string[] recipes = new string[]
        {
            "Viking King",
            "Necro Man, Sir",
            "Miss Terry"
        };

        GameObject unitContainer = new GameObject("Friendly Units");
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
        // Destroy victory condition and add another for it to work
        Destroy(owner.GetComponent<DefeatAllEnemiesVictoryCondition>());
        AddVictoryCondition();

        while (placeUnits.Count > 0)
        {
            //for (int i = 0; i < placeUnits.Count; i++) {
            //    placeUnits.Peek().gameObject.SetActive(true);
            //    Unit unit = placeUnits.Peek();
            //    unit.Place(board.GetTile(pos));
            //    unit.Match();
            //    yield return null;
            //}
            yield return null;
        }

        SelectTile(units[0].tile.pos);

        owner.round = owner.gameObject.AddComponent<TurnOrderController>().Round();
        yield return null;
        owner.ChangeState<SelectUnitState>();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
        //Unit unit = placeUnits.Peek();
        //unit.Place(board.GetTile(pos));
        //unit.Match();
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    { 
        Tile tile = board.GetTile(pos);
        if (tile.content == null) {
            Unit unit = placeUnits.Dequeue();
            unit.Place(board.GetTile(pos));
            unit.dir = (Directions)UnityEngine.Random.Range(0, 4);
            unit.Match();
            unit.gameObject.SetActive(true);
        }
    }

    void AddVictoryCondition()
    {
        DefeatAllEnemiesVictoryCondition vc = owner.gameObject.AddComponent<DefeatAllEnemiesVictoryCondition>();
    }
}
