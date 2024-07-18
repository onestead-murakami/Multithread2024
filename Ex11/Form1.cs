using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex11
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

            // プログレスバーの目盛り幅を設定

            // WEBAPI通信によるファイルダウンロード

            // ダウンロード完了を通知

            // プログレスバーを0に変更

            // ダウンロードボタンを活性状態に変更
        }

        /// <summary>画面再描画（メインスレッドにメッセージ送信）</summary>
        private void RefreshForm()
        {
            // プログレスバーの目盛り進める
            progressBar1.PerformStep();
        }

        /// <summary>// WEBAPI通信によるファイルダウンロード（リクエスト→レスポンス→ファイル保存→進捗率変更）</summary>
        /// <returns>Task</returns>
        private Task DownloadByTask()
        {
            var tasks = new Task[3];
            for (int i = 0; i < tasks.Length; i++)
            {
                // Webリクエストを作成
                var request = WebRequest.Create(URL);

                tasks[i] = Task.Run(() =>
                {
                    // Webリクエストを実行
                    using (var response = request.GetResponse())
                    {
                        // ファイルダウンロードを実行
                        Download(response);
                    }

                    // 画面再描画
                    Invoke(((MethodInvoker)RefreshForm));
                });
            }
            return Task.WhenAll(tasks);
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