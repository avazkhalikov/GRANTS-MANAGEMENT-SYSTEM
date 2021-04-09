using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace BOTACORE.CORE.Domain
{
    public class CryptBase
    {
        private static byte[] key ={
                                      93,
                                      252,
                                      76,
                                      113,
                                      209,
                                      29,
                                      253,
                                      168
                                  };
        private static byte[] IV = { 
                                       172,
                                       44,
                                       172,
                                       193,
                                       7,
                                       48,
                                       131,
                                       165
                                   };

        public string Decrypt(string stringToDecrypt)
        {
            byte[] inputByteArray=new byte[stringToDecrypt.Length+1];
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray,0,inputByteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string Encrypt(string stringToEncrypt)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms,des.CreateEncryptor(key,IV),CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
