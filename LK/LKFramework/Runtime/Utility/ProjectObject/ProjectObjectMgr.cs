using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LucienKlein
{

    //递增的ID来分配.这样就可以通过ID获取对象了.
    public sealed class ProjectObjectMgr<T> : SingletonBase<ProjectObjectMgr<T>> where T : class, IProjectObject
    {


        public List<T> list = new();

        public static void _Add(T info)
        {
            Instance.Add(info);
        }
        public static T _Get(int id)
        {
            return Instance.Get(id);
        }

        //慎用!非常容易出bug
        public static void _Clear()
        {
            Instance.Clear();
        }

        public void Add(T info)
        {
            list.Add(info);
            info.InstanceID=list.Count-1;
        }
        public T Get(int id)
        {
            if (id<0||id>list.Count-1)
            {
                return null;
            }
            if (list[id]==null)
            {
                return null;
            }
            return list[id];


        }

        //慎用!非常容易出bug
        public void Clear()
        {
            list.Clear();
        }





    }

    public interface IProjectObject
    {
        public int InstanceID
        {
            get;
            set;
        }
    }

}
