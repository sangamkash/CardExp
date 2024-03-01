using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.GameData
{
    [System.Serializable]
    public class LevelData
    {
        public string levelName;
        public Vector2 gridType;
        public int[][] cardIds;
    }
    
    [System.Serializable]
    public class AllLevelData
    {
        public List<LevelData> levelDatas;
    }
}
