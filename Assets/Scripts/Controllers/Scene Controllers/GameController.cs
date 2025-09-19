using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class GameController : MonoBehaviour
{
    GameData _gameData;

    [SerializeField]
    int _elementsToChange;
    int _success;

    [SerializeField]
    int _totalMistakes;
    int _mistakes;

    bool _gameReady;
    bool _firstTime = true;
    bool _gameover = false;
    bool _newRecord = false;

    [Header("Time")]
    [SerializeField]
    float _timerBeforeGame; //TIEMPO QUE SE DARA AL JUGADOR PARA MEMORIZAR LA ESCENA
    [SerializeField]
    float _timerInGame; //TIEMPO QUE SE DARA AL JUGADOR PARA MEMORIZAR LA ESCENA
    [SerializeField]
    float _timeToWait; //TIEMPO DE ESPERA DEL PRIMER BANNER, ESTE DURA LO QUE DURA LA ANIMACION (3s)
    float _timekeeper; //TIEMPO TRANSCURRIDO EN IDENTIFICAR LOS OBJETOS CAMBIADOS, SE COMPARA CON EL RECORD

    GameObject[] _allObjectsToChange;
    List<GameObject> _availableObjects;

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
    GameObject _nextLvlButton;

    AudioController _audioController;
    UIController _UIController;
    InfoGameManager _infoGameManager;
    SceneController _sceneController;

    private void Awake()
    {
        SaveSystem.PrintPath();
        _gameData = SaveSystem.Load();

        Random.InitState(System.DateTime.Now.Millisecond + System.DateTime.Now.Second);

        _audioController = GetComponent<AudioController>();
        _UIController = GetComponent<UIController>();
        _infoGameManager = GetComponent<InfoGameManager>();
        _sceneController = GetComponent<SceneController>();

        _gameReady = false;

        _timekeeper = 0;
    }

    private void Start()
    {
        Debug.Log("Nivel: " + PlayLevel.selectedLvl + " , Mejor tiempo: " + _gameData.levels[PlayLevel.selectedLvl].bestTime);

        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        _mainCamera.transform.position = _position[PlayLevel.selectedLvl].transform.position;
        _mainCamera.transform.rotation = _position[PlayLevel.selectedLvl].transform.rotation;

        _obtElements[PlayLevel.selectedLvl].SetActive(true);

        _allObjectsToChange = GameObject.FindGameObjectsWithTag("ObjectToChange");
        CreateNewList();

        if (_gameData.levels[PlayLevel.selectedLvl + 1].unlocked == true)
            _nextLvlButton.SetActive(true);

        StartCoroutine(GamePhases());
    }

    private void Update()
    {
        if (_firstTime == false && _gameover == false)
            _timekeeper += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveSystem.DeleteFile();
        }
    }

    IEnumerator GamePhases()
    {
        yield return BeforeGamePhases();
        yield return InGamePhase();
    }

    IEnumerator BeforeGamePhases()
    {
        yield return new WaitForSeconds(1.5f);

        _UIController.UpdateBeforeGameTimer(_timeToWait);

        _UIController.StartAnimBackground();

        for (float t = _timeToWait; t > 0; t--)
        {
            _UIController.UpdateBeforeGameTimer(t);
            yield return new WaitForSeconds(1f);
        }

    }

    IEnumerator InGamePhase()
    {
        _UIController.SetPhase(UIController.GamePhase.InGame); // Activamos el canvas de In Game

        float timeLeft = _timerBeforeGame;
        for (; timeLeft > 0; timeLeft--)
        {
            _UIController.UpdateInGameTimer(timeLeft, "0");
            yield return new WaitForSeconds(1f);
        }

        _UIController.TimerGameObjectActive(false);

        yield return new WaitForSeconds(1.0f); //TIEMPO DE LA TRANSICION

        ChangeElements();

        yield return new WaitForSeconds(1);

        RestartTimer(Mathf.RoundToInt(_timerInGame));
        _firstTime = false;

        timeLeft = _timerInGame;

        while (timeLeft > 0 && _gameover == false)
        {
            timeLeft -= Time.deltaTime;
            _UIController.UpdateInGameTimer(timeLeft, "00.00");

            //CAMBIAMOS EL COLOR DEL TIMER
            if (timeLeft < 4)
            {
                _UIController.TimerInGame(true, Color.red);
            }

            yield return null;
        }

        GameOver("YOU LOSE");
    }

    public void SuccessObject()
    {
        _success++;
        _audioController.PlaySuccess();
        _UIController.SuccessOn(_success - 1);

        //SI ACIERTA A TODOS LOS OBJETOS
        if (_success == _elementsToChange)
        {
            GameOver("WINNER");

            _infoGameManager.GuardarTiempo(_timekeeper);

            VerificarData();
        }
    }

    public void MistakeObject()
    {
        _mistakes++;
        _audioController.PlayFail();
        _UIController.MistakeOn(_mistakes - 1);

        if (_mistakes == _totalMistakes)
            GameOver("YOU LOSE");
    }

    public void RestartTimer(int time)
    {
        _timerInGame = time;
        _UIController.TimerGameObjectActive(true);
        _UIController.ResetTimer(false, time, Color.black);
    }

    private void CreateNewList()
    {
        _availableObjects = new List<GameObject>();
        ResetStateGO();
        _availableObjects.Clear();

        for (int x = 0; x < _allObjectsToChange.Length; x++)
        {
            _availableObjects.Add(_allObjectsToChange[x]);
        }

        Shuffle();
    }

    private void ResetStateGO()
    {
        for (int x = 0; x < _allObjectsToChange.Length; x++)
        {
            ObjectChanger objectChangerGO = _allObjectsToChange[x].GetComponent<ObjectChanger>();

            objectChangerGO.Change = false;
        }
    }

    private void DeleteElement(int element)
    {
        //Debug.Log("ELEMENTO ELIMINADO: " + _availableObjects[element]);
        _availableObjects.Remove(_availableObjects[element]);

        //PrintList();
    }

    public void ChangeElements()
    {
        CreateNewList();

        for (int x = 0; x < _elementsToChange; x++)
        {
            int randonNum = Random.Range(0, _availableObjects.Count);
            ObjectChanger objectChangerGO = _availableObjects[randonNum].GetComponent<ObjectChanger>();

            objectChangerGO.ChangeObject();
            objectChangerGO.Change = true;

            //Debug.Log(objectChangerGO.name);

            DeleteElement(randonNum);
        }

        _gameReady = true;
    }

    private void Shuffle()
    {
        //Random.InitState(System.DateTime.Now.GetHashCode());

        for (int i = _availableObjects.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            // swap
            (_availableObjects[i], _availableObjects[j]) = (_availableObjects[j], _availableObjects[i]);
        }
    }

    public void PlaySound(int value)
    {
        //if (value == 1)
        //    audioController.PlaySuccess();
        //else if (value == 2)
        //    audioController.PlayFail();
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
                _nextLvlButton.SetActive(true);
            }

            SaveSystem.Save(_gameData);
            Debug.Log("Nuevo record");
        }
        else
        {
            Debug.Log("No se mejoro el record");
        }
    }

    private void GameOver(string message)
    {
        _gameReady = false;
        _gameover = true;

        //_UIController.SetPhase(UIController.GamePhase.End);
        _UIController.EnableCanvas();
        _UIController.EndGameTextChanger(message);
    }

    //ATRIBUTOS
    public bool GameReady
    {
        get { return _gameReady; }
        set { _gameReady = value; }
    }
}

public static class PlayLevel
{
    public static int selectedLvl;
}