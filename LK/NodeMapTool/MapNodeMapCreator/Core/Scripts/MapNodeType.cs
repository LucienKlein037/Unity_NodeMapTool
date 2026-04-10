namespace LucienKlein
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    //节点类型类.
    public abstract class MapNodeType
    {

        public MapNodeType(int row)
        {
            this._row=row;
            Init();
        }

        protected int _row;

        protected abstract void Init();
        public abstract void Process();
    }


}
