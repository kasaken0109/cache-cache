using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPool
{
    bool IsUse { get; }
    void SetUp(Transform parent);
    void Delete();
}

public class ObjectPool<T> where T : Object, IPool
{
    Transform _parent;
    int _setCount;
    List<T> _pool = new List<T>();
    T _getT;

    public void SetUp(T type, Transform parent = null, int setCount = 30)
    {
        _getT = type;
        _parent = parent;
        _setCount = setCount;

        Create();
    }

    void Create()
    {
        for (int i = 0; i < _setCount; i++)
        {
            T t = Object.Instantiate(_getT, _parent);
            t.SetUp(_parent);
            _pool.Add(t);
        }
    }

    public T Respons()
    {
        foreach (T t in _pool)
            if (!t.IsUse) return t;

        Create();
        return Respons();
    }
}
