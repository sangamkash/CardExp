using UnityEngine;

namespace CardGame.CreatorSystem.Data
{
    [CreateAssetMenu(fileName = "GridLayoutData", menuName = "CardGame/GridLayout", order = 2)]
    public class GridLayoutData : ScriptableObject
    {
        [SerializeField] private Vector2[] gridLayouts;

        public Vector2[] GetAllGridLayouts() => gridLayouts;
    }
}