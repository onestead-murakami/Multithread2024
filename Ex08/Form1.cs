using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Ex08
{
    public partial class Form1 : Form
    {

        // ファイルダウンロード先のURL
        private const string URL = "https://oc6mntdl2pr77x7brzdc6myxqu0tmdde.lambda-url.ap-northeast-1.on.aws/";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // ダウンロードボタンを非活性状態に変更
            button1.Enabled = false;


            for (int i = 0; i < 3; i++)
            {
                // Webリクエストを作成
                var request = WebRequest.Create(URL);

                // Webリクエストを実行
                var response = request.GetResponse();

                // ファイルダウンロードを実行
                Download(response);

            }

            // ダウンロードボタンを活性状態に変更
            button1.Enabled = true;

        }

        /// <summary>Webレスポンスに含まれるデータをテキストファイルに保存</summary>
        /// <param name="response">Webレスポンス</param>
        private static void Download(WebResponse response)
        {
            // レスポンスヘッダーからファイル名を取得
            string id = response.Headers["Content-Disposition"].Split('=')[1].Replace("\"", "");

            // レスポンスボディーからのデータ取得を開始
            using (var stream = response.GetResponseStream())
            {
                // 保存先のファイルを作成
                using (var writer = new StreamWriter(id, false))
                {
                    // 1KBのバイナリ領域を確保
                    var buf = new byte[1024];

                    // すべてのデータを1KBずつファイルへ書き込む
                    int count = stream.Read(buf, 0, buf.Length);
                    while (count > 0)
                    {
                        writer.Write(Encoding.UTF8.GetString(buf, 0, count));
                        count = stream.Read(buf, 0, buf.Length);
                    }
                    writer.WriteLine();
                }
            }
        }
    }
}
