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
    public partial class Form1 : Form
    {

        public static byte[] key, iv;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Encriptionボタン押下時処理（暗号化処理）
        /// </summary>
        /// <param name="sender">メッセージの送信元</param>
        /// <param name="e">メッセージ</param>
        private void button1_Click(object sender, EventArgs e)
        {

            //ファイルを暗号化する
            EncryptFile(@"C:\Users\lenovo\Desktop\暗号化ファイル\test.txt", 
                @"C:\Users\lenovo\Desktop\暗号化ファイル\test.enc", out key, out iv);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
        }

        /// <summary>
        /// ファイルを暗号化する
        /// </summary>
        /// <param name="sourceFile">暗号化するファイルパス</param>
        /// <param name="destFile">暗号化されたデータを保存するファイルパス</param>
        /// <param name="key">暗号化に使用した共有キー</param>
        /// <param name="iv">暗号化に使用した初期化ベクタ</param>
        public static void EncryptFile(
            string sourceFile, string destFile, out byte[] key, out byte[] iv)
        {
            //RijndaelManagedオブジェクトを作成
            System.Security.Cryptography.RijndaelManaged rijndael =
                new System.Security.Cryptography.RijndaelManaged();

            //共有キーと初期化ベクタを作成
            //Key、IVプロパティがnullの時に呼びだすと、自動的に作成される
            //自分で作成するときは、GenerateKey、GenerateIVメソッドを使う
            key = rijndael.Key;
            iv = rijndael.IV;

            //暗号化されたファイルを書き出すためのFileStream
            System.IO.FileStream outFs = new System.IO.FileStream(
                destFile, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            //対称暗号化オブジェクトの作成
            System.Security.Cryptography.ICryptoTransform encryptor =
                rijndael.CreateEncryptor();
            //暗号化されたデータを書き出すためのCryptoStreamの作成
            System.Security.Cryptography.CryptoStream cryptStrm =
                new System.Security.Cryptography.CryptoStream(
                    outFs, encryptor,
                    System.Security.Cryptography.CryptoStreamMode.Write);

            //暗号化されたデータを書き出す
            System.IO.FileStream inFs = new System.IO.FileStream(
                sourceFile, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] bs = new byte[1024];
            int readLen;
            while ((readLen = inFs.Read(bs, 0, bs.Length)) > 0)
            {
                cryptStrm.Write(bs, 0, readLen);
            }

            //閉じる
            inFs.Close();
            cryptStrm.Close();
            encryptor.Dispose();
            outFs.Close();
        }
    }
}
