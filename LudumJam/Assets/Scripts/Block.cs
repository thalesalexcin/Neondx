using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour 
{
    public Color AliveColor;
    public Color DestroyedColor;

    public bool Destroyed;
    private SpriteRenderer _SpriteRenderer;

    void OnValidate()
    {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _UpdateSprite();
    }

    void Awake()
    {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _UpdateSprite();
    }

    public void Toggle()
    {
        Destroyed = !Destroyed;
        _UpdateSprite();        
    }

    private void _UpdateSprite()
    {
        if (Destroyed)
            _Destroy();
        else
            _Revive();
    }

    private void _Destroy()
    {
        _SpriteRenderer.color = DestroyedColor;   
    }

    private void _Revive()
    {
        _SpriteRenderer.color = AliveColor;
    }
}
