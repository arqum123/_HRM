using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;
namespace HRM.Core.Helper
{
    public class QSDecryption
    {
        public Dictionary<string, string> QS = new Dictionary<string, string>();
        public QSDecryption(string encQS)
        {
            string decQS = string.Empty;
            decQS = new EncryptedQueryString().Decrypt(encQS);
            string[] keyvalues = decQS.Split('&');
            string[] values = null;
            try
            {
                foreach (string keyvalue in keyvalues)
                {
                    values = keyvalue.Split('=');
                    QS.Add(values[0], values.Length == 2 ? values[1] : string.Empty);
                }
            }
            catch (Exception)
            {
                throw new FormatException("Querystirng is not in valid format");
            }
        }
    }
}