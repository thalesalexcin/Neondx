using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour 
{
    public Sprite TurnedOnSprite;
    public Sprite TurnedOffSprite;

    public bool Destroyed;
    public bool TurnedOff;
    private SpriteRenderer _SpriteRenderer;

    void OnValidate()
    {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _UpdateSprite();
        TurnedOff = Destroyed;
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
            _TurnOff();
        else
            _TurnOn();
    }

    private void _TurnOff()
    {
        _SpriteRenderer.sprite = TurnedOffSprite;   
    }

    private void _TurnOn()
    {
        _SpriteRenderer.sprite = TurnedOnSprite;
    }

    internal void Reset()
    {
        Destroyed = false;
        _TurnOn();
    }
}
