using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.Util;

public class CursorController : MonoBehaviour
{
    public bool CanRotate;
    public LayerMask BlocksMask;
    private List<Transform> _CursorBlocks;

    void Awake()
    {
        _CursorBlocks = GetComponentsInChildren<Transform>().Where(c => c.gameObject != this.gameObject).ToList();
    }

    // Use this for initialization
	void Start () 
    {
        //Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector2 p = new Vector2();
        float rotate = 0;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            p.y += 1;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            p.y -= 1;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            p.x -= 1;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            p.x += 1;

        if (CanRotate)
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
                rotate += 90;
            if (Input.GetKeyDown(KeyCode.LeftShift))
                rotate -= 90;
            transform.Rotate(0, 0, rotate);
        }

        bool destroy = Input.GetKeyDown(KeyCode.Space);

        List<Collider2D> hits = new List<Collider2D>();
        foreach (var cursorBlock in _CursorBlocks)
        {
            var hit = Physics2D.OverlapPoint(cursorBlock.transform.position + (Vector3) p, BlocksMask);

            if (hit)
                hits.Add(hit);
        }

        if(hits.Any())
            transform.Translate(p, Space.World);

        if(destroy)
            foreach (var hit in hits)
                DestroyObject(hit.gameObject);
	}


    private void _MouseCursor()
    {
        var p = _GetWorldPoint(Input.mousePosition);
        p.x = Mathf.RoundToInt(p.x);
        p.y = Mathf.RoundToInt(p.y);

        transform.position = p;
    }

    private Vector3 _GetWorldPoint(Vector3 screenPosition)
    {
        return Utils.GetWorldPositionOnPlane(Camera.main, screenPosition, 0);
    }
}
