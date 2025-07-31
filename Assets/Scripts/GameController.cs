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
    public int success;
    public int mistakes;

    bool _gameReady;

    GameObject[] listGO;

    List<GameObject> listDoble = new List<GameObject>();

    [Header("UI")]
    public GameObject beforeStartCanvas;
    public TMP_Text timerBeforeStartTxt;

    public GameObject inGameCanvas;
    public TMP_Text timerInGameTxt;
    public TMP_Text endGameTxt;
    float _timerInGame;
    bool _firstime = true;


    public GameObject gameOverCanvas;
    public TMP_Text totalObjectToChange;

    private void Awake()
    {
        Random.InitState(System.DateTime.Now.Millisecond + System.DateTime.Now.Second);

        _gameReady = false;
        success = 0;
        _timerInGame = 10;
    }

    private void Start()
    {
        totalObjectToChange = totalObjectToChange.GetComponent<TextMeshProUGUI>();

        listGO = GameObject.FindGameObjectsWithTag("ObjectToChange");
        totalObjectToChange.text = elementsToChange.ToString();

        CreateNewList();

        StartCoroutine(StartGame());
    }

    private void Update()
    {
        if (success == elementsToChange)
        {
            inGameCanvas.SetActive(false);
            gameOverCanvas.SetActive(true);

            endGameTxt.text = "WINNER";
        }
        else if (mistakes == 0 || _timerInGame < 0 && _firstime == false)
        {
            inGameCanvas.SetActive(false);
            gameOverCanvas.SetActive(true);

            endGameTxt.text = "GAME OVER";
            Debug.Log("GAME OVER");
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1.5f);

        timerBeforeStartTxt.text = "3";
        float duration = 3.0f;

        while (duration > 0)
        {
            duration -= Time.deltaTime;
            timerBeforeStartTxt.text = Mathf.CeilToInt(duration).ToString();

            yield return null;
        }

        beforeStartCanvas.SetActive(false);
        inGameCanvas.SetActive(true);

        StartCoroutine(RestartTimer());
    }

    IEnumerator RestartTimer()
    {
        _timerInGame = 10;
        timerInGameTxt.text = "10";

        while (_timerInGame > 0)
        {
            _timerInGame -= Time.deltaTime;
            timerInGameTxt.text = Mathf.CeilToInt(_timerInGame).ToString();

            yield return null;
        }

        if (_firstime)
        {
            ChangeElements();
            StartCoroutine(RestartTimer());
            _firstime = false;
        }
    }

    private void CreateNewList()
    {
        ResetStateGO();
        listDoble.Clear();

        for (int x = 0; x < listGO.Length; x++)
        {
            listDoble.Add(listGO[x]);
        }

        Shuffle();
    }

    private void ResetStateGO()
    {
        for (int x = 0; x < listGO.Length; x++)
        {
            ObjectChanger objectChangerGO = listGO[x].GetComponent<ObjectChanger>();

            objectChangerGO.Change = false;
        }
    }

    private void PrintList()
    {
        for (int x = 0; x < listDoble.Count; x++)
        {
            Debug.Log("Elemento " + (x + 1) + ": " + listDoble[x]);
        }

        Debug.Log("-----------------------------------------------------");
    }

    private void DeleteElement(int element)
    {
        //Debug.Log("ELEMENTO ELIMINADO: " + listDoble[element]);
        listDoble.Remove(listDoble[element]);

        //PrintList();
    }

    // ESTA FUNCION SE LLAMA DESDE EL BOTON "CHANGE" CAMBIA EL TOTAL DE ELEMENTOS DEFINIDOS Y EL JUEGO SE DA POR INICIADO.
    public void ChangeElements()
    {
        CreateNewList();

        for (int x = 0; x < elementsToChange; x++)
        {
            int randonNum = Random.Range(0, listDoble.Count);
            ObjectChanger objectChangerGO = listDoble[randonNum].GetComponent<ObjectChanger>();

            objectChangerGO.ChangeObject();
            objectChangerGO.Change = true;

            DeleteElement(randonNum);

            //PrintList();
        }

        _gameReady = true;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Shuffle()
    {
        Random.InitState(System.DateTime.Now.GetHashCode());

        for (int i = listDoble.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            // swap
            (listDoble[i], listDoble[j]) = (listDoble[j], listDoble[i]);
        }
    }

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

}
