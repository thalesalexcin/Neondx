using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
    public delegate void UIValueChanged(float value);
    public event UIValueChanged OnMultiplierChanged;
    public event UIValueChanged OnScoreChanged;
    public event UIValueChanged OnSpeedChanged;
    public event UIValueChanged OnBonusIndexChanged;
    public event UIValueChanged OnLevelChanged;
    public event UIValueChanged OnCursorChanged;
    public event UIValueChanged OnBlockSetChanged;

    // constantes
    const int NB_CURSOR = 4;
    const int NB_FAC_PAR_CURSOR = 5;
    const int MAX_INDICE_BONUS = 16;
    const int PERTE_INDICE_BONUS = 1; // correspond a un demi cran
    const int GAIN_INDICE_BONUS = 1; //correspond a un demi cran
    const int SCORE_WIN_BASE = 500;
    const float MAX_SPEED = 8;

    // variables accessible de l'exterieur
    public float NextLevelAnimationDuration = 1;
    public float speedIncrementation = 1.2f;
    
    [HideInInspector]
    public uint score = 0;
    [HideInInspector]
    public ushort numLevel = 1;
    [HideInInspector]
    public GameObject cursor;
    [HideInInspector]
    public GameObject Fac;
    [HideInInspector]
    public float speed = 1;
    [HideInInspector]
    public int indiceBonus = 8;
    [HideInInspector]
    public byte MultiplicateurBonus = 1;


    // variables fonctionnement interne
    private byte numCursor = 1;
    private byte numFac;
    private bool semiValidate = false;
    private bool canContinue = true;

    //stockage des prefabs
    private GameObject[] cursorTab = new GameObject[NB_CURSOR];
    private GameObject[,] FacTab = new GameObject[NB_CURSOR, NB_FAC_PAR_CURSOR];

    // Use this for initialization
    void Awake () 
    {
        //init cursor et Fac
        initCursor();
        cursor = cursorTab[0];
        generateFAC(false);
    }

    private void initCursor()
    {
        //chargement des cursors depuis les assets
        string s;
        for (int i = 0; i < NB_CURSOR; i++)
        {
            s = "Cursors/Cursor";
            s += (i + 1).ToString();
            cursorTab[i] = (GameObject) Resources.Load(s);
            initFAC(i);
        }
    }

    private void initFAC(int numCursor)
    {
        string s;
        for(int i = 0; i < NB_FAC_PAR_CURSOR; i++)
        {
            s = "ValidBlocks/Fac_" + (numCursor+1).ToString() + "_";
            s += (i+1).ToString();
            FacTab[numCursor, i] = (GameObject) Resources.Load(s);
        }
    }

    public bool hasNextLevel()
    {
        return canContinue;
    }

    public bool nextLevel(bool isValid)
    {
        bool hasPassedLevel = false;
        if (isValid) 
            hasPassedLevel = win();
        else loose();

        return hasPassedLevel;
    }

    // la grille precedente etait valide
    private bool win()
    {
        _IncrementScore();
        _IncrementBonusIndex();
        setMultiplicateurBonus();
        return setCursor();
    }

    private void _IncrementBonusIndex()
    {
        if (indiceBonus < MAX_INDICE_BONUS)
        {
            indiceBonus += GAIN_INDICE_BONUS;
        }
        
        
        if (OnBonusIndexChanged != null)
            OnBonusIndexChanged(indiceBonus);
    }

    private void _IncrementScore()
    {
        score += (uint)SCORE_WIN_BASE * MultiplicateurBonus;

        if (OnScoreChanged != null)
            OnScoreChanged(score);
    }

    // la grille precedente n'etait pas valide
    private void loose()
    {
        indiceBonus -= PERTE_INDICE_BONUS;

        if (OnBonusIndexChanged != null)
            OnBonusIndexChanged(indiceBonus);

        if (indiceBonus <=0)
        {
            canContinue = false;
        }
        else
        {
            setMultiplicateurBonus();
            generateFAC(true);
        }
        
    }

    private void setMultiplicateurBonus()
    {
        if(indiceBonus < 10)
        {
            MultiplicateurBonus = 1;
        }
        else if (indiceBonus < 12)
        {
            MultiplicateurBonus = 2;
        }
        else if (indiceBonus < 14)
        {
            MultiplicateurBonus = 4;
        }
        else if (indiceBonus < 16)
        {
            MultiplicateurBonus = 8;
        }
        else 
        {
            MultiplicateurBonus = 16;
        }

        if (OnMultiplierChanged != null)
            OnMultiplierChanged(MultiplicateurBonus);
    }


    // met a jour le semiValidate
    // met a jour le level
    // met a jour le speed
    // met a jour le cursor
    // met à jour le FAC
    private bool setCursor()
    {
        bool hasPassedLevel = false;
        
        if (!semiValidate)
        {
            semiValidate = true;
            generateFAC(true);
        }
        else if (_HasNextCursor())
            _NextCursor();
        else
        {
            _NextLevel();
            hasPassedLevel = true;
        }
        
        cursor = cursorTab[numCursor - 1];
        return hasPassedLevel;
    }

    private bool _HasNextCursor()
    {
        return numCursor <= numLevel && numCursor < cursorTab.Length;
    }

    private void _NextCursor()
    {
        numCursor++;
        generateFAC(false);
        semiValidate = false;

        if (OnCursorChanged != null)
            OnCursorChanged(0);
    }

    private void _NextLevel()
    {
        numLevel++;
        
        _UpdateSpeed();
        
        numCursor = 1;
        generateFAC(false);
        semiValidate = false;

        if (OnLevelChanged != null)
            OnLevelChanged(numLevel);
    }

    private void _UpdateSpeed()
    {
        speed *= speedIncrementation;
        if (speed > MAX_SPEED) 
            speed = MAX_SPEED;

        if (OnSpeedChanged != null)
            OnSpeedChanged(speed);
    }

    // genere la Figure A Fabriquer selon le curseur en cours
    private void generateFAC(bool memeCursor)
    {
        
        byte tempNumFac;
        do {
            tempNumFac = (byte)Random.Range(0, NB_FAC_PAR_CURSOR);
        } while (tempNumFac == numFac && memeCursor);

        Fac = FacTab[numCursor - 1, tempNumFac];

        if (OnBlockSetChanged != null)
            OnBlockSetChanged(0);
    }
}
