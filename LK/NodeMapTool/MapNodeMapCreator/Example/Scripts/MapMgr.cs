namespace LucienKlein
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    public class MapMgr : SingletonMonoBase<MapMgr>
    {
        //****************************************************************************************************/

        //图标资源路径.里面的内容需要和枚举e_mapNodeType的成员同名
        public string ICON_PATH = "Texture/";
        public float MoveToNextNodeDelayTime = 0.1f;
        //不同的地图Prefab
        public List<GameObject> listMapObj = new();

        [HideInInspector]
        public MapFather NowMap;

        [HideInInspector]
        //一局游戏可能有多个关
        public int nowLevel = 0;

        [HideInInspector]
        //记录经过的节点的数(跨地图)    实际上是当前所在节点的行数再+1
        public int NowPassedNodeRowCount = 0;
        public int NowRow => NowPassedNodeRowCount-1;

        [HideInInspector]
        public MapNode LastSelectedMapNode;

        public UnityAction<int> OnEnterNextNode;

        public GameObject MapPanelPrefab;
        MapPanel mapPanel;

        //----------------------------------------------------------------------------------------------------/
        private void Start()
        {
            MapNode.ICON_PATH=ICON_PATH;
            MapNode.MoveToNextNodeDelayTime=MoveToNextNodeDelayTime;
            CreateMap(0);
        }

        //****************************************************************************************************/
        public void CreateMap(int i)
        {
            nowLevel++;
            NowMap?.gameObject?.DestroyMe();

            MapFather map = listMapObj[i].InstantiateMeReturnComponent<MapFather>();
            NowMap=map;
            NowMap.CreateMap_UI();

            MapPanel p = Instantiate(MapPanelPrefab).GetComponent<MapPanel>();
            p.transform.SetParent(transform, false);

            Debug.Log(map+"----map");
            Debug.Log(p+"----p");

            p.SetInfo(NowMap);
        }

        public void ShowMap()
        {
            if (NowMap==null)
                return;

            mapPanel.gameObject.SetActive(true);

        }
        public void HideMap()
        {
            if (NowMap==null)
                return;

            mapPanel.gameObject.SetActive(false);
        }
        public void MoveToNextNode(int col)
        {
            Debug.Log($"MoveToNextNode:{NowPassedNodeRowCount},{col}");

            if (NowMap==null)
                return;

            NowPassedNodeRowCount++;
            NowMap.MoveToNextNode(col);

            OnEnterNextNode?.Invoke(NowRow);
        }

        public void NextNodeProcess(int col)
        {
            List<MapNode> listMapNode = NowMap.MapNodes[NowPassedNodeRowCount];
            if (col<0||col>=listMapNode.Count)
            {
                Debug.LogError(col);
                return;
            }
            listMapNode[col].Process();
        }

        public void Clear()
        {
            NowPassedNodeRowCount=0;
            if (NowMap!=null)
            {
                NowMap.gameObject.DestroyMe();

            }
        }


        public void SetLastSelectedMapNode(MapNode node)
        {
            this.LastSelectedMapNode=node;
        }

        //------------------------------------------------------------------------------------------------------------------------/


        void ResetOnNewGame()
        {
            Clear();


            NowMap=null;

            nowLevel=0;
            NowPassedNodeRowCount=0;
            LastSelectedMapNode=null;

            OnEnterNextNode=null;
        }
    }


    public partial class MapNode
    {
        //在点击后延迟执行.可能你会用动画事件
        private partial void DelayMoveToNextNode()
        {
            MapMgr.Instance.MoveToNextNode(Index.y);
        }

    }

}
