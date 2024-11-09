using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using YG;

namespace DefaultNamespace
{
    public class WordManager : MonoBehaviour
    {
        [SerializeField] private Transform spawnParent;
        [SerializeField] private GameObject wordPrefab;

        [SerializeField] private GameObject winScreen;
        [SerializeField] private GameObject looseScreen;

        [SerializeField] private AudioSource letterSource;
        [SerializeField] private AudioSource failureSource;
        [SerializeField] private AudioSource successSource;

        [SerializeField] private List<Color> colors;
        [SerializeField] private Camera cam;

        private static float _minX, _maxX, _y;

        private bool _game;

        private Level _level;
        private MenuController _menuController;
        private int _levelIndex;
        private int _finishedCount;

        private bool _first;
        
        private List<WordRenderer> _wordRenderers;

        private void Awake()
        {
            CountLimits();
        }

        private static void CountLimits()
        {
            var mainCamera = Camera.main;
            if (!mainCamera)
            {
                return;
            }
            Vector2 point = -mainCamera.ScreenToWorldPoint(Vector3.zero);
            _y = point.y;
            _maxX = point.x - WordRenderer.Width / 2;
            _minX = -_maxX;
        }
        
        public static float MinY => -_y;

        private static Vector2 GenerateSpawnPoint()
        {
            return new Vector2(UnityEngine.Random.Range(_minX, _maxX), _y);
        }

        private WordRenderer SpawnWord(Word word)
        {
            var position = GenerateSpawnPoint();
            var go = Instantiate(wordPrefab, position, Quaternion.identity, spawnParent);
            var wordRenderer = go.GetComponent<WordRenderer>();
            wordRenderer.SetWord(word, this);
            return wordRenderer;
        }

        private IEnumerator GenerateWords(List<Word> words)
        {
            while (spawnParent.childCount > 0)
            {
                DestroyImmediate(spawnParent.GetChild(0).gameObject);
            }
            
            _wordRenderers = new List<WordRenderer>();
            words.Sort();
            var d = 5f / (1f + _levelIndex / 10f);
            foreach (var word in words)
            {
                var wordRenderer = SpawnWord(word);
                _wordRenderers.Add(wordRenderer);
                yield return new WaitForSeconds(d);
            }
        }

        public void Retry()
        {
            GenerateLevel(_level, _menuController, _levelIndex);
        }

        public void Win()
        {
            _game = false;
            winScreen.SetActive(true);
            _menuController.UpdatePassedCount(_levelIndex + 1);
        }

        public void Loose()
        {
            failureSource.Play();
            _game = false;
            looseScreen.SetActive(true);
        }

        public void GenerateLevel(Level level, MenuController menuController, int levelIndex)
        {
            cam.backgroundColor = colors[UnityEngine.Random.Range(0, colors.Count)];
            _finishedCount = 0;
            winScreen.SetActive(false);
            looseScreen.SetActive(false);
            _game = true;
            _level = level;
            _menuController = menuController;
            _levelIndex = levelIndex;
            StartCoroutine(GenerateWords(level.Words));
            if (_first)
            {
                YandexGame.FullscreenShow();
            }
            _first = true;
        }

        public void Stop()
        {
            _game = false;
        }

        public void NextLevel()
        {
            _menuController.NextLevel(_levelIndex + 1);
        }

        private void Update()
        {
            if (!_game)
            {
                return;
            }
            if (Input.inputString.Length > 0)
            {
                _wordRenderers.Sort();
                foreach (var wordRenderer in _wordRenderers)
                {
                    if (!wordRenderer.IsActive())
                    {
                        continue;
                    }
                    var letter = wordRenderer.GetCurrentLetter();
                    var processedChar = Input.inputString.ToLower()[0];
                    if (letter == processedChar)
                    {
                        wordRenderer.TypeOne();
                        letterSource.Play();
                        if (wordRenderer.IsFinished())
                        {
                            _wordRenderers.Remove(wordRenderer);
                            wordRenderer.StartDestroy();
                            _finishedCount++;
                            successSource.Play();
                            if (_finishedCount == _level.Words.Count)
                            {
                                Win();
                            }
                        }
                        break;
                    }
                    failureSource.Play();
                }
            }
        }
    }
}