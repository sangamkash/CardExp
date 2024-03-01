using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.CreatorSystem.Data;
using CardGame.GameData;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.CreatorSystem
{
    public class LevelDesigner : MonoBehaviour
    {
        [Header("LoadOrCreate Ref")] 
        [SerializeField] private Dropdown levelDropdown;
        [SerializeField] private Button LoadBtn;
        [SerializeField] private Button CreateBtn;

        [Header("Level name")]
        [SerializeField] private InputField levelNameIpf;
        [SerializeField] private Dropdown gridDropdown;

        [Header("Grid")]
        [SerializeField] private GridLayoutGroup gridLayout;

        [Header("Other Data")]
        [SerializeField] private CardData cardData;
        [SerializeField] private GridLayoutData gridLayoutData;
        private Vector2 currentGrid = new Vector2(2, 2);
        private LevelData currentLevelData;
        private AllLevelData levelData => LevelDataManager.Instance.GetLevelData();

        public void Awake()
        {
            CreateBtn.onClick.AddListener(Create);
            LoadBtn.onClick.AddListener(Load);
            levelDropdown.onValueChanged.AddListener(OnLevelSelected);
            gridDropdown.onValueChanged.AddListener(OnGridSelected);
        }

        private void OnLevelSelected(int index)
        {
            currentLevelData = levelData.levelDatas[index];
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
                var newLevelData = new LevelData()
                {
                     levelName=levelNameIpf.text,
                     gridType= currentGrid,
                };
                levelData.levelDatas.Add(newLevelData);
                currentLevelData = newLevelData;
            }
        }
        private bool isDataValid()
        {
            //TODO Do add validation
            return true;
        }
        private void Populate()
        {
            var levelNames = levelData.levelDatas.Select(level => level.levelName).ToList();
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
        }

        private void Save()
        {
            LevelDataManager.Instance.Save();
            Populate();
        }
    }
}
