using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LucienKlein
{

    //地图节点图的初始化器父类,用来生成主要的地图数据(如行数,每行的节点数,每个节点的类型等).一个游戏可能有多张地图,每个图需要各自实现地图初始化器
    public abstract class MapNodeMapInitializerFather
    {
        //****************************************************************************************************/

        public abstract List<List<e_mapNodeType>> GetMapNodeTypeMap();

        /// <summary>
        /// 对nodeTypeMap进行随机分配
        /// </summary>
        /// <param name="nodeTypeMap">节点图</param>
        /// <param name="type">节点类型</param>
        /// <param name="count">数量</param>
        /// <param name="rows">在这些行里随机</param>
        protected void RandomDistribute(ref List<List<e_mapNodeType>> nodeTypeMap, e_mapNodeType type, int count = 0, params int[] rows)
        {
            if (count<=0)
                return;

            //指定行里所有的NoneNode;
            Dictionary<int, List<int>> NewIndexs = new();
            //指定行里所有有NoneNode的行
            List<int> listHaveNewIndexRow = new();
            //临时的当前的行
            List<int> nowRow;
            //存储延迟Remove的行的key
            List<int> delayRemoveKey = new();
            //抽中的Index
            Vector2Int nowSelect = Vector2Int.zero;

            //初步添加
            if (rows.Length==0)
            {
                for (int i = 0; i < nodeTypeMap.Count; i++)
                {
                    NewIndexs.Add(i, new());
                    listHaveNewIndexRow.Add(i);
                    for (int j = 0; j < nodeTypeMap[i].Count; j++)
                    {
                        if (nodeTypeMap[i][j]==e_mapNodeType.None)
                        {
                            NewIndexs[i].Add(j);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < rows.Length; i++)
                {
                    NewIndexs.Add(rows[i], new List<int>());
                    listHaveNewIndexRow.Add(rows[i]);

                    for (int j = 0; j < nodeTypeMap[rows[i]].Count; j++)
                    {
                        if (nodeTypeMap[rows[i]][j]==e_mapNodeType.None)
                        {
                            NewIndexs[rows[i]].Add(j);
                        }
                    }
                }
            }
            //筛查掉没有NoneNode的行
            foreach (var key in NewIndexs.Keys)
            {
                nowRow=NewIndexs[key];
                if (nowRow.Count==0)
                {
                    delayRemoveKey.Add(key);
                    listHaveNewIndexRow.Remove(key);
                }
            }
            for (int i = 0; i < delayRemoveKey.Count; i++)
            {
                NewIndexs.Remove(delayRemoveKey[i]);
            }


            for (int z = 0; z < 10000; z++)
            {
                if (count<=0||NewIndexs.Count==0)
                {
                    return;
                }



                nowSelect.x=listHaveNewIndexRow[LKRandom.ListIndex(listHaveNewIndexRow.Count)];
                nowRow=NewIndexs[nowSelect.x];
                nowSelect.y=nowRow[LKRandom.ListIndex(nowRow.Count)];
                SetType(ref nodeTypeMap, nowSelect, type);
                count--;

                nowRow.Remove(nowSelect.y);
                if (nowRow.Count==0)
                {
                    listHaveNewIndexRow.Remove(nowSelect.x);
                    NewIndexs.Remove(nowSelect.x);
                }

                if (z>9000)
                {
                    Debug.LogWarning("警告:!!!!!!!!!!!!!!!!!!!!!!!!");
                    break;
                }
            }



        }

        /// <summary>
        /// 指定行所有的None类型会被设置为指定类型,已被分配类型的则忽略.
        /// </summary>
        /// <param name="nodeTypeMap"></param>
        /// <param name="type"></param>
        /// <param name="Row"></param>
        protected void RowDistribute(ref List<List<e_mapNodeType>> nodeTypeMap, e_mapNodeType type, int Row)
        {
            if (nodeTypeMap.Count-1<Row)
            {
                Debug.LogWarning("警告:层数不够");
                return;
            }

            for (int i = 0; i < nodeTypeMap[Row].Count; i++)
            {
                if (nodeTypeMap[Row][i]==e_mapNodeType.None)
                {
                    //nodeTypeMap[Row][i]=type;
                    SetType(ref nodeTypeMap, new Vector2Int(Row, i), type);
                }
            }

            return;
        }

        /// <summary>
        /// 所有的None设置为指定类型,通常是最后调用
        /// </summary>
        /// <param name="nodeTypeMap"></param>
        /// <param name="type"></param>
        protected void AllDistribute(ref List<List<e_mapNodeType>> nodeTypeMap, e_mapNodeType type)
        {
            for (int i = 0; i < nodeTypeMap.Count; i++)
            {
                RowDistribute(ref nodeTypeMap, type, i);
            }
        }

        protected void SetType(ref List<List<e_mapNodeType>> nodeTypeMap, Vector2Int index, e_mapNodeType newType)
        {
            if (newType==e_mapNodeType.None)
            {
                throw new Exception("bug");
            }
            if (nodeTypeMap[index.x][index.y]!=e_mapNodeType.None)
            {
                Debug.LogWarning("警告:修改了非None的节点类型");
            }
            nodeTypeMap[index.x][index.y]=newType;
        }

        protected List<Vector2Int> GetNoneTypeIndexs(ref List<List<e_mapNodeType>> nodeTypeMap)
        {
            List<Vector2Int> indexs = new List<Vector2Int>();
            for (int i = 0; i < nodeTypeMap.Count; i++)
            {
                for (int j = 0; j < nodeTypeMap[i].Count; j++)
                {
                    if (nodeTypeMap[i][j]==e_mapNodeType.None)
                    {
                        indexs.Add(new Vector2Int(i, j));
                    }
                }
            }
            return indexs;
        }
    }

}
