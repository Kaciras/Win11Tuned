using System.Threading.Tasks;

namespace Win11Tunned;

public interface Optimizable
{
	string Name { get; }

	string Description { get; }

	void Optimize();
}
