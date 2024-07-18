using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Ex05
{
    class Program
    {
        // 書き込みファイルを開く
        static StreamWriter writer = new StreamWriter("Ex05.txt", false);

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

            // ファイル書き込み
            writer.WriteLine("□ Worker({1}): {0}", index, Thread.CurrentThread.ManagedThreadId);
        }
    }
}
