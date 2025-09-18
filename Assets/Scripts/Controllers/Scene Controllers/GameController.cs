using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;
using UnityEditor.ShaderKeywordFilter;

public class GameController : MonoBehaviour
{
    GameData _gameData;

    public int elementsToChange;
    int _success;
    int _mistakes;

    bool _gameReady;
    float _timerInGame;
    bool _firsTime = true;
    bool _gameover = false;
    bool _newRecord = false;

    float _timekeeper;

    GameObject[] _listGO;
    List<GameObject> _listDoble = new List<GameObject>();

    [Header("Objects to change")]
    [SerializeField]
    private GameObject[] _obtElements;

    [Header("Camera")]
    [SerializeField]
    private GameObject[] _position;
    private GameObject _mainCamera;

    [Header("Swat Van")]
    public GameObject respawnPoint;
    public GameObject destructionPoint;
    public GameObject SwatVanGO;

    [Header("UI")]
    [SerializeField]
    GameObject nextLvlButton;

    AudioController _audioController;
    UIController _UIController;
    InfoGameManager _infoGameManager;

    private void Awake()
    {
        SaveSystem.PrintPath();
        _gameData = SaveSystem.Load();

        Random.InitState(System.DateTime.Now.Millisecond + System.DateTime.Now.Second);

        _audioController = GetComponent<AudioController>();
        _UIController = GetComponent<UIController>();
        _infoGameManager = GetComponent<InfoGameManager>();

        _gameReady = false;
        _timerInGame = 3;

        _success = 0;
        _mistakes = 0;
    }

    private void Start()
    {
        Debug.Log("Nivel: " + PlayLevel.selectedLvl + " , Mejor tiempo: " + _gameData.levels[PlayLevel.selectedLvl].bestTime);

        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        _mainCamera.transform.position = _position[PlayLevel.selectedLvl].transform.position;
        _mainCamera.transform.rotation = _position[PlayLevel.selectedLvl].transform.rotation;

        _obtElements[PlayLevel.selectedLvl].SetActive(true);

        _listGO = GameObject.FindGameObjectsWithTag("ObjectToChange");
        CreateNewList();

        StartCoroutine(GamePhases());
    }

    private void Update()
    {
        if (_success == elementsToChange)
        {
            _gameReady = false;

            //_UIController.SetPhase(UIController.GamePhase.End);
            _UIController.EnableCanvas();
            _UIController.EndGameTextChanger("WINNER");

            _gameover = true;

            _infoGameManager.GuardarTiempo(_timekeeper);

            VerificarData();
        }
        else if (_mistakes == 2 || (_timerInGame <= 0 && _firsTime == false))
        {
            _gameReady = false;
            _gameover = true;

            //_UIController.SetPhase(UIController.GamePhase.End);
            _UIController.EnableCanvas();
            _UIController.EndGameTextChanger("YOU LOSE");
        }

        if (_timerInGame < 4 && _firsTime == false)
        {
            _UIController.TimerInGame(true, Color.red);
        }

        _timekeeper += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveSystem.DeleteFile();
        }
    }

    IEnumerator GamePhases()
    {
        yield return StartCoroutine(BeforeGamePhases());

        yield return StartCoroutine(InGamePhase());

        //yield return StartCoroutine(EndGamePhase());
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
        _UIController.SetPhase(UIController.GamePhase.InGame); // Activamos el canvas de In Game

        while (_timerInGame > 0)
        {
            yield return new WaitForSeconds(1f);

            _timerInGame--;
            _UIController.UpdateInGameTimer(_timerInGame, "0");

            yield return null;
        }

        _UIController.TimerGameObjectActive(false);

        // Activar transicion - Swat Van
        //SwatVanRespawn();
        //yield return new WaitForSeconds(4.25f);

        ChangeElements();
        yield return new WaitForSeconds(1);

        RestartTimer(10);
        _firsTime = false;

        _timekeeper = 0;

        while (_timerInGame > 0 && _gameover == false)
        {
            //yield return new WaitForSeconds(1f);

            _timerInGame -= Time.deltaTime;
            _UIController.UpdateInGameTimer(_timerInGame, "00.00");

            yield return null;
        }
    }

    IEnumerator EndGamePhase()
    {
        yield return null;
    }

    public void SuccessObject()
    {
        _success++;
        _audioController.PlaySuccess();
        _UIController.SuccessOn(_success - 1);
    }

    public void MistakeObject()
    {
        _mistakes++;
        _audioController.PlayFail();
        _UIController.MistakeOn(_mistakes - 1);
    }

    public void RestartTimer(int time)
    {
        _timerInGame = time;
        _UIController.TimerGameObjectActive(true);
        _UIController.ResetTimer(false, time, Color.black);
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
        // SceneManager.GetActiveScene().buildIndex

        if (_newRecord == true)
        {
            PlayLevel.selectedLvl = PlayLevel.selectedLvl + 1;
        }

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

    private void VerificarData()
    {
        if (_timekeeper < _gameData.levels[PlayLevel.selectedLvl].bestTime)
        {
            _gameData.levels[PlayLevel.selectedLvl].bestTime = _timekeeper;

            if (_timekeeper <= 5 && _gameData.levels[PlayLevel.selectedLvl + 1].unlocked == false)
            {
                _gameData.levels[PlayLevel.selectedLvl + 1].unlocked = true;
                _newRecord = true;
                nextLvlButton.SetActive(true);
            }

            SaveSystem.Save(_gameData);
            Debug.Log("Nuevo record");
        }
        else
        {
            Debug.Log("No se mejoro el record");
        }
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

public static class PlayLevel
{
    public static int selectedLvl;
}