using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stalen.DI;
using Core.UI;

namespace Core
{
    public class GameLoop : MonoBehaviour
    {
        [SerializeField] private UpgradesPanel upgradesPanel;

        private Inject.Lazy<PlayerLevel> playerLevel;
        private Inject.Lazy<Crosshair> crosshair;


        private void Awake()
        {
            playerLevel.Value.OnLevelUp.AddListener(PlayerLevelChanged);
            upgradesPanel.OnClosed.AddListener(UpgradePanelClosed);
        }


        private void PlayerLevelChanged()
        {
            Time.timeScale = 0f;
            crosshair.Value.enabled = false;
            upgradesPanel.Show();
        }

        private void UpgradePanelClosed()
        {
            crosshair.Value.enabled = true;
            Time.timeScale = 1f;
        }
    }
}