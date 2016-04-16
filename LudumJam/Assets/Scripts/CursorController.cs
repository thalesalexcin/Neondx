using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class CursorController : MonoBehaviour
{
    private LayerMask _BlocksMask;
    private List<Transform> _CursorBlocks;

    void Awake()
    {
        _CursorBlocks = GetComponentsInChildren<Transform>().Where(c => c.gameObject != this.gameObject).ToList();
        _BlocksMask = LayerMask.NameToLayer("Blocks");
    }
	
	// Update is called once per frame
	void Update () 
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

        List<Collider2D> hits = _GetTouchedBlocks(cursorDisplacement);

        if(hits.Any())
            transform.Translate(cursorDisplacement, Space.World);

        if (Input.GetKeyDown(KeyCode.Space))
            foreach (var hit in hits)
                hit.GetComponent<Block>().Toggle();
	}

    private List<Collider2D> _GetTouchedBlocks(Vector2 cursorDisplacement)
    {
        List<Collider2D> hits = new List<Collider2D>();
        foreach (var cursorBlock in _CursorBlocks)
        {
            var hit = Physics2D.OverlapPoint(cursorBlock.transform.position + (Vector3)cursorDisplacement, _BlocksMask);
            if (hit)
                hits.Add(hit);
        }
        return hits;
    }
}
