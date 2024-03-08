using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CardGame.GameData;
using MagnasStudio.Util;
using UnityEngine;
using Newtonsoft.Json;

namespace CardGame.GameData
{
    public class LevelDataManager
    {
        private static LevelDataManager _Instance;
        public static LevelDataManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new LevelDataManager();
                    _Instance.LoadData();
                }
                return _Instance;
            }
        }
        private AllLevelData data;
        private const string JsonFilePath = "CardGame/Data";
        private const string FileName = "LevelData.json";

        public AllLevelData GetLevelData() => data;

        private string GetPath()
        {
#if UNITY_EDITOR
            return  Application.dataPath + "/" + JsonFilePath;
#else
            
            return   Application.persistentDataPath;
#endif
            
        }

        public void Save()
        {
            Debug.Log("<color=green>TAG::LevelDataManager========================Saving Data ====================================</color>");
#if UNITY_EDITOR
            var path = GetPath();
            var directoryPath = path.Replace("/", "\\");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            var filepath = path + "/" + FileName;
            filepath.WriteFile(JsonConvert.SerializeObject(data,Formatting.Indented));
#else
            var path= GetPath() + "/" + FileName;
            path.WriteFile(JsonConvert.SerializeObject(data,Formatting.Indented));
#endif
        }

        private void LoadData()
        {
            data = new AllLevelData(); 
            var filepath = GetPath()+ "/" + FileName;
            var json = string.Empty;
            if (filepath.CheckAndReadFile(out json))
            {
                try
                {
                    data = JsonConvert.DeserializeObject<AllLevelData>(json);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"TAG::LevelDataManager fail to parse Json {ex}");
                }
            }
        }
    }
}
