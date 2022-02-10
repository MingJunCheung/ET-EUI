using System;
using System.IO;
using System.Security.Cryptography;

namespace ET
{
	public static class MD5Helper
	{
		public static string FileMD5(string filePath)
		{
			byte[] retVal;
            using (FileStream file = new FileStream(filePath, FileMode.Open))
            {
	            MD5 md5 = MD5.Create();
				retVal = md5.ComputeHash(file);
			}
			return retVal.ToHex("x2");
		}

		public static string StringMD5(string Str)
        {
			MD5 md5 = MD5.Create();
            byte[] result = System.Text.Encoding.Default.GetBytes(Str);
			byte[] outPut = md5.ComputeHash(result);
			return BitConverter.ToString(outPut).Replace("-", "");
        }
	}
}
