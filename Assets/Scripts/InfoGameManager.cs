using UnityEngine;

public class InfoGameManager : MonoBehaviour
{
    public static InfoGameManager Instance;

    private void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject); // Persistente entre escenas
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    public void GuardarTiempo(float tiempoActual)
    {
        if (PlayerPrefs.HasKey("MejorTiempo"))
        {
            float mejorTiempo = PlayerPrefs.GetFloat("MejorTiempo");
            if (tiempoActual < mejorTiempo)
            {
                PlayerPrefs.SetFloat("MejorTiempo", tiempoActual);
                PlayerPrefs.Save();

                Debug.Log("Nuevo mejor tiempo: " + tiempoActual);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("MejorTiempo", tiempoActual);
            PlayerPrefs.Save();

            Debug.Log("Primer tiempo: " + tiempoActual);
        }
    }

    public float ObtenerMejorTiempo()
    {
        return PlayerPrefs.GetFloat("MejorTiempo", -1f); // -1 si no existe
    }
}
