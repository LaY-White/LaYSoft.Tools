using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaYSoft.BaseCode
{
    public class MyFactory<T> where T : new()
    {
        private static Hashtable _ClassDic = new Hashtable();

        public static T CreateCurSystemClass()
        {
            Type t = typeof(T);
            T target = default(T);

            lock (_ClassDic.SyncRoot)
            {
                if (_ClassDic.ContainsKey(t.FullName))
                {
                    target = (T)_ClassDic[t.FullName];
                }
                else
                {
                    target = new T();
                    _ClassDic[t.FullName] = target;
                }
            }
            return target;
        }
    }
}
