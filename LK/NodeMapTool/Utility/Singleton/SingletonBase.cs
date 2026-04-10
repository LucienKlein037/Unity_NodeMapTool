namespace LucienKlein
{

using UnityEngine;


public class SingletonBase<T>   where T:class,new()  
{
    private static T instance=new T();
    public static T Instance { get { return instance; } }

    



    






    protected void print(object o)
    {
        Debug.Log(o);
    }


}

}
