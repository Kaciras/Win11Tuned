using System;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;

namespace Win11Tuned.Rules;

// 考虑过用合并 JSON 的设计，但新版任务管理器出得也不久，而且数组不好合并，所以还是手动改放心些。
public class TaskmgrSettingsRule : Rule
{
	const string PATH = @"%LOCALAPPDATA%\Microsoft\Windows\TaskManager\settings.json";
    const long COL_HIDDEN = 17957377;
	const long TABLE_COLUMNS = 17196662917;

    readonly FileInfo settingsFile = new(Environment.ExpandEnvironmentVariables(PATH));

	public string Name => "调整任务管理器的视图";

	public string Description => "修改任务管理器的详细视图中的列，显示线程数、命令行，移除描述等无用的列。";

	JsonNode document;
	JsonNode tableSetting;
	JsonNode detailsTable;
	JsonNode processes;

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
		processes = document.Root["ListSettings"]["Lists"][0];

		return processes["Columns"][2]["Flags"].GetValue<long>() != COL_HIDDEN
            || document.Root["CpuMode"]?.GetValue<int>() != 1
            || tableSetting["AutoAdjustColumns"].GetValue<bool>()
			|| detailsTable["SelectedColumns"].GetValue<long>() != TABLE_COLUMNS;
    }

	public void Optimize()
	{
		tableSetting["AutoAdjustColumns"] = false;
		detailsTable["SelectedColumns"] = TABLE_COLUMNS;
        processes["Columns"][2]["Flags"] = COL_HIDDEN;

        document.Root["CpuMode"] = 1;

        var columnWidths = detailsTable["ColumnWidths"];
		columnWidths[0] = 160;		// Name
		columnWidths[2] = 48;		// PID
		columnWidths[7] = 40;		// CPU
		columnWidths[13] = 80;		// Memory (active)
		columnWidths[23] = 50;		// Threads
		columnWidths[34] = 1600;    // Command Line

        columnWidths = detailsTable["ColumnWidths_TwoCpuMetrics"];
        columnWidths[0] = 160;      // Name
        columnWidths[2] = 48;       // PID
        columnWidths[7] = 40;       // CPU
        columnWidths[13] = 80;      // Memory (active)
        columnWidths[23] = 50;      // Threads
        columnWidths[34] = 1600;    // Command Line

        File.WriteAllText(settingsFile.FullName, document.ToString());
	}
}
