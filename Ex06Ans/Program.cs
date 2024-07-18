using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Ex06Ans
{
    class Program
    {
        // 書き込みファイルを開く
        static StreamWriter writer = new StreamWriter("Ex06Ans.txt", false);

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
                // ファイル書き込み
                writer.WriteLine("□ Worker({1}): {0}", index, Thread.CurrentThread.ManagedThreadId);
            }
        }
    }
}
