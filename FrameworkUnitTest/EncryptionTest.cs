using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Security.Cryptography;

namespace FrameworkUnitTest
{
    /// <summary>
    /// EncryptionTest 的摘要说明
    /// </summary>
    [TestClass]
    public class EncryptionTest
    {
        public EncryptionTest()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void DesTest()
        {
            //
            // TODO:  在此处添加测试逻辑
            //
            string Key = "EW2sdfkj";
            string Algorithm = "DES";
            string Plaintext = "<uid>123</uid><time>20200630103535</time>";
            string Ciphertext = "05D29594DF61FA5D0432A3EC546FF382F6B97338CFDD19B241F1C9F0A3F51DB93EBB66A77CEA09A8750D74CB32C089EA";

            //萍乡二维码解密
            //密文转16进字符串，然后des解密
            //var decryptText = DesDecrypt(HexStrToByte(Ciphertext), Encoding.ASCII.GetBytes(Key));
            var decryptText = DesDecrypt(Ciphertext, Key);
            Assert.AreEqual(decryptText, Plaintext,false);
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="pToDecrypt">待解密的字符串</param>
        /// <param name="sKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        string DesDecrypt(string pToDecrypt, string sKey)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                des.Key = ASCIIEncoding.UTF8.GetBytes(sKey);
                des.IV = ASCIIEncoding.UTF8.GetBytes(sKey);
                //des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.Default.GetString(ms.ToArray());
            }
            catch(Exception e)
            {
                return "";
            }
        }


        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="pToDecrypt">待解密的字符串</param>
        /// <param name="sKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        string DesDecrypt(byte[] pToDecrypt, byte[] sKey)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = pToDecrypt;
                //new byte[pToDecrypt.Length / 2];
                //for (int x = 0; x < pToDecrypt.Length / 2; x++)
                //{
                //    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                //    inputByteArray[x] = (byte)i;
                //}
                des.Key = sKey;  //ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = sKey;  //ASCIIEncoding.ASCII.GetBytes(sKey);
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.Default.GetString(ms.ToArray());
            }
            catch(Exception e)
            {
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hexText"></param>
        /// <returns></returns>
        byte[] HexStrToByte(string hexText)
        {
            byte[] hex = new byte[2];
            try
            {
                if (string.IsNullOrEmpty(hexText) || hexText.Length < 2) return hex;
                int len = hexText.Length;
                hex = new byte[len / 2];

                for (int i = 0; i < len / 2; i++)
                {
                    hex[i] = Convert.ToByte(hexText.Substring(i * 2, 2), 16);
                }

            }
            catch { }

            return hex;
        }
    }
}
