using StructureMap;
using System;
using System.Configuration;

namespace BOTACORE.CORE.Impl
{
   
    public class Configuration : IConfiguration
    {


        public string AdminSiteURL
        {
            get { return "<br /> http://adminurl"; } 
            //get { return getAppSetting(typeof(string), "AdminSiteURL").ToString(); }
        }





        private static object getAppSetting(Type expectedType, string key)
        {
            string value = ConfigurationManager.AppSettings.Get(key);
            if (value == null)
            {
                Log.Fatal("Configuration.cs", string.Format("AppSetting: {0} is not configured", key));
                throw new Exception(string.Format("AppSetting: {0} is not configured.", key));
            }

            try
            {
                if (expectedType.Equals(typeof(int)))
                {
                    return int.Parse(value);
                }

                if (expectedType.Equals(typeof(string)))
                {
                    return value;
                }

                throw new Exception("Type not supported.");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Config key:{0} was expected to be of type {1} but was not.", key, expectedType),
                                    ex);
            }
        }
    }
}
