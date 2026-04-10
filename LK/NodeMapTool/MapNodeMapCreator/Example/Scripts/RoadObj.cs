namespace LucienKlein
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Random = UnityEngine.Random;


    //TODO:根据自己要求去写道路逻辑.可以采用LineRenderer或者Mesh等方式.这里只是简单的实例化一些点来表示道路.
    public class RoadObj : UnityEngine.MonoBehaviour
    {
        public GameObject RoadPointPrefab;
        //点间距.用来计算需要多少个点
        public float Space = 5;
        //一个道路最多多少个点.用来限制实例化的点数量.
        public int MaxCount = 60;


        int Count = 0;

        private void Start()
        {
            RoadPointPrefab.SetActive(false);

            Vector3[] cps = CreateControlPoints();
            Vector3[] points = LKBezier.GetBezierCurve(Count, cps);
            for (int i = 0; i < Count; i++)
            {
                GameObject go = Instantiate(RoadPointPrefab);
                go.transform.SetParent(transform, false);
                go.SetActive(true);
                go.transform.position=points[i];
            }
        }
        Vector3[] CreateControlPoints()
        {
            Vector3[] controlPoints = new Vector3[4];

            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector2 size = rectTransform.rect.size;
            float ControlPointXMax = size.x/2;
            Count=(int)(size.y/Space)+1;
            Count=Mathf.Min(Count, MaxCount);

            GameObject obj = new();
            obj.transform.SetParent(transform);
            RectTransform ObjRectTransform = obj.AddComponent<RectTransform>();
            ObjRectTransform.sizeDelta = Vector2.zero; // 设置大小为0，避免影响布局

            ObjRectTransform.localPosition = new Vector3(0, -size.y / 2, 0);
            controlPoints[0]=ObjRectTransform.position;

            ObjRectTransform.localPosition = new Vector3(Random.Range(-ControlPointXMax, ControlPointXMax), -size.y / 2+size.y / 3, 0);
            controlPoints[1]=ObjRectTransform.position;

            ObjRectTransform.localPosition = new Vector3(Random.Range(-ControlPointXMax, ControlPointXMax), -size.y / 2+ 2*size.y / 3, 0);
            controlPoints[2]=ObjRectTransform.position;

            ObjRectTransform.localPosition = new Vector3(0, size.y / 2, 0);
            controlPoints[3]=ObjRectTransform.position;

            Destroy(obj);
            return controlPoints;

        }
















    }

}
