using System;


namespace botaweb.StructureMapGenerics
{
    public class HelloWorldPrinter : IHelloWorld
    {
        public string PrintHelloWorld()
        {
            return "HELLO WORLD";          
        }
    }
}
