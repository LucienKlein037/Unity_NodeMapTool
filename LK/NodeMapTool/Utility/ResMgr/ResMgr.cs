namespace LucienKlein
{
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class ResMgr : SingletonBase<ResMgr>
{
    Sprite none;
    //纯透明的图片,用来占位
    //public Sprite SpriteNone
    //{
    //    get
    //    {
    //        if (none == null)
    //        {
    //            none=ResMgr.Instance.Load<Sprite>("Art/Sprite/General/None");
    //        }
    //        return none;
    //    }
    //}
    //纯白图片
    //static Sprite _whiteSprite;
    //public static Sprite WhiteSprite
    //{
    //    get
    //    {
    //        if (_whiteSprite==null)
    //        {
    //            _whiteSprite= ResMgr.Instance.Load<Sprite>("Art/Sprite/General/White");
    //        }
    //        return _whiteSprite;
    //    }
    //}

    string tempName = null;
    //------------------------------------------------------------------------------------------------------------------------------------------------
    public GameObject Load(string path)
    {
        return Load(path);
    }
    public T Load<T>(string path) where T : Object
    {

        T result = Resources.Load<T>(path);
        if (result==null)
        {
            Debug.LogWarning("未找到路径资源"+path);
        }

        if (result is GameObject)
        {
            GameObject obj = GameObject.Instantiate(result as GameObject);

            tempName = path.Substring(path.LastIndexOf("/") + 1);
            obj.name= tempName;

            return obj as T;
        }
        if (result==null&& typeof(T) == typeof(Sprite))
        {
            //没找到的话我会用一个纯透明的图片占位
            //result=SpriteNone as T;
            result=null;
        }



        return result;
    } 

    public T[] LoadAll<T>(string path) where T : ScriptableObject
    {
        return Resources.LoadAll<T>(path);
    }


    public T Load<T>(string folderPath, string fileNameWithoutExtension) where T : Object
    {
        var assets = Resources.LoadAll(folderPath);

        foreach (var asset in assets)
        {
            if (asset.name == fileNameWithoutExtension  && asset is T matched)
            {
                return matched;
            }
        }

        return null;

    }
    public void LoadAsync<T>(string path, UnityAction<T> callback, bool keepName = true) where T : Object
    {
        MonoMgr.Instance.StartCoroutine(LoadAsyncCoro<T>(path, callback, keepName));
    }




    //------------------------------------------------------------------------------------------------------------------------------------------------
    IEnumerator LoadAsyncCoro<T>(string path, UnityAction<T> callback, bool keepName) where T : Object
    {

        ResourceRequest rr = Resources.LoadAsync<T>(path);
        yield return rr;
        if (rr.asset is GameObject)
        {
            GameObject obj = GameObject.Instantiate(rr.asset as GameObject);
            if (keepName)
            {
                tempName = path.Substring(path.LastIndexOf("/") + 1);
                obj.name = tempName;
                Debug.Log(path);
                Debug.Log(obj.name);
            }
            callback(obj as T);

        }
        else
        {
            callback(rr.asset as T);
        }
    }











}


//public Sprite LoadSpriteFromSprites(string path,int index)
//{
//    Sprite[] sprites = Resources.LoadAll<Sprite>(path);
//    if (sprites!=null&&sprites.Length-1>=index)
//    {
//        return sprites[index];
//    }
//    return null;
//}
}
