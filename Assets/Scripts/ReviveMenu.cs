using UnityEngine;
using System;
using UnityEngine.UI;
using YG;

public class ReviveMenu : MonoBehaviour
{
    [SerializeField] Text defeatTime;
    private float timeToDefeat = 5;
    private float timeOfDefeat;
    private bool buttonClicked = false;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject currentRound;


    private void Update()
    {
        float timeLeft = timeOfDefeat - Time.time;
        if(timeLeft < 0)
        {
            YandexGame.RewardVideoEvent -= ContinueGame;
            defeatTime.text = "0";
            if (!buttonClicked)
            {
                FinishGame();
                Destroy(this.gameObject);
            }
        }
        else
        {
            
            defeatTime.text = Math.Round(((decimal)(timeLeft)), 1).ToString();
        }
    }
    public void Initializer(GameObject gameManager, GameObject currentRound)
    {
        YandexGame.RewardVideoEvent += ContinueGame;
        this.gameManager = gameManager;
        this.currentRound = currentRound;
        timeOfDefeat = Time.time + timeToDefeat;
    }
    public void ShowAd()
    {

        gameManager.GetComponent<GameController>().PlayButtonSound();
        buttonClicked = true;
        Debug.Log(currentRound.name);
        Debug.Log('f');
        YandexGame.RewVideoShow(0);
        
    }

    public void ContinueGame(int id)
    {
        YandexGame.RewardVideoEvent -= ContinueGame;
        if (id == 0)
        {
            
            gameManager.GetComponent<GameController>().ChangeIsRevived(true);
            
            currentRound.GetComponent<RoundManager>().StartCoroutine(currentRound.GetComponent<RoundManager>().StartNextRound());
            Destroy(this.gameObject);
        }
    }

    public void FinishGame()
    {
        YandexGame.RewardVideoEvent -= ContinueGame;
        buttonClicked = true;
        currentRound.GetComponent<RoundManager>().StartCoroutine(currentRound.GetComponent<RoundManager>().Defeat());
        Destroy(this.gameObject);
    }


}
