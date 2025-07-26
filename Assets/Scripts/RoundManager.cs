using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Plugins.Audio.Core;

public class RoundManager : MonoBehaviour
{

    [Header("Переменные")]
    [SerializeField] private bool isHardmode;
    private bool roundFinished = false;
    [SerializeField] private string answer;
    private float defeatTime;
    [SerializeField] private AudioClip correctAnswerSound;
    [SerializeField] private AudioClip IncorrectAnswerSound;

    [Header("Объекты")]
    [SerializeField] private GameObject gameManager;
    [SerializeField] private Button[] answers = new Button[4];
    [SerializeField] private Transform levelPosition;
    [SerializeField] private Text result;
    [SerializeField] private Text timeBeforeLose;
    [SerializeField] private Text opName;
    [SerializeField] private GameObject reviveOption;
    [SerializeField] private Text roundsAnswered;
    [SerializeField] private SourceAudio _source;



    public void InitializeRound(string[] answers_, string opKey, string answer, GameObject gameManager, float roundDuration, bool isHardmode, Transform levelPosition)
    {
        for (int i = 0; i < 4; i++)
        {
            answers[i].GetComponentInChildren<Text>().text = answers_[i];
        }
        this.answer  = answer;
        this.gameManager = gameManager;
        _source.Play(opKey);

        defeatTime = Time.time + roundDuration;
        this.isHardmode = isHardmode;
        this.levelPosition = levelPosition;

        roundsAnswered.text = String.Format("{0}/20", gameManager.GetComponent<GameController>().GetRoundNum());

    }

    private void Update()
    {
        if(defeatTime - Time.time >= 0)
        {
            timeBeforeLose.text = Math.Round(((decimal)(defeatTime - Time.time)),1).ToString();
        }
        else
        {
            timeBeforeLose.text = "0";
        }
        if(Time.time >= defeatTime && !roundFinished)
        {
            foreach (Button button in answers)
            {
                button.interactable = false;
            }
            result.text = "Время вышло!";
            result.color = Color.red;
            _source.Play(string.Format("Assets/Sounds/{0}.mp3", IncorrectAnswerSound.name));

            if (isHardmode)
            {
                StartCoroutine(Defeat());
            }
            else
            {
                StartCoroutine(StartNextRound());
            }
            
        }
    }


    public void CheckAnswer(Button clickedButton)
    {
        _source.Stop();
        if(clickedButton.GetComponentInChildren<Text>().text == answer)
        {
            _source.Play(string.Format("Assets/Sounds/{0}.mp3", correctAnswerSound.name));
            result.text = "Правильно!";
            result.color = Color.green;
            foreach(Button button in answers)
            {
                button.interactable = false;
            }

            gameManager.GetComponent<GameController>().UpdateScore(true);
            StartCoroutine(RoundPassed());
        }
        else
        {
            _source.Play(string.Format("Assets/Sounds/{0}.mp3", IncorrectAnswerSound.name));
            result.text = "Неправильно!";
            result.color = Color.red;
            opName.text = string.Format("Правильный ответ: {0}", answer);
            foreach (Button button in answers)
            {
                button.interactable = false;
            }

            if (isHardmode)
            {
                if (gameManager.GetComponent<GameController>().GetIsRevived())
                {
                    StartCoroutine(Defeat());
                }
                else
                {
                    roundFinished = true;
                    GameObject reviveMenu = Instantiate(reviveOption, levelPosition);
                    reviveMenu.GetComponent<ReviveMenu>().Initializer(gameManager, gameObject);
                }
                
            }
            else
            {
                StartCoroutine(StartNextRound());
                
            }

            
        }   
    }

    public void Back()
    {
        gameManager.GetComponent<GameController>().PlayButtonSound();
        gameManager.GetComponent<GameController>().BackFromLevel();
        gameManager.GetComponent<GameController>().CreateMenu();
        Destroy(this.gameObject);

    }

    IEnumerator RoundPassed()
    {
        roundFinished = true;
        gameManager.GetComponent<GameController>().AddPassedRound();
        yield return new WaitForSeconds(1);
        gameManager.GetComponent<GameController>().ContinueGame();
        Destroy(this.gameObject);
    }

    public IEnumerator StartNextRound()
    {
        
        roundFinished = true;
        yield return new WaitForSeconds(1);
        gameManager.GetComponent<GameController>().ContinueGame();
        Destroy(gameObject);

    }
    public IEnumerator Defeat()
    {
        roundFinished = true;
        yield return new WaitForSeconds(1);
        gameManager.GetComponent<GameController>().CreateDefeatMenu();
        Destroy(this.gameObject);
    }
}
