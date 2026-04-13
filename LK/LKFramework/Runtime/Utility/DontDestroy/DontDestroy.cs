using UnityEngine;

namespace LucienKlein
{

    //让挂载物体不会在加载新场景时被摧毁
    public class DontDestroy : UnityEngine.MonoBehaviour
    {
        //------------------------------------------------------------------------------------------------------------------------------------------------
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

    }

}
