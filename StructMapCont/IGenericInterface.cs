using System;


namespace StructMapCont
{
    public interface IGenericInterface<T>
    {
        String ReturnString(T value);
    }
}
