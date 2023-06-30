using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class PlayerLevelBar : MonoBehaviour
    {
        [SerializeField] private PlayerLevel playerLevel;

        [Space]
        [SerializeField] private Image fill;
        [SerializeField] private RectTransform cursor;
        [SerializeField] private float fillSpeed = 5f;

        private float currentFill = 0f;


        private void Awake()
        {
            playerLevel.OnExperienceChanged.AddListener(ExperienceChanged);
            ExperienceChanged(playerLevel.Experience);
            currentFill = playerLevel.Experience / playerLevel.ExperienceToLevelUp;
            SetFill(currentFill);
        }

        private void OnDestroy()
        {
            playerLevel.OnExperienceChanged.RemoveListener(ExperienceChanged);
        }

        private void Update()
        {
            var target = playerLevel.Experience / playerLevel.ExperienceToLevelUp;
            currentFill = Mathf.Lerp(currentFill, target, fillSpeed * Time.deltaTime);
            SetFill(currentFill);
        }


        private void ExperienceChanged(float value)
        {
            //SetFill(playerLevel.Experience / playerLevel.ExperienceToLevelUp);
        }

        private void SetFill(float value)
        {
            fill.fillAmount = value;
            SetCursorPosition(value);
        }

        private void SetCursorPosition(float value)
        {
            SetNormalizedPosition(cursor, new Vector2(value, 0.5f));
        }

        private static void SetNormalizedPosition(RectTransform transform, Vector2 position)
        {
            var parent = transform.parent as RectTransform;
            var parentRect = parent.rect;
            transform.localPosition = parentRect.size * (position - parent.pivot);
        }
    }
}