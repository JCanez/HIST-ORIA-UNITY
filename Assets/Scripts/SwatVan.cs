using UnityEngine;

public class SwatVan : MonoBehaviour
{
    public float velocidad;

    void Start()
    {

    }

    void Update()
    {
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }
}
