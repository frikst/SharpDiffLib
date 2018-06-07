using KST.SharpDiffLib.Base;
using KST.SharpDiffLib.Definition.Rules;

namespace KST.SharpDiffLib.Definition
{
    public interface IRulesNotFoundFallback
    {
	    TAlgorithmRules RulesNotFoundFallback<TAlgorithmRules, TType>(IMergerRulesLocator rulesLocator)
		    where TAlgorithmRules: class, IAlgorithmRules;
    }
}
