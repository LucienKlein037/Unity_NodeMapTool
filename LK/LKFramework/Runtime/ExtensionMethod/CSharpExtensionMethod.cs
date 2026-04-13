using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LucienKlein
{

    public static class CSharpExtensionMethod
    {
        #region Dictionary

        //字典转List(不保证顺序)
        public static List<T> ToList<T>(this IDictionary dic) where T : class
        {
            List<T> list = new List<T>();
            foreach (var key in dic.Keys)
            {
                list.Add(dic[key] as T);
            }
            return list;
        }
        //转为易看的字符串
        public static string ToClearString(this IDictionary dic)
        {
            StringBuilder sb = new();
            sb.Append("{");
            foreach (var key in dic.Keys)
            {
                if (dic[key] is IDictionary id)
                {
                    sb.Append($" ({key}--{id.ToClearString()}) \n");
                }
                else
                {

                    sb.Append($" ({key}--{dic[key].ToString()}) ");
                }
            }
            sb.Append("}");
            return sb.ToString();
        }
        #endregion

        #region List
        //根据特定抽样法获取对应index列表
        public static List<int> SampleGetListIndexs(this IList list, e_makeType type, int sampleCount)
        {
            //***************************************************************************************************/判断
            if (list.Count==0)
            {

                throw new Exception("bug");
                //return null;
            }

            //***************************************************************************************************/
            switch (type)
            {
                case e_makeType.NonRepeatedRandom:
                    return NonRepeatedRandom(list, sampleCount);
                default:

                    throw new Exception("检测不到对应类型的处理方式");
            }
            //***************************************************************************************************/封装
            List<int> NonRepeatedRandom(IList list, int sampleCount)
            {
                //记录循环次数,保护线程避免死循环
                int times = 0;
                //----------------------------------------------------------------------------------------------------运行许可判定
                if (sampleCount <= 0)
                {
                    throw new Exception("sampleCount:"+sampleCount+";list.Count:"+list.Count);
                }
                sampleCount=Mathf.Min(sampleCount, list.Count);

                //----------------------------------------------------------------------------------------------------
                List<int> SelectedIndexs = new List<int>();
                while (SelectedIndexs.Count < sampleCount && times < 1000)
                {
                    int r = Random.Range(0, list.Count);
                    if (SelectedIndexs.IndexOf(r) == -1)
                    {
                        SelectedIndexs.Add(r);
                    }

                    times++;
                    if (times>900)
                    {
                        Debug.LogWarning(times+"----times");
                    }
                }




                return SelectedIndexs;
            }

        }

        public static void Add<T>(this List<T> list, List<T> newlist)
        {
            for (int i = 0; i < newlist.Count; i++)
            {
                list.Add(newlist[i]);
            }

        }

        public static T Last<T>(this List<T> list)
        {
            return list[list.Count-1];
        }

        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = LKRandom.IntRange(0, n); // 随机索引 [0, n]
                (list[k], list[n]) = (list[n], list[k]); // 交换位置
            }
        }


        public static T GetRandomElement<T>(this List<T> list)
        {
            return list[LKRandom.ListIndex(list.Count)];
        }

        #endregion

        #region Array
        public static T Last<T>(this T[] array)
        {
            // 检查数组是否为空
            if (array == null || array.Length == 0)
            {
                throw new InvalidOperationException("Array is empty or null.");
            }

            // 返回数组的最后一个元素
            return array[array.Length - 1];
        }
        #endregion


        #region int
        public static List<int> GetIndexList(this int count)
        {
            if (count<=0)
            {
                return new List<int>();
            }
            List<int> list = new List<int>();
            for (int i = 0; i < count; i++)
            {
                list.Add(i);
            }
            return list;
        }
        #endregion

        #region float

        public static bool NearEqual(this float f1, float f2)
        {
            return Math.Abs(f1 - f2) < 1e-6;
        }
        public static bool NearEqual(this float f1, int f2)
        {
            return Math.Abs(f1 - f2) < 1e-6;
        }
        public static bool JudgeProbability1(this float probability)
        {
            if (probability>=1)
            {
                return true;
            }
            if (probability<=0)
            {
                return false;
            }
            return probability>= LKRandom.F1;
        }
        public static bool JudgeProbability100(this float probability)
        {
            if (probability>=100)
            {
                return true;
            }
            if (probability<=0)
            {
                return false;
            }
            return probability> LKRandom.F100;
        }

        public static float CleanFloat(this float value, float epsilon = 0.00001f)
        {
            // 自动纠正接近整数或一位小数的数
            float rounded = Mathf.Round(value * 100000f) / 100000f;

            // 如果距离最近的整数或小数很接近，就取它
            float rounded2 = Mathf.Round(rounded * 100f) / 100f;

            if (Mathf.Abs(value - rounded2) < epsilon)
                return rounded2;

            return rounded; // 保留到 5 位有效数字
        }

        #endregion

        #region string

        public static string CamelToLanguage(this string str)
        {
            //str="bestName"
            //char[] chars = str.ToCharArray();
            if (str.Length>10000)
            {
                throw new Exception("字符串长度太长");
            }
            int k = 0;
            int i = 0;
            while (i < str.Length)
            {
                k++;
                if (k>1000)
                {
                    throw new Exception("bug");
                }
                if (i==0)
                {
                    i++;
                    continue;

                }
                char c = str[i];
                if (char.IsLetter(c)&&char.IsUpper(c)&&str.Length-1>i&&char.IsLower(str[i+1]))
                {
                    str= str.Remove(i, 1);
                    str= str.Insert(i, " "+char.ToLower(c));
                    i++;
                }
                i++;

            }
            return str;
        }
        public static string LanguageToCamel(this string str)
        {
            //str="bestName"
            //char[] chars = str.ToCharArray();

            int k = 0;
            if (str.Length>10000)
            {
                throw new Exception("字符串长度太长");
            }

            int i = 0;
            while (i < str.Length)
            {
                k++;
                if (k>1000)
                {

                    throw new Exception("bug");
                }
                if (i==0)
                {
                    i++;
                    continue;
                }
                char c = str[i];
                if (c==' '&&
                    str.Length-1>i&&char.IsLetter(str[i+1]))
                {
                    char upperChar = char.ToUpper(str[i+1]);
                    str= str.Remove(i+1, 1);
                    str= str.Remove(i, 1);
                    str= str.Insert(i, upperChar.ToString());
                }

                i++;
            }

            return str;
        }

        //指定图片下的某个索引<sprite="spriteicons" index=98>与<sprite="Default Sprite Asset" index=6>
        //指定图片下的某个名称<sprite="Default Sprite Asset" name="Unity Logo">与<sprite="spriteicons" name="spriteicons_9">
        //指定图片的颜色<sprite="spriteicons" index=98 color=#55CF4E>和含有透明度<sprite="Default Sprite Asset" index=6 color=#F14FE264>
        //图片的颜色随组件控制<sprite="spriteicons" name="spriteicons_9" tint=1 color=#FFFFFFFF>
        //图片的颜色不跟随随组件控制<sprite="spriteicons" name="spriteicons_9" color=#FFFFFF80 tint=0>
        //图片的颜色不跟随随组件控制<sprite="spriteicons" name="spriteicons_13">
        //<sprite="DefaultSpriteAsset" name="MHP" tint=1>
        /*
         * 
         *  <sprite="DefaultSpriteAsset" name="MHP" tint=1>
            <sprite="DefaultSpriteAsset" name="ATK" tint=1>
            <sprite="DefaultSpriteAsset" name="MEG" tint=1>
            <sprite="DefaultSpriteAsset" name="SPD" tint=1>
            <sprite="DefaultSpriteAsset" name="ARP" tint=1>
            <sprite="DefaultSpriteAsset" name="DEF" tint=1>
            <sprite="DefaultSpriteAsset" name="CRI" tint=1>
            <sprite="DefaultSpriteAsset" name="CHD" tint=1>

         */
        public static string ToSpriteAssetString(this string str)
        {
            str =   $"<sprite=\"DefaultSpriteAsset\" name=\"{str}\" tint=1>";


            return str;
        }
        public static string ToColorString(this string str, Color color)
        {
            str =   $"<color={color.ColorToHex()}>{str}</color>";


            return str;
        }

        #endregion



        #region Enum
        public static T Parse<T>(this System.Enum @enum, string str) where T : System.Enum
        {
            return (T)Enum.Parse(typeof(T), str);
        }


        #endregion



    }
    //*******************************************************************************************************************************************************************************************************/
    public enum e_makeType
    {
        NonRepeatedRandom,
    }
}
