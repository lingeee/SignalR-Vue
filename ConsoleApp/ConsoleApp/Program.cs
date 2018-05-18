using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static readonly object _lock = new object();
        static bool _go = false;

        //静态变量和构造方法的执行顺序问题
        //异步方法 delegate & callback
        //异步方法 async/await
        //task 两种获取实例的方式 及其的状态变化
        //task个人经验：访问相同资源的划为一类任务，以最长的一类任务（锁）计算总时长
        static int index = 0;

        static ConcurrentQueue<Task> cq = new ConcurrentQueue<Task>();

        static async Task<string> asyncMethod()
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(2000);
                return "我是返回值";
            });
        }

        static CancellationTokenSource cancelToken = new CancellationTokenSource();

        delegate Func<string,string> GetDataDelegate();

        static Func<string, string> shixian()
        {
            return (input) => string.Format("Hello {0}", input);
        }

        static void callBack(IAsyncResult r) {
            GetDataDelegate g = r.AsyncState as GetDataDelegate;

            Console.WriteLine("callBack:{0}", g.EndInvoke(r)("world")) ;
        }

        static void Main(string[] args)
        {
            #region collections
            //stack
            Stack stack = new Stack();
            ConcurrentStack<int> stack2 = new ConcurrentStack<int>();

            //queue
            Queue queue = new Queue();
            ConcurrentQueue<int> queue2 = new ConcurrentQueue<int>();

            //set
            SortedSet<int> set = new SortedSet<int>();
            HashSet<string> set1 = new HashSet<string>();
            ConcurrentBag<int> set2 = new ConcurrentBag<int>();

            //key/value
            Hashtable ht = new Hashtable();
            ConcurrentDictionary<int, int> dict = new ConcurrentDictionary<int, int>();

            //list
            ArrayList array3 = new ArrayList();
            List<string> array = new List<string>();
            LinkedList<int> li = new LinkedList<int>();

            //BitArray
            BitArray ba = new BitArray(1);

            #endregion

            GetDataDelegate gd = new GetDataDelegate(shixian);
            gd.BeginInvoke(callBack, gd);

            Func<int, int> a = (int b) => b + 2;

            Console.WriteLine("a：{0}", a(1));
            Console.ReadLine();

            //可取消的委托
            Console.WriteLine("可取消任务 线程开始 时间：{0}", DateTime.Now.ToString("HH:mm:ss"));

            ThreadPool.QueueUserWorkItem((o)=> DoSomeThing(cancelToken.Token, o));

            Console.ReadLine();

            cancelToken.Cancel();
            Console.WriteLine("可取消任务 线程结束 时间：{0}", DateTime.Now.ToString("HH:mm:ss"));
            Console.ReadLine();

            Console.WriteLine("主线程开始 时间：{0}", DateTime.Now.ToString("HH:mm:ss"));

            List<Task> list = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                list.Add(Task.Factory.StartNew(() =>
                {
                    Work(i);
                }));
            }

            list.Add(Task.Factory.StartNew(() =>
            {
                Console.WriteLine("支线结束 结果：{0}", asyncMethod().Result);
            }));

            //主线程工作三秒
            Thread.Sleep(3000);
            while (list.Count > 0)
            {
                if (list[0].IsCompleted) list.RemoveAt(0);
            }

            
            
            Console.WriteLine("主线程结束 时间：{0}", DateTime.Now.ToString("HH:mm:ss"));
            Console.ReadLine();


            //ThreadPool

            //Console.ReadLine();
            //Console.WriteLine("开始 等待用户输入");

            ////new Thread(Work).Start(1);
            //Console.ReadKey();
            //Console.WriteLine("IsEntered =", Monitor.IsEntered(_lock).ToString());
            //lock (_lock)
            //{
            //    Console.WriteLine("Main lock");
            //    _go = true;
            //    Monitor.Pulse(_lock);

            //}

            //Console.WriteLine("进入下一步");

            //Console.ReadLine();
        }


        private static void   DoSomeThing(CancellationToken token, object o)  
        {  
            for (int i = 0; i<int.MaxValue; i++)  
            {  
                for (int j = 0; j<int.MaxValue; j++)  
                {
                    Console.WriteLine("i= {0}", i);
                    Console.WriteLine("j= {0}", j);
                    if (token.IsCancellationRequested)  
                    {
                        
                        break;  
                    }
                    
                    if (j == int.MaxValue - 1)  
                    {  
                        i++;
                       
                    }
                }
                if (token.IsCancellationRequested) { break; }
            }
        }

        static void Work(int i)
        {
            lock (_lock)
            {
                Console.WriteLine("执行顺序：{2} in当前TaskID：{0} index：{1}", Task.CurrentId, index, i);
                
                index++;
                Thread.Sleep(500);

                Console.WriteLine("执行顺序：{2} out当前TaskID：{0} index：{1}", Task.CurrentId, index, i);
            }

            Thread.Sleep(500);


        }


        public class Single
        {
            private static Single _this;
            private static readonly object _locker = new object();

            private Single(){}

            public static Single getSingle()
            {
                if (_this == null)
                {
                    lock (_locker)
                    {
                        if (_this == null)
                        {
                            _this = new Single();
                        }
                    }
                }
                
                return _this;
            }
        }


    }
}
