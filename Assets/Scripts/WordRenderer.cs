using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace DefaultNamespace
{
    public class WordRenderer : MonoBehaviour, IComparable<WordRenderer>
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text text;
        [SerializeField] private AspectRatioFitter aspectRatioFitter;
        [SerializeField] private Animator textShrinkAnimator;
        [SerializeField] private Animator disappearAnimator;

        public const float Width = 5.55f;
        public const float Height = 5.55f;
        
        private Word _word;
        private string _langWord;
        private int _typedCount;
        private bool _active;
        private float _startY;
        private string _color;
        private WordManager _wordManager;
        
        

        public bool IsActive()
        {
            return _active;
        }

        public void SetWord(Word word, WordManager wordManager)
        {
            _wordManager = wordManager;
            var colors = new List<string>() {"red", "green", "blue", "yellow", "orange", "purple"};
            _color = colors[UnityEngine.Random.Range(0, colors.Count)];
            text.gameObject.SetActive(false);
            _startY = transform.position.y;
            _word = word;
            image.sprite = word.Sprite;
            _langWord = LanguageController.CurrentLanguage == Language.English ? word.EnglishWord : word.RussianWord;
            text.text = _langWord.ToLower();
            aspectRatioFitter.aspectRatio = (float)word.Sprite.texture.width / word.Sprite.texture.height;
        }

        public bool IsFinished()
        {
            return _typedCount == _langWord.Length;
        }

        public char GetCurrentLetter()
        {
            return _langWord[_typedCount];
        }

        public void TypeOne()
        {
            _typedCount++;
            UpdateText();
        }

        private void Update()
        {
            if (!_active && transform.position.y < _startY - Mathf.Clamp01(1f / aspectRatioFitter.aspectRatio) * Width)
            {
                text.gameObject.SetActive(true);
                _active = true;
            }

            if (transform.position.y < WordManager.MinY)
            {
                _wordManager.Loose();
                DestroyImmediate(gameObject);
            }
        }

        private void UpdateText()
        {
            text.text = $"<color={_color}>{_langWord[.._typedCount]}</color>{_langWord[_typedCount..]}";
            textShrinkAnimator.SetTrigger("Shrink");
        }

        public void StartDestroy()
        {
            disappearAnimator.SetTrigger("Disappear");
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public int CompareTo(WordRenderer other)
        {
            if (!Mathf.Approximately(transform.position.y, other.transform.position.y))
            {
                return transform.position.y.CompareTo(other.transform.position.y);
            }
            return transform.position.x.CompareTo(other.transform.position.x);
        }
    }
}