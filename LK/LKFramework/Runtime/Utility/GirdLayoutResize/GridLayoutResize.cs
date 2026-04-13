using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LucienKlein
{

    //根据grid的元素自动设置整体大小(占满)
    public class GridLayoutResize : UnityEngine.MonoBehaviour
    {
        //************************************************************************************************************************/

    #if UNITY_EDITOR

        private void OnValidate()
        {
            Resize();

        }

        [ContextMenu("Resize")]
        public void Resize()
        {
            GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();
            if (gridLayoutGroup==null||transform is not RectTransform)
            {
                return;
            }
            RectTransform rectTransform = (RectTransform)transform;
            int childCount = transform.childCount;
            int constraintCount = gridLayoutGroup.constraintCount;

            float width = 0;
            float height = 0;
            int rowCount = 0;
            int columnCount = 0;

            //int height=gridLayoutGroup.padding.left*2+gridLayoutGroup.


            switch (gridLayoutGroup.constraint)
            {
                case GridLayoutGroup.Constraint.Flexible:
                    return;
                case GridLayoutGroup.Constraint.FixedColumnCount:
                    columnCount=Mathf.Min(childCount, constraintCount);
                    rowCount=Mathf.CeilToInt(childCount*1f/rowCount);
                    break;
                case GridLayoutGroup.Constraint.FixedRowCount:
                    rowCount=Mathf.Min(childCount, constraintCount);
                    columnCount=Mathf.CeilToInt(childCount*1f/rowCount);
                    break;
                default:
                    break;
            }

            width=gridLayoutGroup.padding.left*2+
                gridLayoutGroup.cellSize.x*columnCount+
                gridLayoutGroup.spacing.x*(columnCount-1);

            height=gridLayoutGroup.padding.top*2+
                gridLayoutGroup.cellSize.y*rowCount+
                gridLayoutGroup.spacing.y*(rowCount-1);

            (transform as RectTransform).sizeDelta=new Vector2(width, height);
        }
    #endif









    }

}
