namespace LucienKlein
{
using System;
using UnityEngine;

public class SingletonMonoBase<T> : UnityEngine.MonoBehaviour where T: UnityEngine.MonoBehaviour
{
    private static T instance ;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                //GameObject obj = new GameObject();
                //instance= obj.AddComponent<T>();
                //obj.name=typeof(T).Name;

                //throw new Exception(typeof(T).Name);

                Debug.LogWarning("Instance为null:"+typeof(T).Name);
                return null;
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("警告:已存在一个instance");
            return;
        }
        instance = this as T;
        if (transform.parent==null)
        {
            DontDestroyOnLoad(gameObject);
        }
    }










}

}
