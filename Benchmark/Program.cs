using System;
using BenchmarkDotNet.Running;
using Win11Tuned.Benchmark;

BenchmarkRunner.Run<RegFilePerf>();
Console.ReadKey();
