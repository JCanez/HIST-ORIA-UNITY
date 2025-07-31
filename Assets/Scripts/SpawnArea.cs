using UnityEngine;

[ExecuteAlways]
public class SpawnArea : MonoBehaviour
{
    [Header("Tamaño del area (local)")]
    public Vector3 areaSize = new Vector3(5, 1, 5);

    [Header("Prefab que se generara")]
    public GameObject prefabToSpawn;

    [Header("Escala aleatoria del objeto instanciado")]
    public Vector2 randomScaleRange = new Vector2(0.5f, 1.5f);

    [Header("Mostrar area en escena")]
    public Color gizmoColor = new Color(0, 1, 0, 0.3f);

    private GameObject instancia;

    private void Start()
    {
        if(prefabToSpawn != null)
        {
            Vector3 pos = GetRandomPositionInside();
            instancia = Instantiate(prefabToSpawn, pos, Quaternion.identity, transform); ;

            float randomScale = Random.Range(randomScaleRange.x, randomScaleRange.y);
            instancia.transform.localScale = Vector3.one * randomScale;
        }
    }

    Vector3 GetRandomPositionInside()
    {
        Vector3 halfSize = areaSize * 0.5f;

        float x = Random.Range(-halfSize.x, halfSize.x);
        float y = Random.Range(halfSize.y, halfSize.y);
        float z = Random.Range(halfSize.z, halfSize.z);

        return transform.position + transform.rotation * new Vector3(x,y,z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, areaSize);
    }
}
