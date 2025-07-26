using UnityEngine;
using UnityEngine.UI;
using YG;


public class LeaderboardMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject notAuth;
    [SerializeField] private GameObject auth;
    [SerializeField] private Text currentScoreNotAuth;

    public void SetGameManager(GameObject gameManager)
    {
        this.gameManager = gameManager;
    }

    private void Start()
    {
        if (!YandexGame.auth)
        {
            notAuth.SetActive(true);
            currentScoreNotAuth.text = YandexGame.savesData.score.ToString();

        }
    }

    public void BackToMenu()
    {
        gameManager.GetComponent<GameController>().PlayButtonSound();
        gameManager.GetComponent<GameController>().CreateMenu();
        Destroy(this.gameObject);
    }

    public void Login()
    {
        YandexGame.AuthDialog();
    }

    public void ReloadMenu()
    {
        gameManager.GetComponent<GameController>().PlayButtonSound();
        gameManager.GetComponent<GameController>().CreateLeaderboardMenu();
        Destroy(this.gameObject);
    }
}
