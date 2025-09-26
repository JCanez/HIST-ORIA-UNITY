using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    private GameData _gameData;
    private LevelStarsUI _levelStarsUI;

    private int _currentLvl;

    [Header("UI")]
    [SerializeField]
    private Image _mainBackground;
    [SerializeField]
    private Sprite[] _backgrounds;
    [SerializeField]
    private TMP_Text _lvlNameTxt;
    [SerializeField]
    private TMP_Text _lvlNumText;
    [SerializeField]
    private TMP_Text _bestTimeText;
    [SerializeField]
    private GameObject _LeftArrow;
    [SerializeField]
    private GameObject _RightArrow;
    [SerializeField]
    private GameObject _lockElements;

    string[] _lvlNames = { "Outside", "Entrance 01", "Office 01", "Entrance 02", "Desk", "Cashier", "Safe", "Office 02", "Jewellery", "Exit" };

    private void Awake()
    {
        _levelStarsUI = gameObject.GetComponent<LevelStarsUI>();
    }

    private void Start()
    {
        _currentLvl = PlayLevel.selectedLvl;

        _gameData = SaveSystem.Load();

        if (_gameData.levels.Count == 0)
        {
            Debug.Log("Objeto vacio, se crean niveles");
            CreateLvls();
        }
        else
        {
            Debug.Log("Ya existe los niveles");
        }

        UpdateUI();
    }

    public void MoveLevel(bool side)
    {
        if (side)
        {
            _currentLvl++;

            if (_currentLvl == 9)
            {
                _RightArrow.SetActive(false);
            }
            else
            {
                _LeftArrow.SetActive(true);
            }
        }
        else
        {
            _currentLvl--;

            if (_currentLvl == 0)
            {
                _LeftArrow.SetActive(false);
            }
            else
            {
                _RightArrow.SetActive(true);
            }
        }

        UpdateUI();
    }

    private void CreateLvls()
    {
        _gameData = new GameData();
        _gameData.levels = new List<LevelData>();

        for (int x = 0; x < _lvlNames.Length; x++)
        {
            LevelData lvl = new LevelData();
            lvl.lvlName = _lvlNames[x];
            lvl.lvlNum = x;
            lvl.unlocked = (x == 0);
            lvl.bestTime = -1.0f;

            _gameData.levels.Add(lvl);
        }

        SaveSystem.Save(_gameData);
    }

    private void UpdateUI()
    {
        _lvlNameTxt.text = _gameData.levels[_currentLvl].lvlName;
        _lvlNumText.text = "Lvl " + (_gameData.levels[_currentLvl].lvlNum + 1).ToString();
        _mainBackground.sprite = _backgrounds[_currentLvl];

        if (_gameData.levels[_currentLvl].bestTime > 0)
        {
            _bestTimeText.text = _gameData.levels[_currentLvl].bestTime.ToString("0.00") + "s";
        }
        else
        {
            _bestTimeText.text = "00.00s";
        }

        if (_currentLvl == 9)
        {
            _RightArrow.SetActive(false);
            _LeftArrow.SetActive(true);
        }

        if (!_gameData.levels[_currentLvl].unlocked)
            _lockElements.SetActive(true);
        else
            _lockElements.SetActive(false);

        _levelStarsUI.StarsSetUp(_gameData.levels[_currentLvl].bestTime);
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    public int CurrentLvl
    {
        get { return _currentLvl; }
        set { _currentLvl = value; }
    }
}
