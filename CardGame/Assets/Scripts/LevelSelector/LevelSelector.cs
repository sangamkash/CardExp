using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.GameData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CardGame.LevelRender
{

    public class LevelSelector : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private Button prefab;
        [SerializeField] private GameObject warning;
        [SerializeField] private GameObject scrollView;
        private AllLevelData levelData => LevelDataManager.Instance.GetLevelData();
        private void Start()
        {
            var isHasLevel = levelData.levelDatas.Count > 0;
            warning.SetActive(!isHasLevel);
            scrollView.SetActive(isHasLevel);
            for (var i = 0; i < levelData.levelDatas.Count; i++)
            {
                var index = i;
                var level = levelData.levelDatas[i];
                var t = Instantiate(prefab, container).GetComponent<Button>();
                t.GetComponentInChildren<TextMeshProUGUI>().text = level.levelName;
                t.onClick.AddListener(() => OnLevelSelected(index));
            }
        }

        private void OnLevelSelected(int levelIndex)
        {
            PlayerPrefs.SetInt(GameConstants.PlayerPrefs_CurrentLevel,levelIndex);
            PlayerPrefs.Save();
            SceneManager.LoadScene(GameConstants.Scene_GamePlay);
        }
    }
}