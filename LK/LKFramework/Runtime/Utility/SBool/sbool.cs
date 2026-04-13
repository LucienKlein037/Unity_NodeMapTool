
namespace LucienKlein
{
    //用加减来表达bool,一些状态判定比较好用,比如一个角色进入水或草都能隐身,进入时+1 退出时-1.就能表示当前是否隐身,如果退出就设置false,那么在进入了水又进入草又退出草时则会忽略在水中也能隐身.
    public struct sbool
    {
        private sbyte n;

        public sbool(sbyte value) => n = value;

        // 从 sbyte 转换
        public static implicit operator sbool(sbyte value) => new sbool(value);

        // 从 bool 转换 .太危险
        //public static implicit operator sbool(bool value) => new sbool((sbyte)(value ? 1 : 0));

        // 转 bool
        public static implicit operator bool(sbool rb) => rb.n > 0;

        // 转 sbyte
        public static implicit operator sbyte(sbool rb) => rb.n;


        public static sbool operator ++(sbool value)
        {
            value.n++;
            return value;
        }

        public static sbool operator --(sbool value)
        {
            value.n--;
            return value;
        }

        public static sbool operator +(sbool left, sbyte right)
        {
            return new sbool((sbyte)(left.n + right));
        }

        public static sbool operator -(sbool left, sbyte right)
        {
            return new sbool((sbyte)(left.n - right));
        }

        public override string ToString() => $"n={n}, bool={n > 0}";
    }

}
