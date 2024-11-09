using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YG;

namespace DefaultNamespace
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text levelNameText;
        [SerializeField] private Image levelImage;
        [SerializeField] private AspectRatioFitter aspectRatioFitter;
        [SerializeField] private Button button;
        [SerializeField] private GameObject lockedGraphics;
        [SerializeField] private GameObject unlockedGraphics;
        [SerializeField] private Outline outline;

        private readonly UnityEvent _onClick = new ();

        private Level _level;
        private bool _locked;

        public void SetUp(int levelIndex, Level level, bool locked, UnityAction onClick, bool last)
        {
            _level = level;
            levelNameText.text = level.Name;
            _onClick.AddListener(onClick);
            levelText.text = (levelIndex + 1).ToString();
            var sprite = level.Words[0].Sprite;
            levelImage.sprite = sprite;
            aspectRatioFitter.aspectRatio = (float)sprite.texture.width / sprite.texture.height;
            UpdateLocked(locked, last);
        }

        public void UpdateLanguage()
        {
            levelNameText.text = LanguageController.CurrentLanguage == Language.English ? _level.Name : _level.RussianName;
        }

        public void UpdateLocked(bool locked, bool last)
        {
            outline.enabled = last;
            button.interactable = !locked;
            _locked = locked;
            lockedGraphics.SetActive(locked);
            unlockedGraphics.SetActive(!locked);
        }
        
        public void OnClick()
        {
            if (!_locked)
            {
                _onClick?.Invoke();
            }
            else
            {
                _onClick?.Invoke();
                //YandexGame.RewVideoShow();
            }
        }
    }
}