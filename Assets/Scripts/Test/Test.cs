using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    public GameObject[] A;
    private int pointer;
    public GameObject camera;

    private void Start()
    {
        pointer = PlayLevel.selectedLvl;

        camera.transform.position = A[pointer].transform.position;
        camera.transform.rotation = A[pointer].transform.rotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            pointer++;

            if (pointer >= 10)
                pointer = 0;

            camera.transform.position = A[pointer].transform.position;
            camera.transform.rotation = A[pointer].transform.rotation;
        }
    }

    public void SceneCaller(int idScene)
    {
        SceneManager.LoadScene(idScene);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("Tocado");
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(other.name);
    //    Debug.Log(other.tag);
    //}

    //public void Printing()
    //{
    //    Debug.Log("Funciono");
    //}

    public enum GamePhase { Before, InGame, End }

    public void SetPhase(GamePhase phase)
    {
        //A.SetActive(phase == GamePhase.Before);
        //B.SetActive(phase == GamePhase.InGame);
        //C.SetActive(phase == GamePhase.End);
    }
}
