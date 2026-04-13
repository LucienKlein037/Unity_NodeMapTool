using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LucienKlein
{

    public class MapNodeType_Merchant : MapNodeType
    {

        public MapNodeType_Merchant(int row) : base(row)
        {
        }

        public override void Process()
        {
            Debug.Log("打开商店");
        }

        protected override void Init()
        {
        }

    }

}
