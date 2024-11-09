using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace DefaultNamespace
{
    public class Energyntroller : MonoBehaviour
    {
        [SerializeField] private Slider energySlider;
        [SerializeField] private TMP_Text timeText;
        
        private const int MaxEnergy = 5;

        private const float Duration = 300;

        private float _time = Duration;

        private int _energy = MaxEnergy;

        private static Energyntroller _instance;

        private void Awake()
        {
            _instance = this;
            UpdateSlider();
        }

        public static bool HasEnergy()
        {
            return _instance._energy > 0;
        }

        public static void SpendEnergy()
        {
            _instance._energy--;
            _instance.UpdateSlider();
        }

        public void TryAdd()
        {
            if (_energy == MaxEnergy)
            {
                return;
            }
            YandexGame.RewardVideoEvent += AddEnergy;
            YandexGame.RewVideoShow(0);
        }

        private void AddEnergy(int id)
        {
            if (id == 0)
            {
                if (_energy == MaxEnergy)
                {
                    return;
                }
                _energy++;
                UpdateSlider();
            }
        }

        private void UpdateSlider()
        {
            energySlider.value = (float)_energy / MaxEnergy;
        }

        private void Update()
        {
            _time -= Time.deltaTime;
            timeText.text = ((int)Mathf.Clamp(_time, 0, Duration)).ToString();
            if (_time > 0)
            {
                return;
            }
            if (_energy < MaxEnergy)
            {
                AddEnergy(0);
            }
            _time = Duration;
        }
    }
}