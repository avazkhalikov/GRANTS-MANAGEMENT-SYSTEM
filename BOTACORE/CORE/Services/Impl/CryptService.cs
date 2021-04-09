using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
using System.Web;

namespace BOTACORE.CORE.Services.Impl
{
    public class CryptService:ICryptService
    {
        public Dictionary<string, string> DecryptQueryString(string EncryptedQueryString)
        {
            bool err = false;
            string err_mess = "";
            string decoded = "";
            CryptBase cb=new CryptBase();
            Dictionary<string,string> result=new Dictionary<string,string>();
            result.Add("error","0");
            try
            {
                decoded = cb.Decrypt(HttpUtility.UrlDecode(EncryptedQueryString));
            }
            catch (Exception ex)
            {
                err=true;
                err_mess=ex.ToString();
            }
            if (err==false)
            {
                string[] mas=null;
                mas=decoded.Split("&".ToCharArray());
                try
                {
                    foreach(string elem in mas)
                    {
                        result.Add(elem.Substring(0,elem.IndexOf("=")),
                            string.Copy(elem.Substring(elem.IndexOf("=")+1)));
                    }
                }
                catch (Exception ex)
                {//esli vyshla oshibka, zna4it parametry ne byli peredany ili QueryString pytalis vru4nuyu redaktirovat
                    err=true;
                    err_mess=ex.ToString();
                }
            }

            if (err==true)
            {
                result["error"]="1";
                result.Add("error_details",err_mess);
            }
            return result;
        }

        public string EncryptQueryString(string QueryString)
        {
            string encoded = "";
            CryptBase cb = new CryptBase();
            encoded = HttpUtility.UrlDecode(cb.Encrypt(QueryString));
            return encoded;
        }
    }
}
