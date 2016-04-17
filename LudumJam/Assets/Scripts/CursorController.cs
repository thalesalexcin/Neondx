using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class CursorController : MonoBehaviour
{
    public LayerMask BlocksMask;
    public AudioSource MoveCursor;
    public AudioSource TurnOff;
    public AudioSource TurnOn;

    private List<Transform> _CursorBlocks;
    private GameObject _CurrentCursor;
    
    public void SetCursor(GameObject cursor)
    {
        ClearCursor();

        _CurrentCursor = cursor;
        _CursorBlocks = _CurrentCursor.GetComponentsInChildren<Transform>().Where(c => c.gameObject != cursor).ToList();
    }

    public void ClearCursor()
    {
        if (_CurrentCursor != null)
            DestroyObject(_CurrentCursor);
    }

	// Update is called once per frame
	void Update () 
    {
        if(_CurrentCursor != null)
            _ProcessInputs();
	}

    private void _ProcessInputs()
    {
        Vector3 cursorDisplacement = _GetCursorDisplacement();
        List<Collider2D> hits = _GetTouchedBlocks(cursorDisplacement);

        if (hits.Any())
        {
            if (cursorDisplacement != Vector3.zero)
                MoveCursor.Play();
                
            _CurrentCursor.transform.localPosition = _CurrentCursor.transform.localPosition + cursorDisplacement;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            foreach (var hit in hits)
                hit.GetComponent<Block>().Toggle();
    }

    private Vector3 _GetCursorDisplacement()
    {
        Vector3 cursorDisplacement = new Vector3();

        var unit = 1.45f;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            cursorDisplacement.y += unit;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            cursorDisplacement.y -= unit;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            cursorDisplacement.x -= unit;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            cursorDisplacement.x += unit;

        return cursorDisplacement;
    }

    private List<Collider2D> _GetTouchedBlocks(Vector3 cursorDisplacement)
    {
        List<Collider2D> hits = new List<Collider2D>();
        foreach (var cursorBlock in _CursorBlocks)
        {
            var xxx = cursorBlock.transform.localPosition + cursorDisplacement;

            var hit = Physics2D.OverlapPoint(_CurrentCursor.transform.TransformPoint(xxx), BlocksMask);
            if (hit)
                hits.Add(hit);
        }
        return hits;
    }
}
