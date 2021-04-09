using System;
using System.Collections.Generic;
using System.Text;

namespace StructMapCont
{
    public class GenericClass<T> : IGenericInterface<T>
    {
        public String ReturnString(T value)
        {
            return value.ToString();
        }
    }
}