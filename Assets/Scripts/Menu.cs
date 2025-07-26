using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;


    public void SetGameManagaer(GameObject gameManager)
    {
        this.gameManager = gameManager;
    }

    public void OpenDifficultyMenu()
    {
        gameManager.GetComponent<GameController>().PlayButtonSound();
        gameManager.GetComponent<GameController>().CreateDifficultyMenu();
        Destroy(gameObject);
    }

    public void OpenHowToPlayMenu()
    {
        gameManager.GetComponent<GameController>().PlayButtonSound();
        gameManager.GetComponent<GameController>().CreateHowToPlay();
        Destroy(gameObject);
    }

    public void OpenLeaderboard()
    {
        gameManager.GetComponent<GameController>().PlayButtonSound();
        gameManager.GetComponent<GameController>().CreateLeaderboardMenu();
        Destroy(gameObject);
    }



}
