using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.CreatorSystem.Data;
using CardGame.GameData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CardGame.CreatorSystem
{
    public class LevelDesigner : MonoBehaviour
    {
        [SerializeField] private Button backBtn;

        [Header("LoadOrCreate Ref")] [SerializeField]
        private TMP_Dropdown levelDropdown;

        [SerializeField] private Button loadBtn;
        [SerializeField] private Button CreateBtn;

        [Header("Level name")] 
        [SerializeField] private TMP_InputField levelNameIpf;
        [SerializeField] private TMP_InputField timeIpf;

        [SerializeField] private TMP_Dropdown gridDropdown;
        [SerializeField] private Button saveBtn;
        [SerializeField] private Button DeleteBtn;

        [Header("Other Data")] [SerializeField]
        private CardData cardData;

        [SerializeField] private GridLayoutData gridLayoutData;
        [SerializeField] private CardLayoutHandler cardLayoutHandler;
        [SerializeField] private CardSelector cardSelector;

        private Vector2Int currentGridDimension = new Vector2Int(2, 2);
        private LevelData currentLevelData;
        private AllLevelData levelData => LevelDataManager.Instance.GetLevelData();
        private const string defaultLevelName = "Test";
        private Vector2Int currentGridIndex;
        private int defaultTime=10;

        public void Awake()
        {
            cardSelector.Init(OnCardSelect);
            backBtn.onClick.AddListener(OnBackBtnClick);
            CreateBtn.onClick.AddListener(Create);
            loadBtn.onClick.AddListener(Load);
            levelDropdown.onValueChanged.AddListener(OnLevelSelected);
            gridDropdown.onValueChanged.AddListener(OnGridSelected);
            saveBtn.onClick.AddListener(Save);
            DeleteBtn.onClick.AddListener(DeleteData);
            currentGridDimension = gridLayoutData.GetAllGridLayouts()[0];
            timeIpf.text = defaultTime.ToString();
            levelNameIpf.text = defaultLevelName;
            levelNameIpf.onValueChanged.AddListener(OnTextEnter);
            timeIpf.onValueChanged.AddListener(OnTimeEnter);
            var gridOptions = gridLayoutData.GetAllGridLayouts()
                .Select(gridLayoutData => $"{gridLayoutData.x}x{gridLayoutData.y}").ToList();
            gridDropdown.ClearOptions();
            gridDropdown.AddOptions(gridOptions);
            PopulateUI();
        }

        private void OnTimeEnter(string time)
        {
            var timeValue = defaultTime;
            if (currentLevelData != null)
            {
                if (!int.TryParse(time, out timeValue))
                {
                    Debug.Log($"<color=red>Enter valid Time </color>");
                }
                currentLevelData.time = timeValue;
            }
        }

        private void DeleteData()
        {
            if(currentLevelData != null)
            {
                levelData.levelDatas.Remove(currentLevelData);
                currentLevelData = null;
            }
            PopulateUI();
        }

        private void OnCardSelect(int cardId)
        {
            cardLayoutHandler.SetImageAtIndex(currentGridIndex, cardData.GetCardById(cardId));
            currentLevelData.cardIds[currentGridIndex.x][currentGridIndex.y] = cardId;
        }

        private void OnTextEnter(string str)
        {
            if (currentLevelData != null)
            {
                currentLevelData.levelName = str;
            }
        }

        private void OnBackBtnClick()
        {
            SceneManager.LoadScene(GameConstants.Scene_StartMenu);
        }

        private void OnLevelSelected(int index)
        {
            currentLevelData = levelData.levelDatas[index];
            Load();
        }

        private void OnGridSelected(int index)
        {
            var masterGridData = gridLayoutData.GetAllGridLayouts();
            currentGridDimension = masterGridData[index];
            if (currentLevelData != null)
            {
                var cardIds = new int[currentGridDimension.x][];
                for (int i = 0; i < currentGridDimension.x; i++)
                {
                    cardIds[i] = new int[currentGridDimension.y];
                    for (int j = 0; j < currentGridDimension.y; j++)
                    {
                        if (i < currentLevelData.gridDimension.x && j < currentLevelData.gridDimension.y)
                            cardIds[i][j] = currentLevelData.cardIds[i][j];
                        else
                            cardIds[i][j] = -1;
                    }
                }

                currentLevelData.cardIds = cardIds;
                currentLevelData.gridDimension = currentGridDimension;
                Load();
            }
        }

        private void Create()
        {
            currentGridDimension = gridLayoutData.GetAllGridLayouts()[0];
            var cardIds = new int[currentGridDimension.x][];
            for (int i = 0; i < currentGridDimension.x; i++)
            {
                cardIds[i] = new int[currentGridDimension.y];
                for (int j = 0; j < currentGridDimension.y; j++)
                {
                    cardIds[i][j] = -1;
                }
            }

            var newLevelData = new LevelData()
            {
                levelName = defaultLevelName,
                gridDimension = currentGridDimension,
                cardIds = cardIds
            };
            levelData.levelDatas.Insert(0, newLevelData);
            currentLevelData = newLevelData;
            PopulateUI();
        }

        private bool isDataValid(LevelData data)
        {
            if (data == null)
                return false;
            var gridLayout = gridLayoutData.GetAllGridLayouts();
            var gridMatched = false;
            for (int i = 0; i <gridLayout.Length; i++)
            {
                if (data.gridDimension == gridLayout[i])
                    gridMatched = true;
            }
            if (!gridMatched)
            {
                Debug.Log($"<color=red>Improper Grid dimension</color>");
                return false;
            }
            
            var dic = new Dictionary<int, int>();//id and count;
            var emptyCount = 0;
            for (var i = 0; i < data.cardIds.Length; i++)
            {
                var cards = data.cardIds[i];
                for (var j = 0; j < cards.Length; j++)
                {
                    var card = cards[j];
                    if (card == -1)
                    {
                       // Debug.Log($"<color=red>Image is not assigned to Grid {i}x{j}</color>");
                       emptyCount++;
                    }
                    else
                    {
                        if (dic.ContainsKey(card))
                        {
                            dic[card] += 1;
                        }
                        else
                        {
                            dic.Add(card, 1);
                        }
                    }
                }
            }

            if (emptyCount == data.gridDimension.x * data.gridDimension.y)
            {
                Debug.Log($"<color=red>all cell should not be empty</color>");
                return false;
            }

            foreach (var cardCount in dic)
            {
                if (cardCount.Value % 2 != 0)
                {
                    Debug.Log($"<color=red>Make sure the image should be assign in pair </color>");
                    return false;
                }
            }

            if (data.time <= 0)
            {
                Debug.Log($"<color=red>Make sure the timer should be greater 0</color>");
                return false;
            }
            return true;
        }

        private void PopulateUI()
        {
            if (levelData.levelDatas.Count > 0 && currentLevelData == null)
            {
                currentLevelData = levelData.levelDatas[0];
            }
            else if (currentLevelData == null)
            {
                var cardIds = new int[currentGridDimension.x][];
                for (int i = 0; i < currentGridDimension.x; i++)
                {
                    cardIds[i] = new int[currentGridDimension.y];
                    for (int j = 0; j < currentGridDimension.y; j++)
                    {
                        cardIds[i][j] = -1;
                    }
                }

                currentLevelData = new LevelData()
                {
                    levelName = defaultLevelName,
                    gridDimension = currentGridDimension,
                    cardIds = cardIds
                };
                levelData.levelDatas.Add(currentLevelData);
            }

            var levelNames = levelData.levelDatas.Select(level => level.levelName).ToList();
            levelDropdown.ClearOptions();
            levelDropdown.AddOptions(levelNames);
            Load();
        }

        private void Load()
        {
            if (currentLevelData != null)
            {
                levelNameIpf.text = currentLevelData.levelName;
                timeIpf.text = currentLevelData.time.ToString();
                var k = 0;
                foreach (var option in gridDropdown.options)
                {
                    if (option.text == $"{currentLevelData.gridDimension.x}x{currentLevelData.gridDimension.y}")
                    {
                        gridDropdown.value = k;
                        break;
                    }

                    ++k;
                }

                cardLayoutHandler.CreateLayout(currentLevelData.gridDimension,OnCardSelectInGrid);
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

        private void OnCardSelectInGrid(Vector2Int index)
        {
            currentGridIndex = index;
        }

        private void Save()
        {
            foreach (var data in levelData.levelDatas)
            {
                Debug.Log($"<color=yellow>==== Starting analysing : ''{data.levelName}''====</color>");
                if (!isDataValid(data))
                {
                    Debug.Log($"<color=red>Error found in : '' {data.levelName} '' </color>");
                    return;
                }
            }
            LevelDataManager.Instance.Save();
            PopulateUI();
        }
    }
}
