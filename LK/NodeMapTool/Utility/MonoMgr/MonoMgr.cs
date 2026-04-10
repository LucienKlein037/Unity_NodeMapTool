namespace LucienKlein
{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//全局mono.让任何脚本都能执行Update或者开启协程
public class MonoMgr : SingletonBase<MonoMgr>
{
    //***********************************************************************************************************************************************/
    public MonoMgr()
    {
        GameObject obj = new GameObject("MonoMgrHelper");
        mono = obj.AddComponent<MonoMgrHelper>();
    }
    MonoMgrHelper mono;




    //***********************************************************************************************************************************************/
    #region update实现
    public void AddUpdateListener(UnityAction action)
    {
        mono.AddUpdateListener(action);
        //GameObject obj=new GameObject(action.GetType().FullName);
    }
    public void RemoveUpdateListener(UnityAction action)
    {
        mono.RemoveUpdateListener(action);
        
    }
    public void ClearUpdateListener()
    {
        mono.ClearUpdateListener();
    }
    #endregion

    #region 协程实现
    public Coroutine StartCoroutine(string methodName, object value)
    {
        return mono.StartCoroutine(methodName, value);
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return mono.StartCoroutine(routine);
    }

    public void StopCoroutine(IEnumerator routine)
    {
        mono.StopCoroutine(routine);
    }
    public void StopCoroutine(Coroutine routine)
    {
        mono.StopCoroutine(routine);
    }

    public void StopAllCoroutines()
    {
        mono.StopAllCoroutines();
    }


    #endregion

    //***********************************************************************************************************************************************/








}
}
