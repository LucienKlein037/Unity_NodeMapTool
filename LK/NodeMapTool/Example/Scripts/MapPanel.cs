using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LucienKlein
{
    //using OnValueChanged = UnityEngine.OnValueChangedAttribute;

    public class MapPanel : MonoBehaviour
    {
        public Transform IconContent;
        public Transform MapContent;



        //------------------------------------------------------------------------------------------------------------------------/

        //------------------------------------------------------------------------------------------------------------------------/

        public void SetInfo(MapFather map)
        {
            MapContent.ClearChildGameObject();
            map.transform.SetParent(MapContent, false);
            (map.transform as RectTransform).anchoredPosition = Vector2.zero;

        }



    }

}
