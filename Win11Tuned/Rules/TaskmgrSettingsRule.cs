using System;
using System.IO;
using System.Text.Json.Nodes;

namespace Win11Tuned.Rules;

public class TaskmgrSettingsRule : Rule
{
	const string PATH = @"%LOCALAPPDATA%\Microsoft\Windows\TaskManager\settings.json";
	const long COLUMNS = 17196662917;

	readonly FileInfo settingsFile = new(Environment.ExpandEnvironmentVariables(PATH));

	public string Name => "调整任务管理器的视图";

	public string Description => "修改任务管理器的详细视图中的列，显示线程数、命令行，移除描述等无用的列。";

	JsonNode document;
	JsonNode tableSetting;
	JsonNode detailsTable;

	public bool NeedOptimize()
	{
        // 因为不关心其它项，我也懒得自己带一份初始的，一般装了系统总会开一次任务管理器吧。
        if (!settingsFile.Exists)
		{
			return false;
		}

		using var stream = settingsFile.OpenRead();
		document = JsonNode.Parse(stream);
		tableSetting = document.Root["TableSetting"];
		detailsTable = tableSetting["Tables"][0];

		return tableSetting["AutoAdjustColumns"].GetValue<bool>() 
			|| detailsTable["SelectedColumns"].GetValue<long>() != COLUMNS;
	}

	public void Optimize()
	{
		tableSetting["AutoAdjustColumns"] = false;
		detailsTable["SelectedColumns"] = COLUMNS;

		var columnWidths = detailsTable["ColumnWidths"];
		columnWidths[0] = 160;		// Name
		columnWidths[2] = 48;		// PID
		columnWidths[7] = 40;		// CPU
		columnWidths[13] = 80;		// Memory (active)
		columnWidths[23] = 50;		// Threads
		columnWidths[33] = 1600;    // Command Line

		File.WriteAllText(settingsFile.FullName, document.ToString());
	}
}
