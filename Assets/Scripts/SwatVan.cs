using UnityEngine;

public class SwatVan : MonoBehaviour
{
    public float velocidad;

    public GameObject frontWheel;
    public GameObject backWheel;

    GameController _gameController;

    private void Awake()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);

        frontWheel.transform.Rotate(-5, 0, 0);
        backWheel.transform.Rotate(-5, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ChangePoint")
        {
            Debug.Log("Se cambiaron los modelos");
            _gameController.ChangeElements();
        }

        if (other.name == "Destroy Point")
        {
            Debug.Log("Se va a destruir el objeto");
            StartCoroutine(_gameController.RestartTimer());
            _gameController.Firstime = false;
            Destroy(this.gameObject);
        }
    }
}
