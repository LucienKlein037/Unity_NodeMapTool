using UnityEngine;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace LucienKlein
{


    public static class CopyMachine
    {
        /// <summary>
        /// 通过二进制或反射进行深拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(T obj) where T : class
        {
            if (obj is ICloneable ic)
            {
                return ic.Clone() as T;
            }

            object[] attributes = obj.GetType().GetCustomAttributes(false);
            if ((obj is ISerializable) || (Attribute.IsDefined(obj.GetType(), typeof(SerializableAttribute))))
            {
                return ByBinary();
            }
            else
            {
                return ByReflection();
            }

            T ByBinary()
            {
                object newObj;
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, obj);
                    ms.Seek(0, SeekOrigin.Begin);
                    newObj = bf.Deserialize(ms);
                    ms.Close();
                }
                return newObj as T;
            }
            T ByReflection()
            {
                return reflectionGetObject(obj) as T;

                object reflectionGetObject(object o)
                {
                    if (o==null)
                    {
                        return null;
                    }
                    if (o.GetType().IsValueType||o is string)
                    {
                        return o;
                    }
                    if (o is IList)
                    {
                        //Type listType = typeof(List<>).MakeGenericType(o.GetType().GetGenericArguments()[0]);
                        IList newList = Activator.CreateInstance(o.GetType()) as IList;
                        foreach (var item in o as IList)
                        {
                            newList.Add(item);
                        }
                        return newList;
                    }
                    if (o is IDictionary)
                    {
                        IDictionary newDic = Activator.CreateInstance(o.GetType()) as IDictionary;
                        foreach (var item in (o as IDictionary).Keys)
                        {
                            newDic.Add(item, (o as IDictionary)[item]);
                        }
                        return newDic;
                    }
                    if (o is Enum)
                    {
                        Debug.Log("is Enum");
                    }
                    if (o is Sprite)
                    {
                        return o;
                    }
                    if (o is ScriptableObject)
                    {
                        return o;
                    }
                    if (o is Delegate)
                    {
                        return o;
                    }

                    object newObj;
                    newObj = Activator.CreateInstance(o.GetType());
                    //foreach (var item in o.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance ))
                    foreach (var item in o.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))//Static�о���������Ҫȥ����
                    {

                        //Debug.Log("item" + item.Name);


                        item.SetValue(newObj, reflectionGetObject(item.GetValue(o)));

                    }
                    return newObj;

                }
            }
        }

    }
}
