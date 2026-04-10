namespace LucienKlein
{
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LKMath 
{
    //************************************************************************************************************************/
    

    //用的不多,似乎已经被sfloat取代了
    public static int Floor(float value)
    {
        int intPart = (int)value;
        float decimalPart = value - intPart;

        if (decimalPart >= 0.99f)
            return intPart + 1;
        else
            return intPart;
    }






    //给概率 抽样 返回抽取的索引
    public static List<int> Sample(List<float> listProbability,int count=1,bool isCanRepeat=false)
    {
        if (listProbability == null || listProbability.Count == 0)
            throw new ArgumentException("Probability list cannot be null or empty");

        if (count <= 0 || count > listProbability.Count)
            throw new ArgumentOutOfRangeException("Invalid count value");

        //最终结果
        List<int> result = new();
        //生命索引list
        List<int> listIndex = listProbability.Count.GetIndexList();
        //当前选中的第i个数用于延迟删除
        int nowChooseI=-1;

        float totalP=0;
        //获取总共的概率 防止总概率不等于1的情况
        for (int n = 0; n < listProbability.Count; n++)
        {
            float element = listProbability[n];
            totalP+=element;
        }
        if (Math.Abs(totalP - 1f) > 0.0001f)
        {
            //概率修正
            for (int n = 0; n < listProbability.Count; n++)
            {
                float element = listProbability[n];
                element=element/totalP;
                listProbability[n]=element;
            }
        }

        //循环抽取
        while (result.Count<count)
        {


            float nowP = 0;
            float r = 0;
            r=LKRandom.F1;

            for (int i = 0; i < listIndex.Count; i++)
            {
                nowP+=listProbability[i];
                if (nowP>=r)
                {
                    result.Add(listIndex[i]);
                    //如果不能重复,选中后要剔除掉
                    if (!isCanRepeat)
                    {
                        nowChooseI=i;
                    }
                    break;
                }
            }

            if (nowChooseI!=-1)
            {
                listIndex.RemoveAt(nowChooseI);
                listProbability.RemoveAt(nowChooseI);
                nowChooseI=-1;
            }





        }



        return result;


    }


    //等几率的抽取.返回索引
    public static List<int> SimpleSample(int poolUnitQuantity,  int count,bool isCanRepeat = false)
    {
        List<float> listProbability = new();
        for (int i = 0; i < poolUnitQuantity; i++)
        {
            listProbability.Add(1f/poolUnitQuantity);
        }

        return Sample(listProbability, count, isCanRepeat);
    }





}

}
