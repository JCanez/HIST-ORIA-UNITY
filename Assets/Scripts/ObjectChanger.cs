using UnityEngine;

public class ObjectChanger : MonoBehaviour
{
    GameController _gameController;
    AudioController _audioController;

    GameObject newGO;
    MeshFilter meshFilterGO;
    MeshCollider meshColliderGO;

    public GameObject[] objectsToChange;
    public bool _change;
    bool _touched;

    int contador;

    private void Awake()
    {
        _touched = false;
        _change = false;

        Random.InitState(System.DateTime.Now.Millisecond + System.DateTime.Now.Second);
    }

    private void Start()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _audioController = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioController>();

        //Creamos los primeros modelos en escena
        CreateGameObject();
    }

    private void OnMouseDown()
    {
        if (_gameController.GameReady == true && _touched == false)
        {
            if (_change == true)
                _gameController.SuccessObject();
            else
                _gameController.MistakeObject();
            _touched = true;
        }
    }

    private void CreateGameObject()
    {
        newGO = Instantiate(objectsToChange[Random.Range(0, objectsToChange.Length)], transform.position, transform.rotation, transform);
        meshFilterGO = newGO.GetComponent<MeshFilter>();
    }

    private void CreateGameObject(int valor)
    {
        newGO = Instantiate(objectsToChange[valor], transform.position, transform.rotation, transform);
        meshFilterGO = newGO.GetComponent<MeshFilter>();
    }

    /*TENGO QUE CAMBIAR CIERTA CANTIDAD DE ELEMENTOS DE FORMA ALEATORIO, 
     * PUEDO CAMBIARLOS AQUI MISMO SIN PROBLEMA, PERO TENGO QUE HACER QUE LOS ELEMENTOS CAMBIAS SE MARQUEN,
     * AL PICARLOS CON EL MOUSE, ESTOS DEBEN DE AVISAR SI ERA CORRECTO O INCORRECTO*/

    //ESTE METODO EVALUA Y CAMBIA AL MODELO ACTUAL POR OTRO
    public void ChangeObject()
    {
        //int valor = Random.Range(0, objectsToChange.Length);

        //Debug.Log(objectsToChange[valor].name);

        //Mesh newMesh = objectsToChange[valor].GetComponent<MeshFilter>().sharedMesh;

        //if (meshFilterGO.sharedMesh != newMesh)
        //{
        //    Destroy(newGO);
        //    CreateGameObject(valor);
        //}
        //else
        //    ChangeObject();

        int valor;
        Mesh newMesh;

        do
        {
            valor = Random.Range(0, objectsToChange.Length);
            newMesh = objectsToChange[valor].GetComponent<MeshFilter>().sharedMesh;
        }
        while (meshFilterGO.sharedMesh == newMesh);

        Destroy(newGO);
        CreateGameObject(valor);
    }

    public bool Change
    {
        get { return _change; }
        set { _change = value; }
    }
}