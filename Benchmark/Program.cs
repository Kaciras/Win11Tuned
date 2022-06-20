using System;
using BenchmarkDotNet.Running;
using Win11Tunned.Benchmark;

BenchmarkRunner.Run<RegFilePerf>();
Console.ReadKey();
