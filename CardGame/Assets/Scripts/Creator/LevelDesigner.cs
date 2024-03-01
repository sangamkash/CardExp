using System.Collections;
using System.Collections.Generic;
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
        
        [SerializeField] private Text layoutName;

        [Header("Grid")]
        [SerializeField] private GridLayoutGroup gridLayout;

        [Header("Other Data")]
        [SerializeField] private CardData cardData;
        [SerializeField] private GridLayoutData gridLayoutData;

    }
}
