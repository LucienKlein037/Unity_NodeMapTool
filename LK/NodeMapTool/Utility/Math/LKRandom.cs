namespace LucienKlein
{
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LKRandom
{
    //************************************************************************************************************************/


    public static bool B
    {
        get
        {
            return 0==Random.Range(0, 2);
        }
    }
    public static int I99
    {
        get
        {
            return IntRange(0, 99);
        }
    }
    public static float F1
    {
        get
        {
            return Random.Range(0f, 1f);
        }
    }
    public static float F100
    {
        get
        {
            return Random.Range(0f, 100f);
        }
    }

    /// <summary>
    /// ���ұ�
    /// </summary>
    public static int IntRange(int min, int max)
    {
        if (min>max)
        {
            min= max;
        }
        return Random.Range(min, max+1);
    }
    public static float FloatRange(float min, float max)
    {
        if (min>max)
        {
            return max;
        }
        if (max<min)
        {
            return min;
        }
        return Random.Range(min, max);
    }
    public static int ListIndex(int count)
    {
        if (count<=0)
        {
            Debug.LogWarning("����:��Ӧ��");
            return -1;
        }
        return Random.Range(0, count);

    }

    //����һ������list,���ո���list��ȥ�����ȡ,���ض�Ӧ������
    public static int ProbabilityListRandom(List<float> probabilities)
    {
        float sum = 0;
        foreach (var prob in probabilities)
        {
            sum += prob;
        }

        if (sum!=1)
        {
            Debug.LogWarning("����:���ڲ�Ӧ���ֵ����");
        }
        if (sum <= 0 || sum > 1)
        {
            throw new Exception("bug");
        }

        float f = F1;
        float cumulative = 0;

        for (int i = 0; i < probabilities.Count; i++)
        {
            cumulative += probabilities[i];
            if (f < cumulative)
            {
                return i;
            }
        }

        return probabilities.Count - 1; // �����ϲ�Ӧ�õ�����
    }


    //����С������и��ʽ��н�λ������. ��1.8f��80%����ȡ��Ϊ2
    public static int RandomRound(float f)
    {
        int result = (int)f;

        float rest = f-result;
        if (rest>0)
        {
            if (rest>F1)
            {
                result++;
            }
        }

        return result;
    }


    public static int RandomRound(double f)
    {
        int result = (int)f;

        double rest = f-result;
        if (rest>F1)
        {
            result++;
        }

        return result;
    }

    //����������������list
    public static List<int> RandomIndexList(int count)
    {
        // ��ʼ���б� [0, 1, 2, ..., count-1]
        List<int> list = new();
        for (int i = 0; i < count; i++)
        {
            list.Add(i);
        }

        // ʹ�� Fisher-Yates ϴ���㷨
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = ListIndex(i + 1);
            // ����Ԫ��
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        return list;


    }
    //�����list
    public static List<T> OutOfOrderList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = ListIndex(i + 1);
            // ����Ԫ��
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        return list;
    }



    public static void SetSeed()
    {
        SetSeed(Random.Range(0, int.MaxValue));
    }
    public static void SetSeed(int seed)
    {
        Random.InitState(seed);
        Debug.Log("Seed:"+seed);

    }
}

}
