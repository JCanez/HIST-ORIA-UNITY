using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Before Game")]
    public GameObject beforeGameCanvas;
    public TMP_Text timerBeforeStartTxt; //Timer de 3 segundos que se muestra en el canvas "beforeStartCanvas"
    [SerializeField] Animator _lineImageAnim;

    [Header("In Game")]
    public GameObject inGameCanvas;
    public TMP_Text totalObjectToChange; // Cantidad de objetos que han cambiado
    public GameObject timerInGameGO;
    float _timerInGame;
    TMP_Text _timerInGameTxt; // Tiempo para visualizar y/o encontrar los objetos que han cambiasdo
    Animator _timerInGameAnim;

    [Header("End Game")]
    public GameObject endGameCanvas;
    public TMP_Text endGameTxt; // Texto que se muestra al finalizar la partida y despliega "WINNER" o "LOSER"

    public enum GamePhase { Before, InGame, End }

    private void Awake()
    {
        _timerInGameTxt = timerInGameGO.GetComponent<TMP_Text>();
        _timerInGameAnim = timerInGameGO.GetComponent<Animator>();
    }

    private void Start()
    {
        totalObjectToChange = totalObjectToChange.GetComponent<TextMeshProUGUI>();
        //totalObjectToChange.text = elementsToChange.ToString();
    }

    public IEnumerator BeforeGamePhase()
    {
        yield return new WaitForSeconds(1.5f);

        _lineImageAnim.enabled = true;
        timerBeforeStartTxt.text = "3";

        float duration = 3.0f;

        while (duration > 0)
        {
            duration -= Time.deltaTime;
            timerBeforeStartTxt.text = Mathf.CeilToInt(duration).ToString();

            yield return null;
        }
    }

    public void UpdateBeforeGameTimer(float time)
    {
        timerBeforeStartTxt.text = Mathf.CeilToInt(time).ToString();
    }

    public void UpdateInGameTimer(float time)
    {
        _timerInGameTxt.text = Mathf.CeilToInt(time).ToString();
    }

    public void StartAnimBackground()
    {
        _lineImageAnim.enabled = true;
    }

    public void SetPhase(GamePhase phase)
    {
        beforeGameCanvas.SetActive(phase == GamePhase.Before);
        inGameCanvas.SetActive(phase == GamePhase.InGame);
        endGameCanvas.SetActive(phase == GamePhase.End);
    }

    public void EndGameTextChanger(string text)
    {
        endGameTxt.text = text;
    }

    public void TimerInGame(bool animState, Color color)
    {
        _timerInGameAnim.enabled = animState;
        _timerInGameTxt.color = color;
    }

    public void ResetTimer(bool enable, float totalTime, Color color)
    {
        _timerInGameAnim.enabled = enable;
        _timerInGame = totalTime;
        _timerInGameTxt.text = _timerInGame.ToString();
        _timerInGameTxt.color = color;
    }

    // ATRIBUTOS
}
