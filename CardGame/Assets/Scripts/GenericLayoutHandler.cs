using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public abstract class CardLayoutHandler<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private float cellSpacing=2;
        [SerializeField] private T prefab;
        [SerializeField] private RectTransform container;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        private GenericMonoPool<T> pool;
        protected Vector2 dimension;
        protected T[][] objLayout;

        private void Awake()
        {
            pool = new GenericMonoPool<T>(prefab);
        }

        public void CreateLayout(Vector2Int dimension)
        {
            this.dimension = dimension;
            pool.ResetAllPool();
            var containerDimension = container.rect.size;
            var cellSize = Math.Min(containerDimension.x, containerDimension.y) / 
                           Math.Max(dimension.x, dimension.y) - cellSpacing;
            gridLayoutGroup.cellSize = Vector2.one * cellSize;
            gridLayoutGroup.spacing = Vector2.one * cellSpacing;
            objLayout = new T[dimension.x][];
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayoutGroup.constraintCount = dimension.x;
            var k = 0;
            for (var i = 0; i < dimension.x; i++)
            {
                objLayout[i] = new T[dimension.y];
                for (var j = 0; j < dimension.y; j++)
                {
                    var t = pool.GetObject(container);
                    t.transform.SetSiblingIndex(k);
                    var ci = new Vector2Int(i, j);
                    InitCell(t, ci);
                    k++;
                }
            }
        }

        protected abstract void InitCell(T obj, Vector2Int currentIndex);

        protected T GetCardAtIndex(Vector2Int index)
        {
            if (index.x < dimension.x && index.y < dimension.y && index.x>=0 && index.y >=0)
            {
                return objLayout[index.x][index.y];
            }
            return null;
        }
        
    }
}