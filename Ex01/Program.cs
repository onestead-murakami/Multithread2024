using System;
using System.Threading;

namespace Ex01
{
    class Program
    {
        static void Main(string[] args)
        {
            var start = DateTime.Now;
            Console.WriteLine("□□ 開始");
            for (int i = 0; i < 10; i++)
            {
                Worker(i);
            }
            Console.WriteLine("□□ 時間: {0}", DateTime.Now - start);
            Console.WriteLine("□□ 終了");
            Console.ReadLine();
        }

        static void Worker(int index)
        {
            // 2秒待つ
            Thread.Sleep(2000);
            Console.WriteLine("□ Worker: {0}", index);
        }
    }
}
