using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class CursorController : MonoBehaviour
{
    public LayerMask BlocksMask;

    private List<Transform> _CursorBlocks;
    private GameObject _CurrentCursor;
    
    public void SetCursor(GameObject cursor)
    {
        if (_CurrentCursor != null)
            DestroyObject(_CurrentCursor);
        
        _CurrentCursor = cursor;
        _CursorBlocks = _CurrentCursor.GetComponentsInChildren<Transform>().Where(c => c.gameObject != cursor).ToList();
    }

    void Awake()
    {
    }

	// Update is called once per frame
	void Update () 
    {
        if(_CurrentCursor != null)
            _ProcessInputs();
	}

    private void _ProcessInputs()
    {
        Vector2 cursorDisplacement = _GetCursorDisplacement();
        List<Collider2D> hits = _GetTouchedBlocks(cursorDisplacement);

        if (hits.Any())
            _CurrentCursor.transform.Translate(cursorDisplacement, Space.World);

        if (Input.GetKeyDown(KeyCode.Space))
            foreach (var hit in hits)
                hit.GetComponent<Block>().Toggle();
    }

    private static Vector2 _GetCursorDisplacement()
    {
        Vector2 cursorDisplacement = new Vector2();

        if (Input.GetKeyDown(KeyCode.UpArrow))
            cursorDisplacement.y += 1;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            cursorDisplacement.y -= 1;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            cursorDisplacement.x -= 1;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            cursorDisplacement.x += 1;
        return cursorDisplacement;
    }

    private List<Collider2D> _GetTouchedBlocks(Vector2 cursorDisplacement)
    {
        List<Collider2D> hits = new List<Collider2D>();
        foreach (var cursorBlock in _CursorBlocks)
        {
            var hit = Physics2D.OverlapPoint((Vector2)cursorBlock.transform.position + cursorDisplacement, BlocksMask);
            if (hit)
                hits.Add(hit);
        }
        return hits;
    }
}
