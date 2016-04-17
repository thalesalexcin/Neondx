using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
    public CursorController CursorCtrl;
    public BlockController BlockCtrl;
    public GameManager GameManager;
    public UIManager UIManager;

    public AudioSource LevelUp;
    public GameObject ValidArea;
    public GameObject BlockArea;

    private Vector3 _OldBlockAreaPosition;

    private GameObject _CurrentValidationBlocks;

    void Start()
    {
        _OldBlockAreaPosition = BlockArea.transform.position;
        _CurrentState = GameState.START;
    }

    private GameState _CurrentState;
    private float _CurrentTimer;

    enum GameState
    {
        START,
        MOVING,
        ANIMATE_NEXT_LEVEL,
        GENERATE_NEXT_BLOCK
    }

	// Update is called once per frame
	void Update () 
    {
        switch (_CurrentState)
        {
            case GameState.START:
                    _AddNextBlock();
                    BlockCtrl.SpeedMultiplier = GameManager.speed;
                    _CurrentState = GameState.MOVING;
                break;
            case GameState.MOVING:
                    if (BlockCtrl.HasValidated)
                    {
                        var hasPassedLevel = GameManager.nextLevel(BlockCtrl.IsValid);

                        _ClearLevel();

                        if (hasPassedLevel)
                        {
                            _CurrentState = GameState.ANIMATE_NEXT_LEVEL;
                            _CurrentTimer = 0;
                            LevelUp.Play();
                        }
                        else
                            _CurrentState = GameState.GENERATE_NEXT_BLOCK;
                    }
                break;
            case GameState.GENERATE_NEXT_BLOCK:
                    BlockCtrl.SpeedMultiplier = GameManager.speed;
                    _AddNextBlock();
                    _CurrentState = GameState.MOVING;
                break;
            case GameState.ANIMATE_NEXT_LEVEL:
                    _CurrentTimer += Time.deltaTime;
                    UIManager.BlinkNextLevel();
                    if (_CurrentTimer >= GameManager.NextLevelAnimationDuration)
                    {
                        UIManager.HideNextLevel();
                        _CurrentState = GameState.GENERATE_NEXT_BLOCK;
                    }
                break;
        }
	}

    private void _AddNextBlock()
    {
        var validBlockSet = _GetNextValidBlockSet();
        _InstantiateValidBlockSet(validBlockSet);

        var blockSet = _InstantiateBlankBlockSet(validBlockSet);
        BlockCtrl.ClearBlockSet();
        BlockCtrl.SetBlockSet(blockSet);

        var cursor = _InstantiateCursor(BlockArea);
        CursorCtrl.SetCursor(cursor);

        _SetBlockAreaOpacity(1);
    }

    private GameObject _InstantiateCursor(GameObject blockSet)
    {
        var cursorPrefab = _GetNextCursor();
        var cursor = Instantiate(cursorPrefab);
        cursor.transform.parent = blockSet.transform;
        cursor.transform.localPosition = Vector3.zero;
        cursor.transform.localScale = Vector3.one;
        return cursor;
    }

    private GameObject _GetNextCursor()
    {
        return GameManager.cursor;
    }

    private GameObject _GetNextValidBlockSet()
    {
        return GameManager.Fac;
    }

    private void _ClearLevel()
    {
        if (_CurrentValidationBlocks != null)
            DestroyObject(_CurrentValidationBlocks);

        _SetBlockAreaOpacity(0);
        BlockCtrl.ClearBlockSet();
        CursorCtrl.ClearCursor();
    }

    private void _SetBlockAreaOpacity(float a)
    {
        var color = BlockArea.GetComponent<SpriteRenderer>().color;
        color.a = a;
        BlockArea.GetComponent<SpriteRenderer>().color = color;
    }

    private void _InstantiateValidBlockSet(GameObject blockSet)
    {
        _CurrentValidationBlocks = Instantiate(blockSet);
        _CurrentValidationBlocks.transform.parent = ValidArea.transform;
        _CurrentValidationBlocks.transform.localScale = Vector3.one;
        _CurrentValidationBlocks.transform.localPosition = Vector3.zero;
    }

    private GameObject _InstantiateBlankBlockSet(GameObject blockSet)
    {
        var blankBlockSet = Instantiate(blockSet);
        BlockArea.transform.position = _OldBlockAreaPosition;
        blankBlockSet.transform.parent = BlockArea.transform;
        blankBlockSet.transform.localPosition = Vector3.zero;
        blankBlockSet.transform.localScale = Vector3.one;
        return blankBlockSet;
    }
}
