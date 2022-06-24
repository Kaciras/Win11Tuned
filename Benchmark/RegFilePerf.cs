using BenchmarkDotNet.Attributes;
using Win11Tuned.Benchmark.Properties;
using Win11Tuned.RegFile;

namespace Win11Tuned.Benchmark;

public class RegFilePerf
{
	readonly string content = Resources.RegFile;

	[Benchmark]
	public string Tokenize()
	{
		var t = new RegFileTokenizer(content);
		var avoidElimination = "";

		while (t.Read())
			avoidElimination = t.Value;

		return avoidElimination;
	}

	[Benchmark]
	public bool Read()
	{
		var t = new RegFileReader(content);
		var avoidElimination = false;

		while (t.Read())
			avoidElimination = t.IsKey;

		return avoidElimination;
	}
}
