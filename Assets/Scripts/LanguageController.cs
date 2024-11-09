using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public enum Language {English, Russian}
    public class LanguageController : MonoBehaviour
    {
        public static Language CurrentLanguage = Language.Russian;

        [SerializeField] private Image russianImage;
        [SerializeField] private Image englishImage;

        [SerializeField] private TMP_Text label;
        [SerializeField] private TMP_Text languageText;
        [SerializeField] private TMP_Text iMakeText;
        [SerializeField] private TMP_Text backText;
        [SerializeField] private TMP_Text energyText;
        [SerializeField] private TMP_Text adsText;

        [SerializeField] private Color greenColor;
        [SerializeField] private Color grayColor;
        
        [SerializeField] private MenuController menuController;

        private void Start()
        {
            var str = PlayerPrefs.GetString("Language");
            if (str == "en")
            {
                CurrentLanguage = Language.English;
            }
            UpdateButtons();
        }

        public void SetLanguage(string language)
        {
            SetLanguage(language == "en" ? Language.English : Language.Russian);
        }

        public void UpdateButtons()
        {
            russianImage.color = CurrentLanguage == Language.Russian ? greenColor : grayColor;
            englishImage.color = CurrentLanguage == Language.English ? greenColor : grayColor;
            label.text = CurrentLanguage == Language.Russian ? "Клавиатурное\nБезумие" : "Keyboard\nRush";
            languageText.text = CurrentLanguage == Language.Russian ? "Язык" : "Language";
            iMakeText.text = CurrentLanguage == Language.Russian ? "Сейчас я создаю новые уровни..." : "I make new levels now...";
            backText.text = CurrentLanguage == Language.Russian ? "Назад" : "Back";
            energyText.text = CurrentLanguage == Language.Russian ? "Энергия" : "Energy";
            adsText.text = CurrentLanguage == Language.Russian ? "Реклама" : "Ads";
        }

        private void SetLanguage(Language language)
        {
            CurrentLanguage = language;
            PlayerPrefs.SetString("Language", language == Language.Russian ? "ru" : "en");
            UpdateButtons();
            menuController.UpdateLanguage();
        }
    }
}