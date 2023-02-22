using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PropertyMataaz.Utilities.Abstrctions;

namespace PropertyMataaz.Utilities
{
    public class UtilityMethods :IUtilityMethods
    {
        public string RandomCode(int size)
        {
            try
            {
                char[] chars = new char[62];
                chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
                byte[] data = new byte[1];
                using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
                {
                    crypto.GetNonZeroBytes(data);
                    data = new byte[size];
                    crypto.GetNonZeroBytes(data);

                }
                StringBuilder Result = new StringBuilder(size);
                foreach (byte b in data)
                {
                    Result.Append(chars[b % (chars.Length)]);
                }
                return Result.ToString();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public string FormattedDate(DateTime thisDate)
        {
            try
            {
                if (thisDate != DateTime.MaxValue && thisDate != DateTime.MinValue)
                {
                    return thisDate.ToShortDateString() + ":" + thisDate.ToShortTimeString();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return "";
        }

        public string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid().ToString().Substring(5, 5) + Path.GetExtension(fileName);
        }
    }
}
