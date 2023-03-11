using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.IO;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public RectTransform HighScorePanel;
    public List<TextMeshProUGUI> ScoreHolders;
    string PATH ;
    // Start is called before the first frame update
    void Start()
    {
        PATH = Application.dataPath + "/Scores";
        HighScorePanel.DOScaleX(1f, 3f);
        var currentScores = loadScores();
        for (int i = 0; i < ScoreHolders.Count; i++)
        {
            if(i<currentScores.Length)
            {
                var scoreData = currentScores[i].Split(',');
                if(scoreData.Length>=2)
                ScoreHolders[i].text = scoreData[0]+"          "+ scoreData[1];
            }
        }
        
    }
    public void AddScore(int score, string name)
    {
        if(!Directory.Exists(PATH))
        {
            Directory.CreateDirectory(PATH);
        }
        if(!File.Exists(PATH +"/Score.txt"))
        {
            var writer= File.CreateText(PATH+"/Score.txt");
            writer.WriteLine(score+ ","+name+"\n");
            writer.Close();
        }
        else
        {
            var currentScores = loadScores();
            File.WriteAllText(PATH + "/Score.txt", "");
            List<string> newScores= new List<string>();
            bool isHighScore=false;
            for (int i = 0; i < currentScores.Length; i++)
            {
                var currentScore = currentScores[i];
                int scoreAtIndex = 0;
                if (currentScore.Contains(",") && currentScore.Split(',').Length == 2 && int.TryParse(currentScore.Split(',')[0], out scoreAtIndex))
                {
                    if (!isHighScore &&  score > scoreAtIndex)
                    {
                        
                        newScores.Add(score.ToString()+","+name);
                        newScores.Add(currentScore.Split(',')[0]+","+currentScore.Split(',')[1]);
                        isHighScore = true;
                    }
                    else
                    {
                        newScores.Add(currentScore.Split(',')[0] + "," + currentScore.Split(',')[1]);
                    }
                    
                }
                else
                {
                    newScores.Add(score.ToString() + "," + name);
                }
            }
            if (newScores.Count > 0)
            {
                foreach (var Score in newScores)
                {
                    File.AppendAllText(Application.dataPath + "/Scores/Score.txt", Score.Split(',')[0] + "," + Score.Split(',')[1] + "\n");

                }
                if (!isHighScore)
                    File.AppendAllText(Application.dataPath + "/Scores/Score.txt", score + "," + name + "\n");
            }
            else
            {
                File.AppendAllText(Application.dataPath + "/Scores/Score.txt", score+","+name+ "\n");
            }

                 
        }
    }

    public string[] loadScores()
    {
        if (Directory.Exists(PATH) && File.Exists(PATH + "/Score.txt"))
        {

            return File.ReadAllLines(PATH +"/Score.txt");
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            AddScore(Random.Range(1000, 99999), "JPR");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var scores = loadScores();
            Debug.Log(scores.Length);
            foreach (var score in scores)
            {
                Debug.Log(score);
            }
            {

            }
        }
    }
}
