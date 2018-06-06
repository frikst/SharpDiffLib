namespace KST.SharpDiffLib.Definition.Rules
{
    public interface IInheritableAlgorithmRules : IAlgorithmRules
    {
	    bool IsInheritable { get; set; }

	    IAlgorithmRules InheritedFrom { get; set; }
    }
}
