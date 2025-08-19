using UnityEngine;
using UnityEngine.Rendering;

public class Test : MonoBehaviour
{
    public GameObject A;
    public GameObject B;
    public GameObject C;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetPhase(GamePhase.Before);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SetPhase(GamePhase.InGame);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SetPhase(GamePhase.End);
        }
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
        A.SetActive(phase == GamePhase.Before);
        B.SetActive(phase == GamePhase.InGame);
        C.SetActive(phase == GamePhase.End);
    }
}
