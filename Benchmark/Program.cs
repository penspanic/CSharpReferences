using System;
using BenchmarkDotNet.Running;
using CSharpTest;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ConstructorBenchmark>();
        }
    }
}