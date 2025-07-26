using UnityEngine;
using UnityEngine.UI;
using YG;

public class VictoryMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    [SerializeField] private Text scoreResult;
    [SerializeField] private Text passedRounds;

    public void Initializer(GameObject gameManager, int roundsPassed)
    {
        this.gameManager = gameManager;
        int score = gameManager.GetComponent<GameController>().GetScore();
        scoreResult.text = score.ToString();
        SetPassedRounds(roundsPassed);
        
    }

    private void SetPassedRounds(int roundsPassed)
    {
        passedRounds.text = string.Format("{0}/20", roundsPassed);
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
