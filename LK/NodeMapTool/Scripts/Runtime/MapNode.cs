using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LucienKlein
{

    public partial class MapNode : MonoBehaviour
    {
        //****************************************************************************************************/
        public static string ICON_PATH = "Texture/";
        public static float MoveToNextNodeDelayTime = 0.1f;

        public Image IconImage;
        public Button btn;
        //在地图中的行列索引.
        public Vector2Int Index;
        //节点类型.不同的节点类型会有不同的事件,如战斗,商店等
        public MapNodeType NodeType;
        //未激活和激活的颜色
        public Color inactiveColor;
        public Color activeColor;

        //下一级节点.影响移动到这个节点后,允许移动到哪些节点.在地图上,每个节点会有一些线连接着下一级节点,玩家只能沿着这些线移动
        public List<MapNode> nextMapNodes = new List<MapNode>();

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                IconImage.color=_isActive ? activeColor : inactiveColor;
            }

        }
        bool _isActive = false;
        //****************************************************************************************************/
        private void Start()
        {
            btn.onClick.AddListener(Process);
        }
        //****************************************************************************************************/

        public virtual void SetType(e_mapNodeType type, int row)
        {

            NodeType = GetTypeFromEnum(type, row);
            Sprite sp = ResMgr.Instance.Load<Sprite>(ICON_PATH+type.ToString());
            if (sp!=null)
            {
                IconImage.sprite=sp;
            }
        }

        //点击时触发
        public void Process()
        {
            if (!IsActive)
            {
                return;
            }
            if (NodeType==null)
                return;

            MapMgr.Instance.SetLastSelectedMapNode(this);
            NodeType.Process();
            Invoke("DelayMoveToNextNode", MoveToNextNodeDelayTime);
            Debug.Log(NodeType.GetType().Name+"进行Process");
        }
        //------------------------------------------------------------------------------------------------------------------------/
        private partial void DelayMoveToNextNode();
        //------------------------------------------------------------------------------------------------------------------------/
        public void SetInfo(e_mapNodeType type, Vector2Int index, Vector3 localPos, int mapIndex)
        {
            gameObject.name=type.ToString();
            Index=index;
            IsActive=false;
            SetType(type, index.x);
            (transform as RectTransform).anchoredPosition=localPos;
        }

        private partial MapNodeType GetTypeFromEnum(e_mapNodeType e, int row);



    }

}
