using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaProgram
{
    public abstract class SingletonService<T> where T : new()
    {
        private static T _instance;
        public static T GetInstance()
        {
            if (_instance == null)
            {
                _instance = new T();
            }

            return _instance;
        }

        public abstract void ResetData();
    }
}
