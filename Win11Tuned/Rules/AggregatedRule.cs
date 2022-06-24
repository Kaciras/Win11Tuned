using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win11Tunned.Rules;

internal class AggregatedRule : Rule
{
    public string Name => throw new NotImplementedException();

    public string Description => throw new NotImplementedException();

    readonly IEnumerable<Rule> rules;



    public bool NeedOptimize()
    {
        throw new NotImplementedException();
    }

    public void Optimize()
    {
        throw new NotImplementedException();
    }
}
