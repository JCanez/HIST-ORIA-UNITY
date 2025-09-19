using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    MenuController menuController;
    GameController gameController;

    private void Start()
    {
        menuController = GetComponent<MenuController>();
        gameController = GetComponent<GameController>();
    }

    public void CallLevel(int indexScene)
    {
        PlayLevel.selectedLvl = menuController.CurrentLvl;
        SceneManager.LoadScene(indexScene);
    }

    public void CallScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void CallScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        PlayLevel.selectedLvl++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
