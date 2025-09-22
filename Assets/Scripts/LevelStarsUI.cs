using UnityEngine;
using UnityEngine.UI;

public class LevelStarsUI : MonoBehaviour
{
    [Header("Stars")]
    [SerializeField]
    private Image[] _starImages;
    [SerializeField]
    private Sprite _starOffSprite;
    [SerializeField]
    private Sprite _starOnSprite;

    GameData _gameData;

    public void Start()
    {
        _gameData = SaveSystem.Load();
    }

    public void StarsSetUp(float lvlIndex)
    {
        float lvlTime = lvlIndex;

        for (int x = 0; x < _starImages.Length; x++)
            _starImages[x].sprite = _starOffSprite;

        // Encender estrellas según tiempo
        if (lvlTime >= 10f)
        {
            _starImages[0].sprite = _starOnSprite;
        }
        else if (lvlTime >= 5f && lvlTime < 10)
        {
            _starImages[0].sprite = _starOnSprite;
            _starImages[1].sprite = _starOnSprite;
        }
        else if (lvlTime > 0f && lvlTime < 5f)
        {
            for (int i = 0; i < _starImages.Length; i++)
                _starImages[i].sprite = _starOnSprite;
        }

    }
}
