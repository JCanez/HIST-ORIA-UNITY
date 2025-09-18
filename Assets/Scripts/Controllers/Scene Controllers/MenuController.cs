using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    private GameData gameData;

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

    string[] lvlNames = { "Outside", "Entrance 01", "Office 01", "Entrance 02", "Desk", "Cashier", "Safe", "Office 02", "Jewellery", "Exit" };

    private void Start()
    {
        _currentLvl = 0;

        gameData = SaveSystem.Load();

        if (gameData.levels.Count == 0)
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
        gameData = new GameData();
        gameData.levels = new List<LevelData>();

        for (int x = 0; x < lvlNames.Length; x++)
        {
            LevelData lvl = new LevelData();
            lvl.lvlName = lvlNames[x];
            lvl.lvlNum = x;
            lvl.unlocked = (x == 0);
            lvl.bestTime = 15f;

            gameData.levels.Add(lvl);
        }

        SaveSystem.Save(gameData);
    }

    private void UpdateUI()
    {
        _lvlNameTxt.text = gameData.levels[_currentLvl].lvlName;
        _lvlNumText.text = "Lvl " + (gameData.levels[_currentLvl].lvlNum + 1).ToString();
        _mainBackground.sprite = _backgrounds[_currentLvl];

        if (!gameData.levels[_currentLvl].unlocked)
            _lockElements.SetActive(true);
        else
            _lockElements.SetActive(false);
    }

    public int CurrentLvl
    {
        get { return _currentLvl; }
        set { _currentLvl = value; }
    }
}
