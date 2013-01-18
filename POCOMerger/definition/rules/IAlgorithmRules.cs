using POCOMerger.implementation;

namespace POCOMerger.definition.rules
{
	public interface IAlgorithmRules
	{
		void Initialize(MergerImplementation mergerImplementation);
		bool IsInheritable { get; set; }
		IAlgorithmRules InheritAfter { get; set; }
	}
}
