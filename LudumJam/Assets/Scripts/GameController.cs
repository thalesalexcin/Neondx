using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
    public CursorController CursorCtrl;
    public BlockController BlockCtrl;
    public GameManager GameManager;

    public GameObject ValidArea;
    public GameObject BlockArea;

    private Vector3 _OldBlockAreaPosition;

    private GameObject _CurrentValidationBlocks;

    void Start()
    {
        _OldBlockAreaPosition = BlockArea.transform.position;
        _AddNextBlock();
        BlockCtrl.SpeedMultiplier = GameManager.speed;
    }

	// Update is called once per frame
	void Update () 
    {
        if (BlockCtrl.HasValidated)
        {
            if (GameManager.hasNextLevel())
                GameManager.nextLevel(BlockCtrl.IsValid);

            BlockCtrl.SpeedMultiplier = GameManager.speed;

            _AddNextBlock();
        }    
	}

    private void _AddNextBlock()
    {
        var validBlockSet = _GetNextValidBlockSet();
        _InstantiateValidBlockSet(validBlockSet);

        var blockSet = _InstantiateBlankBlockSet(validBlockSet);
        BlockCtrl.SetBlockSet(blockSet);

        var cursor = _InstantiateCursor(BlockArea);
        CursorCtrl.SetCursor(cursor);
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

    private void _InstantiateValidBlockSet(GameObject blockSet)
    {
        if (_CurrentValidationBlocks != null)
            DestroyObject(_CurrentValidationBlocks);

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
