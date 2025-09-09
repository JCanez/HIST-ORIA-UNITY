using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    MenuController menuController;

    private void Start()
    {
        menuController = GetComponent<MenuController>();
    }

    public void CallScene(int sceneId)
    {
        PlayLevel.selectedLvl = menuController.CurrentLvl; 
        SceneManager.LoadScene(sceneId);
    }
}
