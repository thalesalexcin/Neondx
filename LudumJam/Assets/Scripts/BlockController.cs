using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
    public bool Continuous;
    public Vector2 Velocity;
    public float TurnDuration;
    private float _CurrentTimer;

	// Update is called once per frame
	void Update () 
    {
        if (Continuous)
            transform.Translate(Velocity * Time.deltaTime);
        else
        {
            _CurrentTimer += Time.deltaTime;

            if (_CurrentTimer >= TurnDuration)
            {
                transform.Translate(Velocity);
                _CurrentTimer -= TurnDuration;
            }
        }
	}
}
