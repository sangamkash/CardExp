using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.CreatorSystem.Data;
using CardGame.GameData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CardGame.CreatorSystem
{
    public class LevelDesigner : MonoBehaviour
    {
        [SerializeField] private Button backBtn;
        [Header("LoadOrCreate Ref")] 
        [SerializeField] private TMP_Dropdown levelDropdown;
        [SerializeField] private Button loadBtn;
        [SerializeField] private Button CreateBtn;

        [Header("Level name")]
        [SerializeField] private TMP_InputField levelNameIpf;
        [SerializeField] private TMP_Dropdown gridDropdown;

        [Header("Other Data")]
        [SerializeField] private CardData cardData;
        [SerializeField] private GridLayoutData gridLayoutData;
        [SerializeField] private CardLayoutHandler cardLayoutHandler;
        
        private Vector2Int currentGrid = new Vector2Int(2, 2);
        private LevelData currentLevelData;
        private AllLevelData levelData => LevelDataManager.Instance.GetLevelData();

        public void Awake()
        {
            backBtn.onClick.AddListener(OnBackBtnClick);
            CreateBtn.onClick.AddListener(Create);
            loadBtn.onClick.AddListener(Load);
            levelDropdown.onValueChanged.AddListener(OnLevelSelected);
            gridDropdown.onValueChanged.AddListener(OnGridSelected);
            currentGrid = gridLayoutData.GetAllGridLayouts()[0];
            PopulateUI();
        }

        private void OnBackBtnClick()
        {
            SceneManager.LoadScene(GameConstants.Scene_StartMenu);
        }

        private void OnLevelSelected(int index)
        {
            currentLevelData = levelData.levelDatas[index-1];
        }

        private void OnGridSelected(int index)
        {
            var masterGridData = gridLayoutData.GetAllGridLayouts();
            currentGrid = masterGridData[index];
        }
        
        private void Create()
        {
            if (isDataValid())
            {
                var cardIds = new int[currentGrid.x][];
                for (int i = 0; i < currentGrid.x; i++)
                {
                    cardIds[i] = new int[currentGrid.y];
                    for (int j = 0; j < currentGrid.y; j++)
                    {
                        cardIds[i][j] = -1;
                    }
                }
                var newLevelData = new LevelData()
                {
                     levelName=levelNameIpf.text,
                     gridDimension= currentGrid,
                     cardIds = cardIds
                };
                levelData.levelDatas.Insert(0, newLevelData);
                currentLevelData = newLevelData;
                Save();
                Load();
            }
        }
        private bool isDataValid()
        {
            //TODO Do add validation
            return true;
        }
        private void PopulateUI(int index=0)
        {
            var levelNames = new List<string>() { "None" };
            levelNames.AddRange(levelData.levelDatas.Select(level => level.levelName).ToList());
            levelDropdown.ClearOptions();
            levelDropdown.AddOptions(levelNames);
            var gridOptions = gridLayoutData.GetAllGridLayouts()
                .Select(gridLayoutData => $"{gridLayoutData.x}x{gridLayoutData.y}").ToList();
            gridDropdown.ClearOptions();
            gridDropdown.AddOptions(gridOptions);
        }

        private void Load()
        {
            //TODO 
            if (currentLevelData != null)
            {
               cardLayoutHandler.CreateLayout(currentLevelData.gridDimension);
               for (int i = 0; i < currentLevelData.gridDimension.x; i++)
               {
                   for (int j = 0; j < currentLevelData.gridDimension.y; j++)
                   {
                       var cardId = currentLevelData.cardIds[i][j];
                       cardLayoutHandler.SetImageAtIndex(new Vector2Int(i, j), cardData.GetCardById(cardId));
                   }
               }
               
            }
        }

        private void Save()
        {
            LevelDataManager.Instance.Save();
            PopulateUI();
        }
    }
}
