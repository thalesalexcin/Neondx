using UnityEngine;
using System.Collections;

public class BDDManager : MonoBehaviour 
{
    public bool IsFinished { get; set; }
    public string Error { get; set; }

    public string[] playerPosition;
    public string[] lineBoard;
    public string[] listePseudo = new string[6];
    public string[] listeScore = new string[6];


    void Start()
    {
        Debug.Log("troll");
        loadScores();
    }

    IEnumerator WaitForRequest(WWW www)
    { 
        while (!www.isDone)
            yield return www;

        IsFinished = true;
        Error = www.error;
    }

    public void recordScore(string pseudo, uint score)
    {
        IsFinished = false;
        string url = "http://www.vindesf.fr/neon-enjmin/recordNewScore.php";
        WWWForm form = new WWWForm();
        form.AddField("pseudo", pseudo);
        form.AddField("score", score.ToString());
        WWW www = new WWW(url, form);
        
        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForLoadScore(WWW www)
    {
        while (!www.isDone)
            yield return www;

        // check for errors
        if (www.error == null)
        {
            string s = www.text;

            playerPosition = s.Split(':');


            lineBoard = playerPosition[1].Split(';');
            int i = 0;
            foreach (string str in lineBoard)
            {
                string[] tmp = str.Split('|');
                if (i < 6)
                {
                    listePseudo[i] = tmp[0];
                    listeScore[i] = tmp[1];
                }

                i++;

            }
        }

        IsFinished = true;
        Error = www.error;
    }

    public void loadScores()
    {
        IsFinished = false;
       
        string url = "http://www.vindesf.fr/neon-enjmin/loadScores.php";
        WWW scoresData = new WWW(url);
        StartCoroutine(WaitForLoadScore(scoresData));
    }

    public bool HasError 
    { 
        get 
        {
            return !string.IsNullOrEmpty(Error);
        } 
    }
}
