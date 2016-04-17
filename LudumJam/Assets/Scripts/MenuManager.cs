using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public Image Cursor;

    public GameObject StartButton;
    public GameObject BlockButton;
    public GameObject CreditsButton;    

    public AudioSource TurnOn;
    public AudioSource TurnOff;
    public AudioSource CursorMove;

    public Sprite TurnedOnSprite;
    public Sprite TurnedOffSprite;
    
    private Image _BlockImage;
    private int _CurrentIndex;
    private bool _TurnedOff;

    void Awake()
    {
        _BlockImage = BlockButton.GetComponentInChildren<Image>();
    }

	// Use this for initialization
	void Start () 
    {
        _CurrentIndex = 1;
	}
	
	// Update is called once per frame
	void Update () 
    {
        _ProcessInput();
        _MoveCursor();
	}

    private void _ProcessInput()
    {
        int oldIndex = _CurrentIndex;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            _CurrentIndex--;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            _CurrentIndex++;

        _CurrentIndex = Mathf.Clamp(_CurrentIndex, 0, 2);
        if (oldIndex != _CurrentIndex)
            CursorMove.Play();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            _Select();
    }

    private void _Select()
    {
        switch (_CurrentIndex)
        {
            case 0: _LoadLevel();
                break;
            case 1: _ToggleButton();
                break;
            case 2: _LoadCredits();
                break;
        }
    }

    private static void _LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    private static void _LoadLevel()
    {
        SceneManager.LoadScene("Level");
    }

    private void _ToggleButton()
    {

        _TurnedOff = !_TurnedOff;

        if (_TurnedOff)
        {
            _BlockImage.sprite = TurnedOffSprite;
            TurnOff.Play();
        }
        else
        {
            _BlockImage.sprite = TurnedOnSprite;
            TurnOn.Play();
        }
    }

    private void _MoveCursor()
    {
        Vector3 position = Vector3.zero;
        switch (_CurrentIndex)
        {
            case 0: position = StartButton.transform.position;
                break;
            case 1: position = BlockButton.transform.position;
                break;
            case 2: position = CreditsButton.transform.position;
                break;
        }

        Cursor.transform.position = position;
    }
}
