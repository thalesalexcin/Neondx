using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BlockController : MonoBehaviour
{    
    public bool Continuous;
    public float Speed;
    public float TurnDuration;

    public AudioSource ValidBlock;
    public AudioSource InvalidBlock;

    private float _CurrentTimer;
    private List<Block> _Blocks;
    private GameObject _CurrentBlockSet;

    public LayerMask ValidationBlocksMask;

    public void SetBlockSet(GameObject blockSet)
    {
        ClearBlockSet();
        _CurrentBlockSet = blockSet;
        _Blocks = _CurrentBlockSet.GetComponentsInChildren<Block>().ToList();

        foreach (var block in _Blocks)
        {
            block.Reset();
            block.gameObject.layer = LayerMask.NameToLayer("Blocks");
        }
    }

	// Update is called once per frame
	void Update () 
    {
        if (_CurrentBlockSet != null)
        {
            _Move();
            _CheckValidation();
        }
	}

    private void _Move()
    {
        if (Continuous)
            _ContinousTranslation();
        else
            _DiscreteTranslation();
    }

    private void _DiscreteTranslation()
    {
        _CurrentTimer += Time.deltaTime;

        if (_CurrentTimer >= TurnDuration)
        {
            _Translate(_GetSpeed(), 0, 0);
            _CurrentTimer -= TurnDuration;
        }
    }

    private void _ContinousTranslation()
    {
        _Translate(_GetSpeed() * Time.deltaTime, 0, 0);
    }

    private void _Translate(float x, float y, float z)
    {
        _CurrentBlockSet.transform.parent.Translate(x,y,z);
    }

    private float _GetSpeed()
    {
        return Speed * SpeedMultiplier;
    }

    private void _CheckValidation()
    {
        int numberOfHits = 0;
        int numberOfValidHits = 0;
        foreach (var block in _Blocks)
        {
            var hit = Physics2D.OverlapPoint(block.transform.position, ValidationBlocksMask);
            if (hit)
            {
                numberOfHits++;
                if (block.Destroyed == hit.GetComponent<Block>().Destroyed)
                    numberOfValidHits++;
            }
        }

        if (numberOfHits == _Blocks.Count)
        {
            HasValidated = true;
            IsValid = numberOfValidHits == _Blocks.Count;
            
            if (IsValid)
                ValidBlock.Play();
            else
                InvalidBlock.Play();
        }
    }

    public bool HasValidated { get; set; }
    public bool IsValid { get; set; }

    public float SpeedMultiplier { get; set; }

    public void ClearBlockSet()
    {
        IsValid = false;
        HasValidated = false;

        if (_CurrentBlockSet != null)
            DestroyObject(_CurrentBlockSet);
    }
}
