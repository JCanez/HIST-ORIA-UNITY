using UnityEngine;
using UnityEngine.Rendering;

public class Test : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Tocado");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Activado");
    }

    public void Printing()
    {
        Debug.Log("Funciono");
    }
}
