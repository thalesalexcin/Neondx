using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{
    public GameManager GameManager;

    public float BlinkSpeed = 5;

    public Text Score;
    public Image EnergyBar;
    public Image NextLevel;

    public Image X2;
    public Sprite X2_On;
    public Sprite X2_Off;

    public Image X4;
    public Sprite X4_On;
    public Sprite X4_Off;
    
    public Image X8;
    public Sprite X8_On;
    public Sprite X8_Off;

    public Image X16;
    public Sprite X16_On;
    public Sprite X16_Off;

    void Awake()
    {
        _RegisterEvents();
    }

    private void _RegisterEvents()
    {
        GameManager.OnScoreChanged += GameManager_OnScoreChanged;
        GameManager.OnBlockSetChanged += GameManager_OnBlockSetChanged;
        GameManager.OnBonusIndexChanged += GameManager_OnBonusIndexChanged;
        GameManager.OnCursorChanged += GameManager_OnCursorChanged;
        GameManager.OnLevelChanged += GameManager_OnLevelChanged;
        GameManager.OnMultiplierChanged += GameManager_OnMultiplierChanged;
        GameManager.OnSpeedChanged += GameManager_OnSpeedChanged;
    }

    void GameManager_OnLevelChanged(float value)
    {
        
    }

    void GameManager_OnSpeedChanged(float value)
    {

    }

    void GameManager_OnMultiplierChanged(float value)
    {
        X2.sprite = X2_Off;
        X4.sprite = X4_Off;
        X8.sprite = X8_Off;
        X16.sprite = X16_Off;
        
        if(value == 2)
            X2.sprite = X2_On;
        else if (value == 4)
            X4.sprite = X4_On;
        else if (value == 8)
            X8.sprite = X8_On;
        else if (value == 16)
            X16.sprite = X16_On;
    }

    public void HideNextLevel()
    {
        Color color = NextLevel.color;
        color.a = 0;
        NextLevel.color = color;
        BlinkSpeed = Mathf.Abs(BlinkSpeed);
    }

    public void BlinkNextLevel()
    {
        Color color = NextLevel.color;

        color.a += BlinkSpeed * Time.deltaTime;

        if (color.a >= 1 || color.a <= 0)
        {
            color.a = Mathf.Clamp01(color.a);
            BlinkSpeed *= -1;
        }

        NextLevel.color = color;
    }

    void GameManager_OnCursorChanged(float value)
    {

    }

    void GameManager_OnBonusIndexChanged(float value)
    {
        EnergyBar.fillAmount = Mathf.Lerp(0, 1, value/16);
    }

    void GameManager_OnBlockSetChanged(float value)
    {

    }

    void GameManager_OnScoreChanged(float value)
    {
        string stringValue = value.ToString();

        if(stringValue.Length <= 7)
            stringValue = stringValue.PadLeft(7, '0');
        Score.text = stringValue;
    }
}
