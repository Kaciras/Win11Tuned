using System;
using System.Management;
using BenchmarkDotNet.Running;
using Win11Tuned.Benchmark;

string wmipathstr = @"\\" + Environment.MachineName + @"\root\SecurityCenter:AntiVirusProduct";
var searcher = new ManagementObjectSearcher(wmipathstr, "SELECT * FROM AntivirusProduct");
ManagementObjectCollection instances = searcher.Get();
foreach (ManagementObject obj in instances)
    Console.WriteLine(obj.GetPropertyValue("displayName"));

var avp = new ManagementClass(wmipathstr);
var status = avp.CreateInstance();
status.SetPropertyValue("displayName", "Win11Tuned_FakeAV");
status.SetPropertyValue("instanceGuid", $"{{{Guid.NewGuid().ToString()}}}");
status.SetPropertyValue("productUptoDate", true);
status.SetPropertyValue("onAccessScanningEnabled", true);

status.Put();

Console.ReadKey();
