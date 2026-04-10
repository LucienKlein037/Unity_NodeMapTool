namespace LucienKlein
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class Map1 : MapFather
    {

        //****************************************************************************************************/





        //****************************************************************************************************/
        protected override void InitMapNodeMapInitializer()
        {
            initializer =new MapNodeMapInitializer1();
        }

        public override void CreateMap_UI()
        {
            base.CreateMap_UI();
        }

        public override void MoveToNextNode(int col)
        {
            if (col>MapNodes[NowActiveRow].Count-1)
            {
                Debug.LogWarning(col+">"+(MapNodes[NowActiveRow].Count-1));
                return;
            }

            //当前列失效
            for (int j = 0; j < MapNodes[NowActiveRow].Count; j++)
            {
                MapNodes[NowActiveRow][j].IsActive=false;
            }

            NowNode=MapNodes[NowActiveRow][col];
            float newY = NodeContentRT.sizeDelta.y*.5f-(NowNode.transform as RectTransform).anchoredPosition.y+100;


            NodeContentRT.anchoredPosition=new Vector2(NodeContentRT.anchoredPosition.x, newY);

            NowActiveRow++;
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





    }

}
