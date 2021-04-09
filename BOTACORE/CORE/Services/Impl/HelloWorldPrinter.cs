using System;


namespace BOTACORE.CORE.Impl
{
    public class HelloWorldPrinter : IHelloWorld
    {

        public string HelloWorld = "<br /> HELLO WORLD from CORE";
        
        public string PrintHelloWorld()
        {
            return HelloWorld;        
        }
    }
}
