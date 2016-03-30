using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleXOREncryption
{    
    public static class EncryptorDecryptor
    {
        public static int key = 129;

        public static string EncryptDecrypt(string textToEncrypt)
        {            
            StringBuilder inSb = new StringBuilder(textToEncrypt);
            StringBuilder outSb = new StringBuilder(textToEncrypt.Length);
            char c;
            for (int i = 0; i < textToEncrypt.Length; i++)
            {
                c = inSb[i];
                c = (char)(c ^ key);
                outSb.Append(c);
            }
            return outSb.ToString();
        }

		public static int SafeIntKey() {
			Random rnd = new Random();
			return rnd.Next(-1000, 1000);
		}
    }
}
