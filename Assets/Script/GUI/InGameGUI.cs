using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameGUI : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    float elapsedTime;
    void Start()
    {
        elapsedTime = 0;
        Settings.Instance.TemporaryScore = 50;
        //StartCoroutine(TrackTime());
    }
    public void Update()
    {
        if (!LevelManager.IsInMenu)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 1)
            {
                Settings.Instance.TemporaryScore--;
                Mathf.Clamp(Settings.Instance.TemporaryScore, 0, 1000);
                elapsedTime = 0;
            }
            //elapsedTimeText.text = "Elapsed Time: " + System.Math.Round(elapsedTime, 1);
            scoreText.text = "Score: " + Settings.Instance.TemporaryScore;

        }
        else
            scoreText.text = "";
    }
    private IEnumerator TrackTime()
    {
        while(true)
        {
            if(!LevelManager.IsInMenu)
            elapsedTime += 0.05f; //Eftersom denna funktionen uppdateras 20 gånger i sekunden har det gått 1/20 sekund för varje uppdatering
            yield return new WaitForSeconds(0.05f); 
        }
    }
}
