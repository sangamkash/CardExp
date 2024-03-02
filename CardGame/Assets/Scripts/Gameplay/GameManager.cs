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
        [SerializeField] private GameObject gameOverScreen;
        [SerializeField] private TextMeshProUGUI gameOverText;
        [SerializeField] private Button gameOverBackBtn;
        [SerializeField] private Button gameOverRetryBtn;

        private const string winMsg = "You Won";
        private const string looseMsg = "You Loose";
        private int scoreCount;
        private int turnCount;
        private AllLevelData allLevelData => LevelDataManager.Instance.GetLevelData();
        private int[][] gridData;
        private void Start()
        {
            turnCount = 0;
            scoreCount = 0;
            var currentLevelIndex = PlayerPrefs.GetInt(GameConstants.PlayerPrefs_CurrentLevel, -1);
            var levelData = allLevelData.levelDatas[currentLevelIndex];
            gridData = new int[levelData.gridDimension.x][];
            for (int i = 0; i < levelData.gridDimension.x; i++)
            {
                gridData[i] = new int[levelData.gridDimension.y];
                for (int j = 0; j < levelData.gridDimension.y; j++)
                {
                    gridData[i][j] = levelData.cardIds[i][j];
                }
            }
            gridLayout.CreateLayout(levelData,OnCardMatch,gridData);
            backBtn.onClick.AddListener(OnBackButtonClicked);
            retryBtn.onClick.AddListener(RetryButtonClicked);
            gameOverBackBtn.onClick.AddListener(OnBackButtonClicked);
            gameOverRetryBtn.onClick.AddListener(RetryButtonClicked);
            levelName.text = levelData.levelName;
            score.text = $"score:{scoreCount} Turn : {turnCount}";
            StartCoroutine(StartTimer(levelData.time));
            gameOverScreen.SetActive(false);
        }

        private void RetryButtonClicked()
        {
            SceneManager.LoadScene(GameConstants.Scene_GamePlay);
        }

        private void OnBackButtonClicked()
        {
            SceneManager.LoadScene(GameConstants.Scene_LevelSelector);
        }

        private IEnumerator StartTimer(float time)
        {
            while (time>0)
            {
                timer.text = $"{time} sec";
                yield return new WaitForSeconds(1);
                time -= 1;
            }
            timer.text = $"0 sec";
            GameCompleted(false);
        }

        private void GameCompleted(bool isWon)
        {
            gameOverText.text = isWon ? winMsg : looseMsg;
            gameOverScreen.SetActive(true);
            StopAllCoroutines();
        }

        private void OnCardMatch(bool isSuccess)
        {
            turnCount++;
            if (isSuccess)
            {
                scoreCount++;
            }
            score.text = $"score:{scoreCount} Turn:{turnCount}";
            var isCompleted = true;
            foreach (var datas in gridData)
            {
                foreach (var data in datas)
                {
                    if (data > 0)
                    {
                        isCompleted = false;
                    }
                }
            }
            
            if (isCompleted)
            {
                GameCompleted(true);
            }
        }
    }
}
