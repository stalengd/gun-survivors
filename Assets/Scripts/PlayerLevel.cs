using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Stalen.DI;
using Core.Data;

namespace Core
{
    public sealed class PlayerLevel : MonoService
    {
        [SerializeField] private GameModeSheet.Reference gameMode;
        [SerializeField] private UnityEvent onLevelUp;
        [SerializeField] private UnityEvent<float> onExperienceChanged;

        public float Experience => currentExperience;
        public float ExperienceToLevelUp => experienceToLevelUp;
        public int Level => currentLevel;
        public UnityEvent OnLevelUp => onLevelUp;
        public UnityEvent<float> OnExperienceChanged => onExperienceChanged;

        private float experienceToLevelUp = 10f;
        private float currentExperience = 0f;
        private int currentLevel = 0;


        private void Awake()
        {
            RefreshExperienceToLevelUp();
        }

        public void GainExperience(float value)
        {
            currentExperience += value;
            while (currentExperience >= experienceToLevelUp)
            {
                currentExperience -= experienceToLevelUp;
                currentLevel++;
                RefreshExperienceToLevelUp();
                onLevelUp?.Invoke();
            }
            onExperienceChanged?.Invoke(currentExperience);
        }

        private void RefreshExperienceToLevelUp()
        {
            experienceToLevelUp = gameMode.Ref.Experience.GetOrLast(Level);
        }
    }
}