using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MenuManager 
{
    protected override void _LoadCredits()
    {
        _CurrentState = MenuManager.MenuState.CREDITS;
        _CurrentTimer = 0;
    }

    protected override void _CreditsState()
    {
        _CurrentTimer += Time.deltaTime;

        var percentage = _CurrentTimer / FadingDuration;

        BackgroundMusic.volume = 1 - percentage;
        Fader.SetOpacity(percentage);

        if (_CurrentTimer >= FadingDuration)
            SceneManager.LoadScene("Menu");
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
