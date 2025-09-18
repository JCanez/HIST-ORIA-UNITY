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

    public void CallScene(int sceneId)
    {
        PlayLevel.selectedLvl = menuController.CurrentLvl;
        SceneManager.LoadScene(sceneId);
    }

    public void NextLevel()
    {
        PlayLevel.selectedLvl++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
