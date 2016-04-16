using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    // constantes
    const int NB_CURSOR = 4;
    const int NB_FAC_PAR_CURSOR = 5;
    const int PERTE_INDICE_BONUS = 1; // correspond a un demi cran
    const int GAIN_INDICE_BONUS = 1; //correspond a un demi cran
    const int SCORE_WIN_BASE = 500;
    const float MAX_SPEED = 8;

    // variables accessible de l'exterieur
    public uint score = 0;
    public ushort numLevel = 1;
    public GameObject cursor;
    public GameObject Fac;
    public float speed = 1;
    public float speedIncrementation = 1.2f;
    public int indiceBonus = 8;
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
	
	// Update is called once per frame
	void Update () {
	
	}

    private void initCursor()
    {
        //chargement des cursors depuis les assets
        string s;
        for (int i = 0; i < NB_CURSOR; i++)
        {
            s = "Assets/Prefabs/Cursor";
            s += (i + 1).ToString() + ".prefab";
            cursorTab[i] = ((GameObject)AssetDatabase.LoadAssetAtPath(s, typeof(GameObject)));
            initFAC(i);
        }
    }

    private void initFAC(int numCursor)
    {
        string s;
        for(int i = 0; i < NB_FAC_PAR_CURSOR; i++)
        {
            s = "Assets/Prefabs/ValidBlocks/Fac_" + (numCursor+1).ToString() + "_";
            s += (i+1).ToString() + ".prefab";
            FacTab[numCursor,i] = ((GameObject)AssetDatabase.LoadAssetAtPath(s, typeof(GameObject)));
        }
    }

    public bool hasNextLevel()
    {
        return canContinue;
    }

    public void nextLevel(bool isValid)
    {
        if (isValid) win();
        else loose();
    }

    // la grille precedente etait valide
    private void win()
    {
        score += (uint) SCORE_WIN_BASE * MultiplicateurBonus;
        indiceBonus += GAIN_INDICE_BONUS;
        setMultiplicateurBonus();
        setCursor();
    }

    // la grille precedente n'etait pas valide
    private void loose()
    {
        indiceBonus -= PERTE_INDICE_BONUS;
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
    }


    // met a jour le semiValidate
    // met a jour le level
    // met a jour le speed
    // met a jour le cursor
    // met à jour le FAC
    private void setCursor()
    {
        if(numLevel == 1) 
        {
            if (!semiValidate) // il faut deux coups pour passer au curseur suivant
            {
                semiValidate = true;
                generateFAC(true);
            }
            else if(numCursor == 1) // si je suis au curseur 1 je passe au 2
            {
                numCursor++;
                generateFAC(false);
                semiValidate = false;
            }
            else // si je suis au curseur 2 je change de niveau
            {
                numLevel++;
                speed *= speedIncrementation;
                if (speed > MAX_SPEED) speed = MAX_SPEED; 
                numCursor = 1;
                generateFAC(false);
                semiValidate = false;
            }
        }
        else if (numLevel == 2)
        {
            if (!semiValidate)
            {
                semiValidate = true;
                generateFAC(true);
            }
            else if (numCursor == 1 || numCursor == 2)
            {
                numCursor++;
                generateFAC(false);
                semiValidate = false;
            }
            else
            {
                numLevel++;
                speed *= speedIncrementation;
                if (speed > MAX_SPEED) speed = MAX_SPEED;
                numCursor = 1;
                generateFAC(false);
                semiValidate = false;
            }
        }
        else
        {
            if (!semiValidate)
            {
                semiValidate = true;
                generateFAC(true);
            }
            else if (numCursor == 1 || numCursor == 2 || numCursor == 3)
            {
                numCursor++;
                generateFAC(false);
                semiValidate = false;
            }
            else
            {
                numLevel++;
                speed *= speedIncrementation;
                if (speed > MAX_SPEED) speed = MAX_SPEED;
                numCursor = 1;
                generateFAC(false);
                semiValidate = false;
            }
        }

        cursor = cursorTab[numCursor - 1];
    }

    // genere la Figure A Fabriquer selon le curseur en cours
    private void generateFAC(bool memeCursor)
    {
        byte tempNumFac;
        do {
            tempNumFac = (byte)Random.Range(0, NB_FAC_PAR_CURSOR);
        } while (tempNumFac == numFac && memeCursor);

        Fac = FacTab[numCursor - 1, tempNumFac];
    }
}
