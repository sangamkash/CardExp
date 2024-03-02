using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.GameData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CardGame.GamePlay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Button backBtn;
        [SerializeField] private TextMeshProUGUI levelName;
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private Button retryBtn;
        [SerializeField] private GameCardGridLayoutHandler gridLayout;
        private AllLevelData allLevelData => LevelDataManager.Instance.GetLevelData();
        private void Start()
        {
            var currentLevelIndex = PlayerPrefs.GetInt(GameConstants.PlayerPrefs_CurrentLevel, -1);
            var levelData = allLevelData.levelDatas[currentLevelIndex];
            gridLayout.CreateLayout(levelData,OnCardMatch);
            backBtn.onClick.AddListener(OnBackButtonClicked);
            retryBtn.onClick.AddListener(RetryButtonClicked);
        }

        private void RetryButtonClicked()
        {
            SceneManager.LoadScene(GameConstants.Scene_GamePlay);
        }

        private void OnBackButtonClicked()
        {
            SceneManager.LoadScene(GameConstants.Scene_LevelSelector);
        }

        private void OnCardMatch()
        {
            
        }
    }
}
