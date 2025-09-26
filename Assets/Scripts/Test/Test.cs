using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    public List<GameObject> Lista;
    public GameObject objeto;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Lista.Remove(objeto);
        }
    }
}