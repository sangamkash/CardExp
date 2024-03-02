using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CardGame.BootSystem
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] private Button levelCreatorBtn;
        [SerializeField] private Button levelPlayBtn;

        private void Awake()
        {
            levelCreatorBtn.onClick.AddListener(LoadLevelCreator);
            levelPlayBtn.onClick.AddListener(LoadLevel);
        }

        private void LoadLevelCreator()
        {
            SceneManager.LoadScene(GameConstants.Scene_Creator);
        }

        private void LoadLevel()
        {
            SceneManager.LoadScene(GameConstants.Scene_LevelSelector);
        }
    }
}