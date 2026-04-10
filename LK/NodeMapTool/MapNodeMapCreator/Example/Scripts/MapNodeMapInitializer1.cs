namespace LucienKlein
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class MapNodeMapInitializer1 : MapNodeMapInitializerFather
    {
        //****************************************************************************************************/
        public MapNodeMapInitializer1()
        {
            Init();
        }
        const int LevelCount = 11;



        public override List<List<e_mapNodeType>> GetMapNodeTypeMap()
        {
            List<List<e_mapNodeType>> nodeTypeMap = GetNoneNodeTypeMap();

            RowDistribute(ref nodeTypeMap, e_mapNodeType.Town, 0);
            RandomDistribute(ref nodeTypeMap, e_mapNodeType.Merchant, 1, 8);
            AllDistribute(ref nodeTypeMap, e_mapNodeType.monster);
            return nodeTypeMap;
        }


        public List<List<e_mapNodeType>> GetNoneNodeTypeMap()
        {
            List<List<e_mapNodeType>> nodeTypeMap = new();
            for (int i = 0; i < LevelCount; i++)
            {
                nodeTypeMap.Add(new List<e_mapNodeType>());
            }
            for (int i = 0; i < nodeTypeMap.Count; i++)
            {
                int yCount = GetColumnCount(i);
                for (int j = 0; j < yCount; j++)
                {
                    nodeTypeMap[i].Add(e_mapNodeType.None);
                }
            }
            return nodeTypeMap;
        }

        //每行多少个 
        int[] AllColumnCountInRow;

        int GetColumnCount(int row)
        {
            return AllColumnCountInRow[row];
        }

        void Init()
        {
            AllColumnCountInRow=new int[LevelCount];
            //每行多少个对应的的可能性 1 2 3 4 5分别为0% 30% 35% 20% 15%
            float[] ColumnP = new float[] { .0f, .3f, .35f, .2f, .15f };

            //直接设定一些特殊行的个数,假如第一个是城镇,通常就是1个,最后一个是boss,通常也是1个
            SetCount(0, 1);
            SetCount(10, 1);

            for (int n = 0; n < AllColumnCountInRow.Length; n++)
            {
                if (AllColumnCountInRow[n]!=0)
                {
                    continue;
                }

                float nowP = 0;
                float randomNum = Random.Range(0, 1f);
                //通过累加概率的方式来确定个数,比如随机数是0.4,那么就落在了2个的区间(0.3-0.65),所以这一行就是2个
                for (int i = 0; i < ColumnP.Length; i++)
                {
                    nowP += ColumnP[i];
                    if (randomNum<nowP)
                    {
                        AllColumnCountInRow[n]= i+1;
                        break;
                    }
                }

            }




            void SetCount(int row, int count)
            {
                if (count<=0||count>ColumnP.Length)
                {
                    throw new Exception("bug");
                }
                AllColumnCountInRow[row]=count;
            }
        }



    }

}
