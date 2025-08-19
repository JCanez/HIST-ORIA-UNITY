using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class GameController : MonoBehaviour
{
    public int elementsToChange;
    int _success;
    int _mistakes;

    bool _gameReady;
    float _timerInGame;
    bool _firsTime = true;

    GameObject[] _listGO;
    List<GameObject> _listDoble = new List<GameObject>();

    [Header("Swat Van")]
    public GameObject respawnPoint;
    public GameObject destructionPoint;
    public GameObject SwatVanGO;

    AudioController _audioController;
    UIController _UIController;

    private void Awake()
    {
        Random.InitState(System.DateTime.Now.Millisecond + System.DateTime.Now.Second);

        _audioController = GetComponent<AudioController>();
        _UIController = GetComponent<UIController>();

        _gameReady = false;
        _timerInGame = 10;

        _success = 0;
        _mistakes = 3;
    }

    private void Start()
    {
        _listGO = GameObject.FindGameObjectsWithTag("ObjectToChange");
        CreateNewList();

        StartCoroutine(GamePhases());
    }

    private void Update()
    {
        if (_success == elementsToChange)
        {
            GameReady = false;

            _UIController.SetPhase(UIController.GamePhase.End);
            _UIController.EndGameTextChanger("WINNER");
        }
        else if (_mistakes == 0 || (_timerInGame < 0 && _firsTime == false))
        {
            GameReady = false;

            _UIController.SetPhase(UIController.GamePhase.End);
            _UIController.EndGameTextChanger("LOSER");
        }

        if (_timerInGame < 3)
        {
            _UIController.TimerInGame(true, Color.red);
        }
    }

    IEnumerator GamePhases()
    {
        yield return StartCoroutine(BeforeGamePhases());

        yield return StartCoroutine(InGamePhase());

        yield return StartCoroutine(EndGamePhase());
    }

    IEnumerator BeforeGamePhases()
    {
        yield return new WaitForSeconds(1.5f);

        _UIController.UpdateBeforeGameTimer(3);

        _UIController.StartAnimBackground();

        float timeToWaiting = 3f;

        while (timeToWaiting > 0)
        {
            yield return new WaitForSeconds(1f);
            timeToWaiting--;
            _UIController.UpdateBeforeGameTimer(timeToWaiting);
            yield return null;
        }
    }

    IEnumerator InGamePhase()
    {
        Debug.Log("Iniciamos segunda fase");
        _UIController.SetPhase(UIController.GamePhase.InGame); // Activamos el canvas de In Game

        while (_timerInGame > 0)
        {
            yield return new WaitForSeconds(1f);
            _timerInGame--;
            _UIController.UpdateInGameTimer(_timerInGame);
            yield return null;
        }

        // Activar transicion - Swat Van
        SwatVanRespawn();

        // Resetear el timer


    }

    IEnumerator EndGamePhase()
    {
        yield return null;
    }

    public IEnumerator RestartTimer()
    {
        //_UIController.RestarTimer(false, 10, Color.black);

        while (_timerInGame > 0)
        {
            yield return new WaitForSeconds(1f);
            _timerInGame--;
            //_timerInGameTxt.text = _timerInGame.ToString();
        }

        //if (_firsTime)
        //{
        //    SwatVanRespawn();
        //}
    }

    private void CreateNewList()
    {
        ResetStateGO();
        _listDoble.Clear();

        for (int x = 0; x < _listGO.Length; x++)
        {
            _listDoble.Add(_listGO[x]);
        }

        Shuffle();
    }

    private void ResetStateGO()
    {
        for (int x = 0; x < _listGO.Length; x++)
        {
            ObjectChanger objectChangerGO = _listGO[x].GetComponent<ObjectChanger>();

            objectChangerGO.Change = false;
        }
    }

    private void PrintList()
    {
        for (int x = 0; x < _listDoble.Count; x++)
        {
            Debug.Log("Elemento " + (x + 1) + ": " + _listDoble[x]);
        }

        Debug.Log("-----------------------------------------------------");
    }

    private void DeleteElement(int element)
    {
        //Debug.Log("ELEMENTO ELIMINADO: " + _listDoble[element]);
        _listDoble.Remove(_listDoble[element]);

        //PrintList();
    }

    // ESTA FUNCION SE LLAMA DESDE EL BOTON "CHANGE" CAMBIA EL TOTAL DE ELEMENTOS DEFINIDOS Y EL JUEGO SE DA POR INICIADO.
    public void ChangeElements()
    {
        CreateNewList();

        for (int x = 0; x < elementsToChange; x++)
        {
            int randonNum = Random.Range(0, _listDoble.Count);
            ObjectChanger objectChangerGO = _listDoble[randonNum].GetComponent<ObjectChanger>();

            objectChangerGO.ChangeObject();
            objectChangerGO.Change = true;

            Debug.Log(objectChangerGO.name);

            DeleteElement(randonNum);

            //PrintList();
        }

        _gameReady = true;
    }

    public void ReloadScene(int indexScene)
    {
        if (indexScene == 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else
            SceneManager.LoadScene(indexScene);
    }

    private void Shuffle()
    {
        Random.InitState(System.DateTime.Now.GetHashCode());

        for (int i = _listDoble.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            // swap
            (_listDoble[i], _listDoble[j]) = (_listDoble[j], _listDoble[i]);
        }
    }

    public void PlaySound(int value)
    {
        //if (value == 1)
        //    audioController.PlaySuccess();
        //else if (value == 2)
        //    audioController.PlayFail();
    }

    private void SwatVanRespawn()
    {
        GameObject swatVanGOI = Instantiate(SwatVanGO, respawnPoint.transform.position, Quaternion.identity);
        swatVanGOI.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
    }

    //ATRIBUTOS
    public int TotalGOChange
    {
        get { return elementsToChange; }
        set { elementsToChange = value; }
    }

    public bool GameReady
    {
        get { return _gameReady; }
        set { _gameReady = value; }
    }

    public bool Firstime
    {
        get { return _firsTime; }
        set { _firsTime = value; }
    }

    public int Mistakes
    {
        get { return _mistakes; }
        set { _mistakes = value; }
    }

    public int Success
    {
        get { return _success; }
        set { _success = value; }
    }
}
