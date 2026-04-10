namespace LucienKlein
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public enum e_mapNodeType
    {
        None,
        //普通怪
        monster,

        //初始.城镇
        Town,
        //商人
        Merchant,






    }

    public partial class MapNode
    {
        private partial MapNodeType GetTypeFromEnum(e_mapNodeType e, int row)
        {
            switch (e)
            {
                case e_mapNodeType.None:
                    return new MapNodeType_None(row);
                case e_mapNodeType.monster:
                    return new MapNodeType_Monster(row);
                case e_mapNodeType.Town:
                    return new MapNodeType_Town(row);
                case e_mapNodeType.Merchant:
                    return new MapNodeType_Merchant(row);
                default:
                    throw new Exception("bug");
            }
        }


    }
}
