using UnityEngine;
using System.Collections;

public class BDDManager : MonoBehaviour {

    public string[] lineBoard;
    public string[] listePseudo = new string[6];
    public string[] listeScore = new string[6];

    // Use this for initialization
    void Start () {
        loadScores();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator WaitForRequest(WWW www)
    { 
        yield return www;
     // check for errors
     if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
            string s = www.text;
            lineBoard = s.Split(';');
            int i = 0;
            foreach (string str in lineBoard)
            {
                string[] tmp = str.Split('|');
                if (i<6)
                {
                    listePseudo[i] = tmp[0];
                    listeScore[i] = tmp[1];
                }
                
                i++;

            }

        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void recordScore(string pseudo, uint score)
    {
        string url = "http://www.vindesf.fr/neon-enjmin/recordNewScore.php";
        WWWForm form = new WWWForm();
        form.AddField("pseudo", pseudo);
        form.AddField("score", score.ToString());
        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest(www));
    }


    IEnumerator WaitForLoadScore(WWW www)
    {
        yield return www;
        // check for errors
        if (www.error == null)
        {
            string s = www.text;
            print(s);
            
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void loadScores()
    {
        Debug.Log("troll2");
        string url = "http://www.vindesf.fr/neon-enjmin/loadScores.php";
        WWW scoresData = new WWW(url);
        StartCoroutine(WaitForRequest(scoresData));

        //string packData = scoresData.text;

        //print(packData);
    }
}
