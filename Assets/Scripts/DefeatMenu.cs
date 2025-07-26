using UnityEngine;
using UnityEngine.UI;
using YG;

public class DefeatMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    [SerializeField] private Text scoreResult;

    public void Initializer(GameObject gameManager)
    {
        this.gameManager = gameManager;
        int score = gameManager.GetComponent<GameController>().GetScore();
        scoreResult.text = score.ToString();
        

    }
    public void Retry()
    {

        gameManager.GetComponent<GameController>().PlayButtonSound();
        gameManager.GetComponent<GameController>().ResetGameParametrs();
        gameManager.GetComponent<GameController>().StartNewGame();
        YandexGame.FullscreenShow();
        Destroy(this.gameObject);
    }

    public void OpenMenu()
    {
        gameManager.GetComponent<GameController>().PlayButtonSound();
        gameManager.GetComponent<GameController>().ResetGameParametrs();
        gameManager.GetComponent<GameController>().CreateMenu();
        YandexGame.FullscreenShow();
        Destroy(this.gameObject);
    }


}
