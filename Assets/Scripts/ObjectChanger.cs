using System.Collections.Generic;
using UnityEngine;

public class ObjectChanger : MonoBehaviour
{
    GameController _gameController;

    GameObject currentInstance;   // instancia en la escena
    GameObject currentPrefab;     // referencia al prefab de origen

    public List<GameObject> objectsToChange;
    public bool _change;
    bool _touched;

    private void Awake()
    {
        _touched = false;
        _change = false;

        Random.InitState(System.DateTime.Now.Millisecond + System.DateTime.Now.Second);
    }

    private void Start()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        CreateGameObject();
    }

    private void OnMouseDown()
    {
        if (_gameController.GameReady && !_touched)
        {
            if (_change)
                _gameController.SuccessObject();
            else
                _gameController.MistakeObject();

            _touched = true;
        }
    }

    private void CreateGameObject()
    {
        currentPrefab = objectsToChange[Random.Range(0, objectsToChange.Count)];
        currentInstance = Instantiate(currentPrefab, transform.position, transform.rotation, transform);
    }

    public void ChangeObject()
    {
        // Creamos una lista temporal sin el prefab actual
        List<GameObject> opciones = new List<GameObject>(objectsToChange);
        opciones.Remove(currentPrefab);

        // Escogemos uno distinto
        currentPrefab = opciones[Random.Range(0, opciones.Count)];

        // Cambiamos la instancia
        Destroy(currentInstance);
        currentInstance = Instantiate(currentPrefab, transform.position, transform.rotation, transform);
    }

    public bool Change
    {
        get { return _change; }
        set { _change = value; }
    }
}
