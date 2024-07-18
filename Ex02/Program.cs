using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ex02
{
    class Program
    {
        static void Main(string[] args)
        {
            var start = DateTime.Now;
            Console.WriteLine("□□ 開始");

            // .NET管理の利用スレッド上限値（研修で違いを確認するため必要なら第一引数を調整）
            //ThreadPool.SetMinThreads(0, 1000);
            //ThreadPool.SetMaxThreads(8, 1000);

            // 別スレッド起動したタスクを記録
            Task[] tasks = new Task[10];

            for (int i = 0; i < 10; i++)
            {
                // 数値の格納先をループ変数とは別にする
                int index = i;

                // TASKで並列処理（マルチスレッド）
                tasks[i] = Task.Run(() => Worker(index));
            }

            // 別スレッド起動したタスクがすべて終了するまで待つ
            Task.WaitAll(tasks);

            Console.WriteLine("□□ 時間: {0}", DateTime.Now - start);
            Console.WriteLine("□□ 終了");
            Console.ReadLine();
        }

        static void Worker(int index)
        {
            // 3秒待つ
            Thread.Sleep(3000);
            Console.WriteLine("□ Worker({1}): {0}", index, Thread.CurrentThread.ManagedThreadId);
        }
    }
}
