using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
namespace TroveToolFree
{
    public  class CL_UTIL
    {
        public XmlDocument _XML = new XmlDocument();
        public string _API_VERSION_ { get; set; }  // Versão DLL Online [No site]

        public bool Pesquisar_Nova_Atualizao(string _MinhaVersao)
        {
            if (this._API_VERSION_ != _MinhaVersao) { return true; }
            //if (BaseTrove.BASE_TROVE_CORE_SOURCE._API_VERSION_CORE_SOURCE_ != _MinhaVersao) { return true; }
            return false;
        }
        public void Carregar_Informacao_XML()
        {
            _XML.Load(BaseTrove.BASE_TROVE_CORE_SOURCE._API_URL_ATUALIZA_CORE_SOURCE_);
            XmlNodeList _l = _XML.DocumentElement.SelectNodes("/TROVE");
            try
            {
                foreach (XmlNode item in _l)
                {
                    if (item.SelectSingleNode("_STATUS_API_").InnerText != "1") //{ this._STATUS_API_ = false; }
                    { BaseTrove.BASE_TROVE_CORE_SOURCE._API_STATUS_CORE_SOURCE_ = false; }
                    
                    else //{ this._STATUS_API_ = true; }
                    { BaseTrove.BASE_TROVE_CORE_SOURCE._API_STATUS_CORE_SOURCE_ = true; }
                    this._API_VERSION_ = item.SelectSingleNode("_API_VERSION_").InnerText;
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                //throw;
            }

        }
        public static string _Get_MD5_FROM_Trove_(string file)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 8192);
            md5.ComputeHash(stream);
            stream.Close();
            byte[] hash = md5.Hash;
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(string.Format("{0:X2}", b));
            }
            return sb.ToString();
        }
    }
    public static class Hash_Codifica_Decodifica
    {
        /// <summary>
        /// Hash_Codifica_Decodifica
        /// </summary>

        public static string KeyHash = "!@#";

        #region Encrypt
        public static string Encrypt(string clearText)
        {

            try
            {
                string EncryptionKey = KeyHash;

                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
				    0x49,
				    0x76,
				    0x61,
				    0x6e,
				    0x20,
				    0x4d,
				    0x65,
				    0x64,
				    0x76,
				    0x65,
				    0x64,
				    0x65,
				    0x76
			    });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return clearText;
            }
            catch (Exception ex)
            {
                // Return Nothing
                return "MECANISMO_INVALIDO";
            }

        }
        #endregion

        #region Decrypt

        public static string Decrypt(string cipherText)
        {
            try
            {
                string EncryptionKey = KeyHash;
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
				    0x49,
				    0x76,
				    0x61,
				    0x6e,
				    0x20,
				    0x4d,
				    0x65,
				    0x64,
				    0x76,
				    0x65,
				    0x64,
				    0x65,
				    0x76
			    });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch (Exception ex)
            {
                //  Return Nothing
                return "MECANISMO_INVALIDO";
            }

        }

        #endregion
    }
}
