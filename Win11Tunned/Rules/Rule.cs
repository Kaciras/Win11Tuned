namespace Win11Tunned.Rules;

/// <summary>
/// 表示一条优化规则，调用 NeedOptimize() 来检测是否可优化。
/// </summary>
public interface Rule : Optimizable
{
	/// <summary>
	/// 检测是否能优化，该方法一定会在 Optimize() 之前调用，可以在此改变内部状态。
	/// </summary>
	/// <returns>如果需要优化则为 true</returns>
	bool NeedOptimize();

	// 以前这方法叫 Check，弄不清到底是检查需要优化还是不需要，所以改个更清晰的名字。
}
