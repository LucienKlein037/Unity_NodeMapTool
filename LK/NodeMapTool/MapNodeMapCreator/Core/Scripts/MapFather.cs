namespace LucienKlein
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Random = UnityEngine.Random;

    //地图类
    public abstract class MapFather : UnityEngine.MonoBehaviour
    {
        //****************************************************************************************************/
        public int Index = -1;
        //********************************************************************************/
        //地图节点预制体
        public GameObject MapNodePrefab;
        //道路预制体
        public GameObject RoadObj;
        //节点父物体
        public RectTransform NodeContentRT;
        //道路父物体
        public RectTransform RoadContent;
        //节点尺寸
        public Vector2 MapNodeSize;
        //节点间隔
        public Vector2 MapNodeSpace;
        //特殊节点尺寸表(行数,尺寸)
        public SerializedDictionary<int, Vector2> DicCustomMapNodeSize = new();
        //底部偏移
        public float PaddingBottom = 128f;
        //顶部偏移
        public float PaddingTop = 128f;

        //********************************************************************************/
        //生效行,可点击
        protected int NowActiveRow = 0;
        public int NowRow => NowActiveRow;
        protected MapNode NowNode;

        protected MapNodeMapInitializerFather initializer;
        //类型节点图
        protected List<List<e_mapNodeType>> NodeTypeMap;
        //节点图
        public List<List<MapNode>> MapNodes = new List<List<MapNode>>();
        //****************************************************************************************************/
        protected virtual void Awake()
        {
            InitMapNodeMapInitializer();
        }
        private void Start()
        {
            NodeContentRT.anchoredPosition=new Vector2(0, NodeContentRT.sizeDelta.y);
        }
        //****************************************************************************************************/
        //移动到下个节点,更新生效行
        public virtual void MoveToNextNode(int col)
        {
            if (NowActiveRow==0)
            {
                NowNode=MapNodes[0][0];
            }

            if (col>MapNodes[NowActiveRow].Count-1)
            {
                Debug.LogWarning("超过了");
                return;
            }

            if (NowActiveRow>0 &&!NowNode.nextMapNodes.Contains(MapNodes[NowActiveRow][col]))
            {
                Debug.LogWarning("不是连接的点");
                return;
            }
            //当前列失效
            for (int j = 0; j < MapNodes[NowActiveRow].Count; j++)
            {
                MapNodes[NowActiveRow][j].IsActive=false;
            }
            //
            NowActiveRow++;
            //如果是最后一列就不需要让下一列生效了;
            if (NowActiveRow<=MapNodes.Count-1)
            {
                //下一列改为活跃
                for (int j = 0; j < MapNodes[NowActiveRow].Count; j++)
                {
                    if (NowNode.nextMapNodes.Contains(MapNodes[NowActiveRow][j]))
                    {
                        MapNodes[NowActiveRow][j].IsActive=true;
                    }
                }
            }
        }

        //基于Scroll View的地图
        public virtual void CreateMap_UI()
        {
            NodeTypeMap=initializer.GetMapNodeTypeMap();
            //总高度=总节点高度*普通节点数 + 各种特殊节点数高度累加 + 节点间隔*普通节点数-1 + 底部留白 + 顶部留白 + 各种特殊节点额外间隔累加
            float TotalHeight = MapNodeSize.y*(NodeTypeMap.Count-DicCustomMapNodeSize.Count)+
                MapNodeSpace.y*(NodeTypeMap.Count-1-DicCustomMapNodeSize.Count)+
                PaddingBottom+PaddingTop;
            foreach (Vector2 size in DicCustomMapNodeSize.Values)
            {
                //尺寸
                TotalHeight+=size.y;
                //间隔
                TotalHeight+=size.y*.5f*2;
            }

            NodeContentRT.sizeDelta=new Vector2(NodeContentRT.sizeDelta.x, TotalHeight);
            NodeContentRT.localPosition=new Vector3(0, TotalHeight, 0);

            Vector3 firstLocalPos = new Vector3(0, -TotalHeight*0.5f+MapNodeSize.y*0.5f+PaddingBottom, 0);

            float nowHeight = -TotalHeight*0.5f+MapNodeSize.y*0.5f+PaddingBottom;
            for (int i = 0; i < NodeTypeMap.Count; i++)
            {
                //每行添加高度
                if (i!=0)
                {
                    bool isCustomSize = false;
                    bool isCustomSize2 = false;
                    foreach (var item in DicCustomMapNodeSize.Keys)
                    {
                        if (item==i)
                        {
                            isCustomSize=true;
                            break;
                        }
                        if (item+1==i)
                        {
                            isCustomSize2=true;
                            break;
                        }
                    }

                    float addHeight;
                    if (isCustomSize)
                    {
                        addHeight=DicCustomMapNodeSize[i].y+DicCustomMapNodeSize[i].y*.5f;
                    }
                    else if (isCustomSize2)
                    {
                        addHeight=DicCustomMapNodeSize[i-1].y+DicCustomMapNodeSize[i-1].y*.5f;
                    }
                    else
                    {
                        addHeight=MapNodeSize.y+MapNodeSpace.y;
                    }

                    //float addHeight = !isCustomSize ? MapNodeSize.y+MapNodeSpace.y : DicCustomMapNodeSize[i].y+DicCustomMapNodeSize[i].y*.5f;
                    nowHeight+=addHeight;
                }


                MapNodes.Add(new());
                for (int j = 0; j < NodeTypeMap[i].Count; j++)
                {
                    GameObject obj = GameObject.Instantiate(MapNodePrefab);
                    ((obj.transform)as RectTransform).sizeDelta=new Vector2(MapNodeSize.x, MapNodeSize.y);
                    obj.transform.SetParent(NodeContentRT, false);
                    MapNode node = obj.GetComponent<MapNode>();

                    node.SetInfo(NodeTypeMap[i][j], new Vector2Int(i, j), GetLocalPos(), Index);

                    foreach (var item in DicCustomMapNodeSize.Keys)
                    {
                        if (i==item)
                        {
                            (node.transform as RectTransform).sizeDelta=DicCustomMapNodeSize[i];
                            break;
                        }
                    }

                    MapNodes[i].Add(node);

                    //********************************************************************************/
                    Vector3 GetLocalPos()
                    {

                        float x;
                        float y;
                        float z;


                        y = nowHeight;
                        z = 0;

                        switch (NodeTypeMap[i].Count)
                        {
                            case 1:
                                x=firstLocalPos.x;
                                break;
                            case 2:
                                x=-1f*MapNodeSize.x-1f*MapNodeSpace.x
                               + j * 2f* MapNodeSize.x + j * 2f* MapNodeSpace.x;
                                break;
                            case 3:
                                x=-1.25f*MapNodeSize.x-1.25f*MapNodeSpace.x
                               + j * 1.25f* MapNodeSize.x + j * 1.25f* MapNodeSpace.x;
                                break;
                            case 4:
                                x=-1.5f*MapNodeSize.x-1.5f*MapNodeSpace.x
                               + j * MapNodeSize.x + j * MapNodeSpace.x;
                                break;
                            case 5:

                            default:
                                float offset = (NodeTypeMap[i].Count - 1) / 2f;
                                x = -offset * (MapNodeSize.x + MapNodeSpace.x)
                                    + j * (MapNodeSize.x + MapNodeSpace.x);
                                break;
                        }

                        x+=Random.Range(-0.25f, 0.25f)*MapNodeSize.x;
                        y+=Random.Range(-0.25f, 0.25f)*MapNodeSize.y;



                        return new Vector3(x, y, z);
                    }

                }
            }
            for (int i = 0; i < MapNodes[0].Count; i++)
            {
                MapNodes[0][i].IsActive=true;
            }
            NowActiveRow = 0;

            //********************************************************************************/设置上下级关系
            CreateRelation();
            //********************************************************************************/生成道路
            CreateRoad();

        }
        //****************************************************************************************************/
        protected abstract void InitMapNodeMapInitializer();

        //创建上下级关系
        void CreateRelation()
        {
            InitNet();
            if (MapNodes.Count==0)
            {
                throw new Exception("bug");
            }
            int rowCount = MapNodes.Count;
            for (int i = 0; i < rowCount-1; i++)
            {
                List<MapNode> nowNodes = MapNodes[i];
                List<MapNode> nextNodes = MapNodes[i+1];

                for (int k = 0; k < nowNodes.Count; k++)
                {
                    MapNode node = nowNodes[k];
                    node.nextMapNodes.Clear();
                }

                if (nowNodes.Count>5||nextNodes.Count>5)
                {
                    throw new Exception("一行最多五列,在这个脚本搜索TODO进行补充");
                }
                List<List<bool>> boolNets = GetNet(nowNodes.Count, nextNodes.Count);
                //返还的boolNets只会是 少数对多数的情况, 当当前行节点数比下一行行节点数更多时 要注意反转一下
                bool isFlip = nowNodes.Count>nextNodes.Count;


                for (int m = 0; m < boolNets.Count; m++)
                {
                    for (int n = 0; n < boolNets[m].Count; n++)
                    {
                        bool element = boolNets[m][n];
                        if (element)
                        {
                            if (isFlip)
                            {
                                nowNodes[n].nextMapNodes.Add(nextNodes[m]);
                            }
                            else
                            {
                                nowNodes[m].nextMapNodes.Add(nextNodes[n]);

                            }
                        }

                    }
                }
            }
            //--------------------------------------------------------------------------------/

            List<List<bool>> GetNet(int nowNodesCount, int nextNodesCount)
            {

                int smallCount = nowNodesCount<=nextNodesCount ? nowNodesCount : nextNodesCount;
                int bigCount = nowNodesCount>nextNodesCount ? nowNodesCount : nextNodesCount;

                List<List<List<bool>>> boolNets = AllboolNets[(smallCount, bigCount)];


                return boolNets[LKRandom.ListIndex(boolNets.Count)];
            }
        }
        //生成道路
        protected virtual void CreateRoad()
        {
            if (MapNodes.Count==0)
            {
                throw new Exception("bug");
            }
            RectTransform RoadRT;
            //Vector3 tempSize;
            for (int i = 0; i < MapNodes.Count; i++)
            {
                for (int j = 0; j < MapNodes[i].Count; j++)
                {
                    MapNode node = MapNodes[i][j];
                    for (int k = 0; k < node.nextMapNodes.Count; k++)
                    {
                        MapNode nextNode = node.nextMapNodes[k];
                        AddRoad(node.transform, nextNode.transform);

                        //********************************************************************************/
                        void AddRoad(Transform NowwNode, Transform NextNode)
                        {
                            Vector3 posA = NowwNode.position;
                            Vector3 posB = NextNode.position;

                            // UI 方向向量（从 A 指向 B）
                            Vector3 dirUnit = (posB - posA).normalized;


                            // 获取宽度尺寸（x轴）
                            float offsetA = 1.5f * NowwNode.GetRectTransform().rect.width*0.5f;
                            float offsetB = 1.5f * NextNode.GetRectTransform().rect.width*0.5f;

                            // 计算点 a 和点 b 的世界坐标
                            posA = posA + dirUnit * offsetA;
                            posB = posB - dirUnit * offsetB;

                            Transform Road = RoadObj.InstantiateMeReturnComponent<Transform>();
                            Road.SetParent(RoadContent, false);
                            // 计算 A 和 B 之间的方向和距离

                            Vector3 direction = posB - posA;
                            float distance = direction.magnitude;
                            RoadRT = Road as RectTransform;

                            // 设置 road 的高度为 A 和 B 之间的距离
                            RoadRT.sizeDelta = new Vector2(RoadRT.sizeDelta.x, distance);

                            // 计算旋转角度，使 road 上端对准 A，下端对准 B
                            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg-90;
                            Road.rotation = Quaternion.Euler(0, 0, angle);

                            // 设置 road 的位置为 A 和 B 中点
                            Road.position = (posA+posB) / 2;
                        }
                    }
                }
            }
        }

        //(int,int) (少的节点数,多的节点数):  组合,当前第几个点,下行第几个点 =>是否连接
        Dictionary<(int, int), List<List<List<bool>>>> AllboolNets;
        //初始化道路网(通过穷举不同个数时的上下级可能的关系来分配好关系,这里只穷举到5) TODO:后续可以增加更多的情况
        void InitNet()
        {
            AllboolNets=new();
            int m;
            int n;

            //----------------------------------------/分类并创建.创建个数为情况数
            m=1;
            n=1;
            AllboolNets.Add((m, n), new());
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            m=1;
            n=2;
            AllboolNets.Add((m, n), new());
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            m =1;
            n=3;
            AllboolNets.Add((m, n), new());
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            m=1;
            n=4;
            AllboolNets.Add((m, n), new());
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            m=1;
            n=5;
            AllboolNets.Add((m, n), new());
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));

            m=2;
            n=2;
            AllboolNets.Add((m, n), new());
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            //比如 这里表示下级2个点, 上级3个点 有2种情况(或上3下2)
            m =2;
            n=3;
            AllboolNets.Add((m, n), new());
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            m=2;
            n=4;
            AllboolNets.Add((m, n), new());
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            m=2;
            n=5;
            AllboolNets.Add((m, n), new());
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));

            m=3;
            n=3;
            AllboolNets.Add((m, n), new());
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            m=3;
            n=4;
            AllboolNets.Add((m, n), new());
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            m=3;
            n=5;
            AllboolNets.Add((m, n), new());
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));

            m=4;
            n=4;
            AllboolNets.Add((m, n), new());
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            m=4;
            n=5;
            AllboolNets.Add((m, n), new());
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));

            m=5;
            n=5;
            AllboolNets.Add((m, n), new());
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));
            AllboolNets[(m, n)].Add(CreateNewNet(m, n));

            //----------------------------------------/分配连接线, 如bools[0][1]=true表示 当前行第1个点连接下一行第2个点.如 
            List<List<bool>> bools;
            bools= AllboolNets[(1, 1)][0];
            bools[0][0]=true;
            bools=AllboolNets[(1, 2)][0];
            bools[0][0]=true;
            bools[0][1]=true;
            bools=AllboolNets[(1, 3)][0];
            bools[0][0]=true;
            bools[0][1]=true;
            bools[0][2]=true;
            bools=AllboolNets[(1, 4)][0];
            bools[0][0]=true;
            bools[0][1]=true;
            bools[0][2]=true;
            bools[0][3]=true;
            bools=AllboolNets[(1, 5)][0];
            bools[0][0]=true;
            bools[0][1]=true;
            bools[0][2]=true;
            bools[0][3]=true;
            bools[0][4]=true;

            bools=AllboolNets[(2, 2)][0];
            bools[0][0]=true;
            bools[1][1]=true;
            //这里表示 下2上3的第一种情况 下面 下0连接上0 下1连接上1和2
            bools=AllboolNets[(2, 3)][0];
            bools[0][0]=true;
            bools[1][1]=true;
            bools[1][2]=true;
            //这里表示 下2上3的第二种情况 下面 下0连接上0,1 下1连接上2
            bools=AllboolNets[(2, 3)][1];
            bools[0][0]=true;
            bools[0][1]=true;
            bools[1][2]=true;
            bools=AllboolNets[(2, 4)][0];
            bools[0][0]=true;
            bools[0][1]=true;
            bools[1][2]=true;
            bools[1][3]=true;
            bools=AllboolNets[(2, 4)][1];
            bools[0][0]=true;
            bools[0][1]=true;
            bools[0][2]=true;
            bools[1][3]=true;
            bools=AllboolNets[(2, 4)][2];
            bools[0][0]=true;
            bools[1][1]=true;
            bools[1][2]=true;
            bools[1][3]=true;
            bools=AllboolNets[(2, 5)][0];
            bools[0][0]=true;
            bools[0][1]=true;
            bools[1][2]=true;
            bools[1][3]=true;
            bools[1][4]=true;
            bools=AllboolNets[(2, 5)][1];
            bools[0][0]=true;
            bools[0][1]=true;
            bools[0][2]=true;
            bools[1][3]=true;
            bools[1][4]=true;

            bools=AllboolNets[(3, 3)][0];
            bools[0][0]=true;
            bools[1][1]=true;
            bools[2][2]=true;
            bools=AllboolNets[(3, 3)][1];
            bools[0][0]=true;
            bools[1][1]=true;
            bools[2][2]=true;
            bools=AllboolNets[(3, 3)][2];
            bools[0][0]=true;
            bools[1][0]=true;
            bools[1][1]=true;
            bools[2][2]=true;
            bools=AllboolNets[(3, 3)][3];
            bools[0][0]=true;
            bools[1][1]=true;
            bools[1][2]=true;
            bools[2][2]=true;


            bools=AllboolNets[(3, 4)][0];
            bools[0][0]=true;
            bools[0][1]=true;
            bools[1][2]=true;
            bools[2][3]=true;
            bools=AllboolNets[(3, 4)][1];
            bools[0][0]=true;
            bools[1][1]=true;
            bools[1][2]=true;
            bools[2][3]=true;
            bools=AllboolNets[(3, 4)][2];
            bools[0][0]=true;
            bools[1][1]=true;
            bools[2][2]=true;
            bools[2][3]=true;
            bools=AllboolNets[(3, 5)][0];
            bools[0][0]=true;
            bools[0][1]=true;
            bools[1][2]=true;
            bools[1][3]=true;
            bools[2][4]=true;
            bools=AllboolNets[(3, 5)][1];
            bools[0][0]=true;
            bools[1][1]=true;
            bools[1][2]=true;
            bools[2][3]=true;
            bools[2][4]=true;
            bools=AllboolNets[(3, 5)][2];
            bools[0][0]=true;
            bools[0][1]=true;
            bools[1][2]=true;
            bools[2][3]=true;
            bools[2][4]=true;
            bools=AllboolNets[(3, 5)][3];
            bools[0][0]=true;
            bools[1][1]=true;
            bools[1][2]=true;
            bools[1][3]=true;
            bools[2][4]=true;

            bools=AllboolNets[(4, 4)][0];
            bools[0][0]=true;
            bools[1][1]=true;
            bools[2][2]=true;
            bools[3][3]=true;
            bools=AllboolNets[(4, 4)][1];
            bools[0][0]=true;
            bools[1][0]=true;
            bools[1][1]=true;
            bools[2][2]=true;
            bools[3][3]=true;
            bools=AllboolNets[(4, 4)][2];
            bools[0][0]=true;
            bools[1][1]=true;
            bools[2][2]=true;
            bools[2][3]=true;
            bools[3][3]=true;

            bools=AllboolNets[(4, 5)][0];
            bools[0][0]=true;
            bools[0][1]=true;
            bools[1][2]=true;
            bools[2][3]=true;
            bools[3][4]=true;
            bools=AllboolNets[(4, 5)][1];
            bools[0][0]=true;
            bools[1][1]=true;
            bools[1][2]=true;
            bools[2][3]=true;
            bools[3][4]=true;
            bools=AllboolNets[(4, 5)][2];
            bools[0][0]=true;
            bools[1][1]=true;
            bools[2][2]=true;
            bools[2][3]=true;
            bools[3][4]=true;
            bools=AllboolNets[(4, 5)][3];
            bools[0][0]=true;
            bools[1][1]=true;
            bools[2][2]=true;
            bools[3][3]=true;
            bools[3][4]=true;

            bools=AllboolNets[(5, 5)][0];
            bools[0][0]=true;
            bools[1][1]=true;
            bools[2][2]=true;
            bools[3][3]=true;
            bools[4][4]=true;
            bools=AllboolNets[(5, 5)][1];
            bools[0][0]=true;
            bools[1][0]=true;
            bools[1][1]=true;
            bools[2][2]=true;
            bools[3][3]=true;
            bools[4][4]=true;
            bools=AllboolNets[(5, 5)][2];
            bools[0][0]=true;
            bools[1][1]=true;
            bools[2][2]=true;
            bools[3][3]=true;
            bools[3][4]=true;
            bools[4][4]=true;
            //--------------------------------------------------------------------------------/
            List<List<bool>> CreateNewNet(int nowNodesCount, int nextNodesCount)
            {
                List<List<bool>> boolNets = new();
                for (int i = 0; i < nowNodesCount; i++)
                {
                    boolNets.Add(new List<bool>());
                    for (int j = 0; j < nextNodesCount; j++)
                    {
                        boolNets[i].Add(false);
                    }
                }
                return boolNets;
            }
        }


        Vector2 GetMapNodeSize(int row)
        {
            Vector2 size;
            if (DicCustomMapNodeSize.ContainsKey(row))
            {
                size =DicCustomMapNodeSize[row];
            }
            else
            {
                size=MapNodeSize;
            }
            return size;

        }
    }












}
