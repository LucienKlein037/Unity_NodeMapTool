namespace LucienKlein
{
using System;
using UnityEngine;

//绘制贝塞尔曲线工具类
public class LKBezier 
{
    public static Vector3[] GetBezierCurve(int count,params Vector3[] p)
    {
        int length=p.Length;
        if (length<2||length>4)
        {
            throw new Exception("暂只支持1~3阶即2,3,4个点"+p.Length);
        }

        Vector3[] points=new Vector3[count];

        for (int i = 0; i < points.Length; i++)
        {
            switch (length)
            {
                case 2:
                    points[i]=CalculateBezierPoint(i*1f/(points.Length-1), p[0], p[1]);
                    break;
                case 3:
                    points[i]=CalculateBezierPoint(i*1f/(points.Length-1), p[0], p[1], p[2]);
                    break;
                case 4:
                    points[i]=CalculateBezierPoint(i*1f/(points.Length-1), p[0], p[1], p[2], p[3]);
                    break;
                default:
                    throw new Exception("bug");
            }
        }

        return points;
    }

    static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1)
    {
        return (1 - t) * p0 + t * p1;
    }
    static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;       // (1-t)^2 * P0
        p += 2 * u * t * p1;       // 2(1-t)t * P1
        p += tt * p2;              // t^2 * P2

        return p;
    }
    static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;           // (1-t)^3 * P0
        p += 3 * uu * t * p1;           // 3(1-t)^2 * t * P1
        p += 3 * u * tt * p2;           // 3(1-t) * t^2 * P2
        p += ttt * p3;                  // t^3 * P3

        return p;
    }
}

}
