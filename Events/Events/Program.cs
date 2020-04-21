using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace ConsoleApplication1
{
    public interface ICutDownNotifier
    {
        void Init(object sender);
        void Run(object sender);
    }
    public delegate void Waiter(object sender);
    public class Timer
    {
        public Timer(int time, string name = "timer", string msg = "info")
        {
            Time = time;
            this.name = name;
            if (msg == "info")
            {
                this.msg = $"Hash of the instance is {GetHashCode()}";
            }
            else this.msg = msg;
        }
        public string name, msg;
        public int Time { get; protected set; } // sec
        public void Wait()
        {
            OnTimerStart(this);
            Thread.Sleep(Time * 1000);
            OnTimeOut(this);
        }
        public event Waiter TimeOut, TimerStart;
        public void OnTimeOut(object sender)
        {
            TimeOut(sender);
        }
        public void OnTimerStart(object sender)
        {
            TimerStart(sender);
        }
    }
    public class LambdaUsage
    {
        public LambdaUsage(Timer timer)
        {
            this.timer = timer;
            Init();
        }
        void Init()
        {
            link = timer => WriteLine($"'Lambda express' regrets to say that {(timer as Timer).name} is out at {DateTime.Now}\n");
        }
        public readonly Timer timer;
        public Waiter link;
    }
    public class EvtHandler : ICutDownNotifier
    {
        public virtual void Init(object sender)
        {
            var o = sender as Timer;
            WriteLine($"It is out with:\n{o.msg}. Reported by {GetType()}.\n");
        }
        public virtual void Run(object sender)
        {
            var o = sender as Timer;
            WriteLine($"Timer '{o.name}'. Waiting for {o.Time} sec... {GetType()} reports\n");
        }
    }
    public class HandlerModernized : EvtHandler
    {
        public override void Init(object sender)
        {
            WriteLine($"Timer '{(sender as Timer).name}' is out {DateTime.Now}. {GetType()} reports\n");
            base.Init(sender);
        }
        public override void Run(object sender)
        {
            var o = sender as Timer;
            WriteLine($"{GetType()} informs: Timer '{o.name}' for {o.Time} sec starts at {DateTime.Now}.");
        }
    }
    public class Program
    {
        public static void Main()
        {
            while (true)
            {
                try
                {
                    WriteLine("Give a name to the first timer");
                    var s = ReadLine();
                    WriteLine("How long should it wait?");
                    int sec = Convert.ToInt32(ReadLine());
                    int sec2, sec3;
                    string s2, s3;
                    WriteLine("Give a name to the second timer");
                    s2 = ReadLine();
                    WriteLine("How long should it wait?");
                    sec2 = Convert.ToInt32(ReadLine());
                    WriteLine("Give a name to the third timer");
                    s3 = ReadLine();
                    WriteLine("How long should it wait?");
                    sec3 = Convert.ToInt32(ReadLine());
                    Timer timer = new Timer(sec, s), timer2 = new Timer(sec2, s2), timer3 = new Timer(sec3, s3);
                    var hndlr = new EvtHandler();
                    var newhndlr = new HandlerModernized();
                    var laus = new LambdaUsage(timer);
                    timer.TimeOut += laus.link;
                    timer.TimerStart += hndlr.Run;
                    timer.TimerStart += newhndlr.Run;
                    timer.TimeOut += hndlr.Init;
                    timer.TimeOut += newhndlr.Init;
                    // sync start of 3 timers
                    timer.Wait();
                }
                catch (FormatException)
                {
                    WriteLine("Error. Try again");
                }
            }
        }
    }
}
