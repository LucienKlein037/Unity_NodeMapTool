using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LucienKlein
{

    //清除浮点数误差的float
    public struct sfloat
    {
        private float _value;

        public sfloat(float value)
        {
            _value = value.CleanFloat();
        }

        public static implicit operator float(sfloat v) => v._value;
        public static implicit operator sfloat(float v) => new sfloat(v);

        public override string ToString() => _value.ToString("0.#####");

        //private static float CleanFloat(float value, float epsilon = 0.00001f)
        //{
        //    float rounded = Mathf.Round(value * 100000f) / 100000f;
        //    float rounded2 = Mathf.Round(rounded * 100f) / 100f;

        //    if (Mathf.Abs(value - rounded2) < epsilon)
        //        return rounded2;

        //    return rounded;
        //}
    }

}
