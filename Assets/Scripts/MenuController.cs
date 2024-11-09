using System;
using System.Collections.Generic;
using DefaultNamespace.UI;
using UnityEngine;
using YG;

namespace DefaultNamespace
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private WordManager wordManager;

        [SerializeField] private List<Level> levels;

        [SerializeField] private Transform buttonsParent;
        
        [SerializeField] private GameObject levelButtonPrefab;
        
        [SerializeField] private WindowSwitcher windowSwitcher;

        private readonly List<LevelButton> _buttons = new ();
        
        private static int LoadLevel()
        {
            return YandexGame.savesData.PassedLevelCount;
        }

        private static void SaveLevel(int passedCount)
        {
            //PlayerPrefs.SetInt("PassedCount", passedCount);
            YandexGame.savesData.PassedLevelCount = passedCount;
            YandexGame.SaveProgress();
        }

        private void Load()
        {
            var passedCount = LoadLevel();

            for (var i = 0; i < levels.Count; i++)
            {
                var index = i;
                var level = levels[i];
                var lb = Instantiate(levelButtonPrefab, buttonsParent).GetComponent<LevelButton>();
                lb.SetUp(i, level, i > passedCount, Open, i == passedCount);
                lb.UpdateLanguage();
                _buttons.Add(lb);
                continue;

                void Open()
                {
                    OpenLevel(index);
                }
            }
        }

        private void Awake()
        {

            YandexGame.GetDataEvent += Load;
            
        }

        public void UpdateLanguage()
        {
            foreach (var button in _buttons)
            {
                button.UpdateLanguage();
            }
        }

        public void UpdatePassedCount(int passedCount)
        {
            if (passedCount < LoadLevel())
            {
                return;
            }
            YandexGame.NewLeaderboardScores("Passed Level Count", passedCount);
            SaveLevel(passedCount);
            for (var i = 0; i < levels.Count; i++)
            {
                _buttons[i].UpdateLocked(i > passedCount, i == passedCount);
            }
        }

        public void NextLevel(int levelIndex)
        {
            OpenLevel(levelIndex);
        }

        private void OpenLevel(int levelIndex)
        {
            if (Energyntroller.HasEnergy())
            {
                Energyntroller.SpendEnergy();
                windowSwitcher.SwitchWindow(WindowName.Game);
                wordManager.GenerateLevel(levels[levelIndex], this, levelIndex);
            }
            else
            {
                wordManager.Stop();
                windowSwitcher.SwitchWindow(WindowName.MainMenu);
            }
        }
    }
}