using UnityEngine;

public class HowToPlay : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;

    public void SetGameManager(GameObject gameManager)
    {
        this.gameManager = gameManager;
    }

    public void BackToMenu()
    {
        gameManager.GetComponent<GameController>().CreateMenu();
        gameManager.GetComponent<GameController>().PlayButtonSound();
        Destroy(this.gameObject);
    }
}
