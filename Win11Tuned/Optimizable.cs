namespace Win11Tuned;

public interface Optimizable
{
	string Name { get; }

	string Description { get; }

	void Optimize();
}
