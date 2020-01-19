using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerPlayer : MonoBehaviour
{
    #region Fields

    private BattleController bc;
    Unit actor { get { return bc.turn.actor; } }

    private Unit nearestFoe;
    Alliance alliance { get { return actor.GetComponent<Alliance>(); } }

    #endregion

    #region MonoBehaviour

    void Awake()
    {

        bc = GetComponent<BattleController>();
    }

    #endregion

    #region Public

    public PlanOfAttack Evaluate()
    {
        // Create and fill out a plan of attack
        PlanOfAttack poa = new PlanOfAttack();

        // Step 1: Decide what ability to use
        AttackPattern pattern = actor.GetComponentInChildren<AttackPattern>();
        if (pattern)
            pattern.Pick(poa);
        else
            DefaultAttackPattern(poa);

        // Step 2: Determine where to move and aim to best use the ability
        //PlaceHolderCode(poa);
        Debug.Log("IsPositionIndependent: " + IsPositionIndependent(poa));
        Debug.Log("IsDirectionIndependent: " + IsDirectionIndependent(poa));
        Debug.Log("PlanDirectionDependent");


        if (IsPositionIndependent(poa))
            PlanPositionIndependent(poa);
        else if (IsDirectionIndependent(poa))
            PlanDirectionIndependent(poa);
        else
            PlanDirectionDependent(poa);

        // If no ability is able to be determined, move to an opponent
        if (poa.ability == null)
            MoveTowardOpponent(poa);

        // Return the completed plan
        return poa;
    }

    #endregion

    #region Private

    void DefaultAttackPattern(PlanOfAttack poa)
    {
        // Just get the first "Attack" ability
        poa.ability = actor.GetComponentInChildren<Ability>();
        poa.target = Targets.Foe;
    }


    /*
     * FIRST IF STATEMENT
     * These two methods determine if a position matters or not when deciding how to use an ability.
     * If TRUE, the move to a random tile within the unit's move range because position doesnt matter.
     */
    bool IsPositionIndependent(PlanOfAttack poa)
    {
        AbilityRange range = poa.ability.GetComponent<AbilityRange>();
        return range.positionOriented == false;
    }

    void PlanPositionIndependent(PlanOfAttack poa)
    {
        List<Tile> moveOptions = GetMoveOptions();
        Tile tile = moveOptions[Random.Range(0, moveOptions.Count)];
        poa.moveLocation = poa.fireLocation = tile.pos;
    }
    /*
     * END FIRST IF STATEMENT
     */


    /*
     * SECOND IF STATEMENT
     * These two methods are used when the position matters but the direction does not, for example
     * Fire Ball or Cure can target different units based on where you move the aiming cursor.
     */
    bool IsDirectionIndependent(PlanOfAttack poa)
    {
        AbilityRange range = poa.ability.GetComponent<AbilityRange>();
        return !range.directionOriented;
    }

    void PlanDirectionIndependent(PlanOfAttack poa)
    {
        Tile startTile = actor.tile;
        Dictionary<Tile, AttackOption> map = new Dictionary<Tile, AttackOption>();
        AbilityRange ar = poa.ability.GetComponent<AbilityRange>();
        List<Tile> moveOptions = GetMoveOptions();

        for (int i = 0; i < moveOptions.Count; i++)
        {
            Tile moveTile = moveOptions[i];
            actor.Place(moveTile);
            List<Tile> fireOptions = ar.GetTilesInRange(bc.board);

            for (int j = 0; j < fireOptions.Count; j++)
            {
                Tile fireTile = fireOptions[j];
                AttackOption ao = null;
                if (map.ContainsKey(fireTile))
                {
                    ao = map[fireTile];
                }
                else
                {
                    ao = new AttackOption();
                    map[fireTile] = ao;
                    ao.target = fireTile;
                    ao.direction = actor.dir;
                    RateFireLocation(poa, ao);
                }

                ao.AddMoveTarget(moveTile);
            }
        }

        actor.Place(startTile);
        List<AttackOption> list = new List<AttackOption>(map.Values);
        PickBestOption(poa, list);
    }

    /*
     * END SECOND IF STATEMENT
     */


    /*
     * THIRD IF STATEMENT
     * This case depends both on the units position on the board and also the direction the unit faces
     */
    void PlanDirectionDependent(PlanOfAttack poa)
    {
        Tile startTile = actor.tile;
        Directions startDirection = actor.dir;
        List<AttackOption> list = new List<AttackOption>();
        List<Tile> moveOptions = GetMoveOptions();

        for (int i = 0; i < moveOptions.Count; i++)
        {
            Tile moveTile = moveOptions[i];
            actor.Place(moveTile);

            for (int j = 0; j < 4; j++)
            {
                actor.dir = (Directions) j;
                AttackOption ao = new AttackOption();
                ao.target = moveTile;
                ao.direction = actor.dir;
                RateFireLocation(poa, ao);
                ao.AddMoveTarget(moveTile);
                list.Add(ao);
            }
        }

        actor.Place(startTile);
        actor.dir = startDirection;
        PickBestOption(poa, list);
    }

    /*
     * END THIRD IF STATEMENT
     */


    List<Tile> GetMoveOptions()
    {
        return actor.GetComponent<Movement>().GetTilesInRange(bc.board);
    }

    void RateFireLocation(PlanOfAttack poa, AttackOption option)
    {
        AbilityArea area = poa.ability.GetComponent<AbilityArea>();
        List<Tile> tiles = area.GetTilesInArea(bc.board, option.target.pos);
        option.areaTargets = tiles;
        option.isCasterMatch = IsAbilityTargetMatch(poa, actor.tile);

        for (int i = 0; i < tiles.Count; i++)
        {
            Tile tile = tiles[i];
            if (actor.tile == tiles[i] || !poa.ability.IsTarget(tile))
                continue;

            bool isMatch = IsAbilityTargetMatch(poa, tile);
            option.AddMark(tile, isMatch);
        }
    }

    bool IsAbilityTargetMatch(PlanOfAttack poa, Tile tile)
    {
        bool isMatch = false;
        if (poa.target == Targets.Tile)
            isMatch = true;
        else if (poa.target != Targets.None)
        {
            Alliance other = tile.content.GetComponentInChildren<Alliance>();
            if (other != null && alliance.IsMatch(other, poa.target))
                isMatch = true;
        }

        return isMatch;
    }

    void PickBestOption(PlanOfAttack poa, List<AttackOption> list)
    {
        int bestScore = 1;
        List<AttackOption> BestOptions = new List<AttackOption>();

        for (int i = 0; i < list.Count; i++)
        {
            AttackOption option = list[i];
            int score = option.GetScore(actor, poa.ability);
            if (score > bestScore)
            {
                bestScore = score;
                BestOptions.Clear();
                BestOptions.Add(option);
            }
            else if (score == bestScore)
            {
                BestOptions.Add(option);
            }
        }

        if (BestOptions.Count == 0)
        {
            poa.ability = null; // Clear ability as a sign not to perform it
            return;
        }

        List<AttackOption> finalPicks = new List<AttackOption>();
        bestScore = 0;
        for (int i = 0; i < BestOptions.Count; i++)
        {
            AttackOption option = BestOptions[i];
            int score = option.bestAngleBasedScore;
            if (score > bestScore)
            {
                bestScore = score;
                finalPicks.Clear();
                finalPicks.Add(option);
            }
            else if (score == bestScore)
            {
                finalPicks.Add(option);
            }
        }

        AttackOption choice = finalPicks[UnityEngine.Random.Range(0, finalPicks.Count)];
        poa.fireLocation = choice.target.pos;
        poa.attackDirection = choice.direction;
        poa.moveLocation = choice.bestMoveTile.pos;
    }

    void FindNearestFoe()
    {
        nearestFoe = null;
        bc.board.Search(actor.tile, delegate(Tile arg1, Tile arg2)
        {
            if (nearestFoe == null && arg2.content != null)
            {
                Alliance other = arg2.content.GetComponentInChildren<Alliance>();
                if (other != null && alliance.IsMatch(other, Targets.Foe))
                {
                    Unit unit = other.GetComponent<Unit>();
                    Stats stats = unit.GetComponent<Stats>();
                    if (stats[StatTypes.HP] > 0)
                    {
                        nearestFoe = unit;
                        return true;
                    }
                }
            }

            return nearestFoe == null;
        });
    }

    void MoveTowardOpponent(PlanOfAttack poa)
    {
        List<Tile> moveOptions = GetMoveOptions();
        FindNearestFoe();
        if (nearestFoe != null)
        {
            Tile toCheck = nearestFoe.tile;
            while (toCheck != null)
            {
                if (moveOptions.Contains(toCheck))
                {
                    poa.moveLocation = toCheck.pos;
                    return;
                }

                toCheck = toCheck.prev;
            }
        }

        poa.moveLocation = actor.tile.pos;
    }

    public Directions DetermineEndFacingDirection()
    {
        Directions dir = (Directions) UnityEngine.Random.Range(0, 4);
        FindNearestFoe();
        if (nearestFoe != null)
        {
            Directions start = actor.dir;
            for (int i = 0; i < 4; i++)
            {
                actor.dir = (Directions) i;
                if (nearestFoe.GetFacing(actor) == Facings.Front)
                {
                    dir = actor.dir;
                    break;
                }
            }

            actor.dir = start;
        }

        return dir;
    }

    //void PlaceHolderCode(PlanOfAttack poa)
    //{
    //    // Move to a random location within the unit's move range
    //    List<Tile> tiles = actor.GetComponent<Movement>().GetTilesInRange(bc.board);
    //    Tile randomTile = (tiles.Count > 0) ? tiles[UnityEngine.Random.Range(0, tiles.Count)] : null;
    //    poa.moveLocation = (randomTile != null) ? randomTile.pos : actor.tile.pos;

    //    // Pick a random attack direction (for direction based abilities)
    //    poa.attackDirection = (Directions) UnityEngine.Random.Range(0, 4);

    //    // Pick a random fire location based on having moved to the random tile
    //    Tile start = actor.tile;
    //    actor.Place(randomTile);
    //    tiles = poa.ability.GetComponent<AbilityRange>().GetTilesInRange(bc.board);
    //    if (tiles.Count == 0)
    //    {
    //        poa.ability = null;
    //        poa.fireLocation = poa.moveLocation;
    //    }
    //    else
    //    {
    //        randomTile = tiles[UnityEngine.Random.Range(0, tiles.Count)];
    //        poa.fireLocation = randomTile.pos;
    //    }

    //    actor.Place(start);
    //}

    //public Directions DetermineEndFacingDirection()
    //{
    //    return (Directions) UnityEngine.Random.Range(0, 4);
    //}

    #endregion
}
