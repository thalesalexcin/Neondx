using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BlockController : MonoBehaviour
{
    private LayerMask _ValidationMask;
    
    public bool Continuous;
    public float Speed;
    public float TurnDuration;
    
    private float _CurrentTimer;
    private List<Block> _Blocks;

    void Awake()
    {
        _Blocks = GetComponentsInChildren<Block>().ToList();
        _ValidationMask = LayerMask.NameToLayer("Validation");
    }

	// Update is called once per frame
	void Update () 
    {
        _Move();

        _CheckValidation();
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
            transform.Translate(Speed,0,0);
            _CurrentTimer -= TurnDuration;
        }
    }

    private void _ContinousTranslation()
    {
        transform.Translate(Speed * Time.deltaTime, 0, 0);
    }

    private void _CheckValidation()
    {
        int numberOfHits = 0;
        int numberOfValidHits = 0;
        foreach (var block in _Blocks)
        {

            var hit = Physics2D.OverlapPoint(block.transform.position, _ValidationMask);
            if (hit)
            {
                numberOfHits++;
                if (block.Destroyed == hit.GetComponent<Block>().Destroyed)
                    numberOfValidHits++;
            }
        }

        if (numberOfHits == _Blocks.Count)
        {
            if (numberOfValidHits == _Blocks.Count)
                Debug.Log("Valid");
            else
                Debug.Log("Invalid");
        }
    }
}
