using UnityEngine;
using UnityEngine.UI;


public class DifficultyMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject hardMode;

    public void SetGameManager(GameObject gameManager)
    {
        this.gameManager = gameManager;
    }
    public void SetDifficultyAndStartGame(int difficulty)
    {
        gameManager.GetComponent<GameController>().PlayButtonSound();
        gameManager.GetComponent<GameController>().SetGameDifficulty(difficulty);
        gameManager.GetComponent<GameController>().StartNewGame();
        Destroy(this.gameObject);
    }

    public void SetHardmode()
    {
        gameManager.GetComponent<GameController>().PlayButtonSound();
        gameManager.GetComponent<GameController>().SetHardmode(hardMode.GetComponent<Toggle>().isOn);
    }

    public void BackToMenu()
    {
        gameManager.GetComponent<GameController>().PlayButtonSound();
        gameManager.GetComponent<GameController>().CreateMenu();
        Destroy(this.gameObject);
    }
}
