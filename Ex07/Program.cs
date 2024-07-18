using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Ex07
{
    class Program
    {
        // 書き込みファイルを開く
        // FileMode.Create：新規ファイルとして作成する。
        // FileAccess.ReadWrite：ファイル読み書き可能として開く
        // FileShare.ReadWrite：ファイル読み書きをプロセスに許可
        static FileStream writer = new FileStream("Ex07.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

        // 排他制御のためのオブジェクト
        static readonly object sync = new object();

        static void Main(string[] args)
        {
            var start = DateTime.Now;
            Console.WriteLine("□□ 開始");
            try
            {

                // PLINQ処理用にイテレーションを準備
                var items = Enumerable.Range(0, 10);

                // 前もって改行を書き込む
                for (int i = 0; i < items.Count(); i++)
                {
                    writer.WriteByte(13); //CR
                    writer.WriteByte(10); //LF
                }

                // PLINQで並列処理（マルチスレッド）
                items.AsParallel().ForAll(i => Worker(i));

            }
            finally
            {
                writer.Close();
                writer.Dispose();
            }
            Console.WriteLine("□□ 時間: {0}", DateTime.Now - start);
            Console.WriteLine("□□ 終了");
            Console.ReadLine();
        }

        static void Worker(int index)
        {
            // 3秒待つ
            Thread.Sleep(3000);

            // 同時書き込み排他制御
            lock (sync)
            {
                // ファイル内容を読み込み
                writer.Seek(0, SeekOrigin.Begin);
                byte[] data = new byte[writer.Length];
                writer.Read(data, 0, data.Length);

                // ファイル書き込み位置特定（start）
                int count = 0;
                int start = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    if (count == index)
                    {
                        break;
                    }
                    else
                    {
                        start++;
                        if (data[i] == 10)
                        {
                            count++;
                        }
                    }
                }
                
                // ファイル書き戻し文字列作成（ファイル書き込み位置より後ろ文字列を記憶）
                int length = data.Length - start;
                writer.Seek(start, SeekOrigin.Begin);
                writer.Read(data, 0, length);

                // ファイル書き込み（挿入）
                string text = String.Format("□ Worker({1}): {0}", index, Thread.CurrentThread.ManagedThreadId);
                writer.Seek(start, SeekOrigin.Begin);
                writer.Write(Encoding.UTF8.GetBytes(text), 0, Encoding.UTF8.GetByteCount(text));

                // ファイル書き込み（追記）
                writer.Write(data, 0, length);
            }
        }
    }
}
