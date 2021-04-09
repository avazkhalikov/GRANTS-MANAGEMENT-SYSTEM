using System;
using System.Collections.Generic;
using System.Text;

namespace StructMapCont
{
    public class GenericClass2 : IGenericInterface<String>
    {
        public String ReturnString(String value)
        {
            return value + "2";
        }
    }
}