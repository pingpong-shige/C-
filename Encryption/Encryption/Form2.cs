using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Encryption
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Decryptionボタン押下字処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //暗号化したファイルを復号化する
            DecryptFile(@"C:\Users\lenovo\Desktop\暗号化ファイル\test.enc",
                @"C:\Users\lenovo\Desktop\暗号化ファイル\test2.txt", Form1.key, Form1.iv);
        }

        /// <summary>
        /// ファイルを復号化する
        /// </summary>
        /// <param name="sourceFile">復号化するファイルパス</param>
        /// <param name="destFile">復号化されたデータを保存するファイルパス</param>
        /// <param name="key">暗号化に使用した共有キー</param>
        /// <param name="iv">暗号化に使用した初期化ベクタ</param>
        public static void DecryptFile(
            string sourceFile, string destFile, byte[] key, byte[] iv)
        {
            //RijndaelManagedオブジェクトの作成
            System.Security.Cryptography.RijndaelManaged rijndael =
                new System.Security.Cryptography.RijndaelManaged();

            //共有キーと初期化ベクタを設定
            rijndael.Key = key;
            rijndael.IV = iv;

            //暗号化されたファイルを読み込むためのFileStream
            System.IO.FileStream inFs = new System.IO.FileStream(
                sourceFile, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            //対称復号化オブジェクトの作成
            System.Security.Cryptography.ICryptoTransform decryptor =
                rijndael.CreateDecryptor();
            //暗号化されたデータを読み込むためのCryptoStreamの作成
            System.Security.Cryptography.CryptoStream cryptStrm =
                new System.Security.Cryptography.CryptoStream(
                    inFs, decryptor,
                    System.Security.Cryptography.CryptoStreamMode.Read);

            //復号化されたデータを書き出す
            System.IO.FileStream outFs = new System.IO.FileStream(
                destFile, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            byte[] bs = new byte[1024];
            int readLen;
            //復号化に失敗すると例外CryptographicExceptionが発生
            while ((readLen = cryptStrm.Read(bs, 0, bs.Length)) > 0)
            {
                outFs.Write(bs, 0, readLen);
            }

            //閉じる
            outFs.Close();
            cryptStrm.Close();
            decryptor.Dispose();
            inFs.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
