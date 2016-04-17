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
    
    public Image CreditsImage;

    private Image _BlockImage;
    private int _CurrentIndex;
    private bool _TurnedOff;

    private MenuState _CurrentState;

    enum MenuState
    {
        MAIN,
        CREDITS
    }

    void Awake()
    {
        _BlockImage = BlockButton.GetComponentInChildren<Image>();
        _CurrentState = MenuState.MAIN;
    }

	// Use this for initialization
	void Start () 
    {
        _CurrentIndex = 1;
	}
	
	// Update is called once per frame
	void Update () 
    {
        switch (_CurrentState)
        {
            case MenuState.MAIN: _MainState();
                break;
            case MenuState.CREDITS: _CreditsState();
                break;
        }
	}

    private void _CreditsState()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            _SetCreditsOpacity(0);
            _CurrentState = MenuState.MAIN;
        }
    }

    private void _SetCreditsOpacity(float opacity)
    {
        var color = CreditsImage.color;
        color.a = opacity;
        CreditsImage.color = color;
    }

    private void _MainState()
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

    private void _LoadCredits()
    {
        _CurrentState = MenuState.CREDITS;
        _SetCreditsOpacity(1);
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
