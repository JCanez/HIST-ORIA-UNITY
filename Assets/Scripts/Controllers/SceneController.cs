using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void CallScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
