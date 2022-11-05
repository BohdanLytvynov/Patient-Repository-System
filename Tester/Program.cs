// See https://aka.ms/new-console-template for more information

using Tester;
using TimerLib;

//var r = Functions.Contains("Агафонов", "Агиф");

//TimeSpan t0 = new TimeSpan(18, 0, 0);

//TimeSpan t1 = new TimeSpan(7, 59, 0);

//bool r = t0 < new TimeSpan(19, 0, 0);

//bool r1 = t1 < new TimeSpan(20, 0, 0);

TimerSystem ts = new TimerSystem(60, 0.0001);

ts.OnTimerFinished += Ts_OnTimerFinished;

ts.OnTimerChanged += Ts_OnTimerChanged;

void Ts_OnTimerChanged(double obj)
{
    Console.WriteLine($"Time: {Math.Round(obj, 0)}");
}

int i = 0;



while (i < 10)
{
    i++;

    ts.Start();

    ts.Reset();
}



void Ts_OnTimerFinished()
{
    Console.WriteLine("Timer Finished");
}

Console.ReadKey();


