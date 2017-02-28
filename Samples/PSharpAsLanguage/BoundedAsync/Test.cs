﻿using System;
using System.Collections.Generic;
using Microsoft.PSharp;

namespace BoundedAsync
{
    #region C# Classes and Structs

    internal struct CountMessage
    {
        public int Count { get; private set; }

        public CountMessage(int count)
        {
            this.Count = count;
        }

        public void Increment()
        {
            ++this.Count;
        }
    }

    #endregion
    
    public class Test
    {
        static void Main(string[] args)
        {
            var runtime = PSharpRuntime.Create();
            Test.Execute(runtime);
            Console.ReadLine();
        }

        [Microsoft.PSharp.Test]
        public static void Execute(PSharpRuntime runtime)
        {
            runtime.CreateMachine(typeof(Scheduler));
        }
    }
}
