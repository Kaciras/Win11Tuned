using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Win11Tuned.Test.Properties;

namespace Win11Tuned.Test;

[TestClass]
public sealed class HostsFileTest
{
	const string TestFile = @"Resources\hosts.txt";

	[TestMethod]
	public void ReadAndWrite()
	{
		var hosts = new HostsFile(TestFile);
		Assert.AreEqual(hosts.ToString(), File.ReadAllText(TestFile));
	}

	[TestMethod]
	public void Entries()
	{
		var hosts = new HostsFile(TestFile);
		var actual = hosts.Entries().ToList();

		var expected = new (string, string)[]
		{
			("v6.localhost", "::1"),
			("foobar", "::1"),
			("foobar", "127.0.0.1"),
			("foobar", "127.0.0.2"),
		};
		CollectionAssert.AreEqual(expected, actual);
	}

	[TestMethod]
	public void ContainsExactly()
	{
		var hosts = new HostsFile(TestFile);
		Assert.IsTrue(hosts.ContainsExactly("v6.localhost", "::1"));
		Assert.IsFalse(hosts.ContainsExactly("foobar", "127.0.0.1"));
		Assert.IsFalse(hosts.ContainsExactly("foobar", "11.22.33.44"));
		Assert.IsFalse(hosts.ContainsExactly("v4.localhost", "::1"));
	}

	[TestMethod]
	public void RemoveAll()
	{
		var hosts = new HostsFile(TestFile);
		hosts.RemoveAll("foobar");
		Assert.AreEqual(Resources.hosts_RemoveAll, hosts.ToString());
	}

	[TestMethod]
	public void RemoveNonExists()
	{
		var hosts = new HostsFile(TestFile);
		hosts.RemoveAll("kaciras.com");
		Assert.AreEqual(hosts.ToString(), File.ReadAllText(TestFile));
	}

	[TestMethod]
	public void Add()
	{
		var hosts = new HostsFile(TestFile);
		hosts.AddEmptyLine();
		hosts.Add("v4.localhost", "127.0.0.1");
		Assert.AreEqual(Resources.hosts_Add, hosts.ToString());
	}
}
