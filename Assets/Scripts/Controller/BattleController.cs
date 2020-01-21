using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;

public class BattleController : StateMachine
{
    public CameraRig cameraRig;
    public Board board;
    public LevelData levelData;
    public Transform TileSelectionIndicator;
    public Point pos;

    public AbilityMenuPanelController abilityMenuPanelController;
    public Turn turn = new Turn();
    public List<Unit> units = new List<Unit>();

    public StatPanelController statPanelController;
    public HitSuccessIndicator hitSuccessIndicator;

    //A more complete implementation for spawning our characters
    //would load the correct models through a Resources.Load call
    public GameObject heroPrefab;
    public Tile currentTile { get { return board.GetTile(pos); } }

    public FacingIndicator facingIndicator;

    public PopupDamageController popupDamageController;
    public BattleMessageController battleMessageController;
    public ComputerPlayer cpu;

    public IEnumerator round;

    void Start()
    {
        ChangeState<InitBattleState>();
    }
}
