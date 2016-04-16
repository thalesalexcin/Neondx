using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
    public CursorController CursorCtrl;
    public BlockController BlockCtrl;
    public GameObject ActorsParent;
    public GameManager GameManager;

    public Transform ValidBlockSetReferencePosition;
    public Transform BlankBlockSetReferencePosition;

    private GameObject _CurrentValidationBlocks;

    void Start()
    {
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

        var cursor = _InstantiateCursor(blockSet);
        CursorCtrl.SetCursor(cursor);
    }

    private GameObject _InstantiateCursor(GameObject blockSet)
    {
        var cursorPrefab = _GetNextCursor();
        var cursor = Instantiate(cursorPrefab);
        cursor.transform.parent = blockSet.transform;
        cursor.transform.localPosition = Vector2.zero;
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
        _CurrentValidationBlocks.transform.parent = ActorsParent.transform;
        _CurrentValidationBlocks.transform.position = ValidBlockSetReferencePosition.transform.position;
    }

    private GameObject _InstantiateBlankBlockSet(GameObject blockSet)
    {
        var blankBlockSet = Instantiate(blockSet);
        blankBlockSet.transform.parent = ActorsParent.transform;
        blankBlockSet.transform.position = BlankBlockSetReferencePosition.transform.position;
        return blankBlockSet;
    }
}
