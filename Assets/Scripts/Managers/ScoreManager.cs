using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int AiTotalAmount;
    public int TotalShotsAmount;
          
    [SerializeField] TMP_Text ballsAmountTextPro;
    [SerializeField] Text StartPanelLevlText;

    [SerializeField] Text BestScoreText;
    [SerializeField] Text ScoreText;

    private int score;
    private int bestScore;

   
    public void InitScore()
    {
        bestScore = DataManager.GetBestScore();

        StartPanelLevlText.text = "Level " + (LevelManager.currentLevel +1 ).ToString();
        BestScoreText.text = "Best Score " + bestScore.ToString();        
    }

    public void StickmanAmount(int newAisAmount, int ballsAmount)
    {
        AiTotalAmount = newAisAmount;
        TotalShotsAmount = ballsAmount;

        ballsAmountTextPro.text = TotalShotsAmount.ToString();
    }
    

    public void AddScore(int scoreAmount)
    {
        score += scoreAmount;
        if (score >= bestScore)
        {
            bestScore = score;
            DataManager.SetBestScore(score);
        }

        ScoreText.text = score.ToString();

        AiTotalAmount--;

        if (AiTotalAmount == 0)
        {
            GameManager.self.levelManager.ScenePassed();
        }
    }    
   

    public void PlayerShooted()
    {

        TotalShotsAmount--;
        ballsAmountTextPro.text = TotalShotsAmount.ToString();

    }

    [SerializeField] string[] MessagesTexts;
    [SerializeField] Text MessageText;
    [SerializeField] Animator MessageAnimator;
    public void ShowMessage()
    {
        MessageText.text = MessagesTexts[Random.Range(0, MessagesTexts.Length)];

        MessageAnimator.Play("ShowMessage", -1, 0);
    }


}//
