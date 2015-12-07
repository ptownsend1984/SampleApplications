using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Reflection.Helper2
{
    public class PerformanceHelper
    {

        public PerformanceCounter Native { get; private set; }
        public PerformanceCounter Reflections { get; private set; }
        public PerformanceCounter Lambdas { get; private set; }
        public PerformanceCounter Expressions { get; private set; }

        public void CountUriConstructions(TimeSpan runtime)
        {
            const string address = @"http://www.ruralsourcing.com";

            var type = typeof(Uri);
            var constructor = type.GetConstructor(new Type[] { typeof(string) });

            PerformanceCounter counter;
            ulong hits;
            DateTime? startTime;
            DateTime? stopTime;
            System.Threading.TimerCallback callback;
            callback = o =>
            {
                stopTime = DateTime.Now;
            };

            counter = new PerformanceCounter();
            hits = 0;
            startTime = DateTime.Now;
            stopTime = null;
            using (var timer = new System.Threading.Timer(callback, null, runtime, TimeSpan.Zero))
            {
                while (stopTime == null)
                {
                    new Uri(address);
                    hits++;
                }
            }

            counter.Name = "Native";
            counter.Hits = hits;
            counter.StartTime = startTime.Value;
            counter.StopTime = stopTime.Value;
            this.Native = counter;

            var args = new object[] { address };
            counter = new PerformanceCounter();
            hits = 0;
            startTime = DateTime.Now;
            stopTime = null;
            using (var timer = new System.Threading.Timer(callback, null, runtime, TimeSpan.Zero))
            {
                while (stopTime == null)
                {
                    constructor.Invoke(args);
                    hits++;
                }
            }

            counter.Name = "Reflections";
            counter.Hits = hits;
            counter.StartTime = startTime.Value;
            counter.StopTime = stopTime.Value;
            this.Reflections = counter;

            counter = new PerformanceCounter();
            hits = 0;
            startTime = DateTime.Now;
            stopTime = null;
            using (var timer = new System.Threading.Timer(callback, null, runtime, TimeSpan.Zero))
            {
                while (stopTime == null)
                {
                    var lambda = (Action)(() => { new Uri(address); });
                    lambda();
                    hits++;
                }
            }

            counter.Name = "Lambdas";
            counter.Hits = hits;
            counter.StartTime = startTime.Value;
            counter.StopTime = stopTime.Value;
            this.Lambdas = counter;

            var newExp = Expression.New(constructor, Expression.Constant(address));
            var lambdaExp = Expression.Lambda(typeof(Func<Uri>), newExp);
            var lambdaCompile = (Func<Uri>)lambdaExp.Compile();

            counter = new PerformanceCounter();
            hits = 0;
            startTime = DateTime.Now;
            stopTime = null;
            using (var timer = new System.Threading.Timer(callback, null, runtime, TimeSpan.Zero))
            {
                while (stopTime == null)
                {
                    lambdaCompile();
                    hits++;
                }
            }

            counter.Name = "Expressions";
            counter.Hits = hits;
            counter.StartTime = startTime.Value;
            counter.StopTime = stopTime.Value;
            this.Expressions = counter;
        }

    }

    public class PerformanceCounter
    {
        public string Name { get; set; }
        public ulong Hits { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
    }
}