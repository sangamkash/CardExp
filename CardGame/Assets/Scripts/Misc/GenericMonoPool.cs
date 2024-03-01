using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMonoPool<T> where T: MonoBehaviour
{
    private List<T> activePool;
    private List<T> inActivePool;

    private T prefab;
    
    public GenericMonoPool(T prefab,int capacity=20)
    {
        this.prefab = prefab;
        activePool = new List<T>(capacity);
        inActivePool = new List<T>(capacity);
    }

    public T GetObject(Transform container)
    {
        if (inActivePool.Count > 0)
        {
            var index = inActivePool.Count - 1;
            var t = inActivePool[index];
            activePool.Add(t);
            inActivePool.RemoveAt(index);
            t.transform.parent = container;
            t.gameObject.SetActive(true);
            return t;
        }
        else
        {
            var t = MonoBehaviour.Instantiate(prefab.gameObject, container).GetComponent<T>();
            activePool.Add(t);
            t.gameObject.SetActive(true);
            return t;
        }
    }

    public void ResetAllPool()
    {
        foreach (var obj in activePool)
        {
            inActivePool.Add(obj);
            obj.gameObject.SetActive(false);
        }
        activePool.Clear();
    }

    public void Reset(T obj)
    {
        obj.gameObject.SetActive(false);
        activePool.Remove(obj);
        inActivePool.Add(obj);
    }
    
}
