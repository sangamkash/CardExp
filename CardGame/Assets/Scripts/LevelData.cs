using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardGame.GameData
{
    [System.Serializable]
    public class LevelData
    {
        public string levelName;
        public Vector2Int gridDimension;
        public int[][] cardIds;
    }
    
    [System.Serializable]
    public class AllLevelData
    {
        public List<LevelData> levelDatas;

        public AllLevelData()
        {
            levelDatas = new List<LevelData>();
        }
    }
}
