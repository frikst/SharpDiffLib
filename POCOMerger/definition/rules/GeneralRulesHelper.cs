using System;
using POCOMerger.fastReflection;
using POCOMerger.implementation;

namespace POCOMerger.definition.rules
{
	public static class GeneralRulesHelper<TType>
	{
		public static Property GetIdProperty(MergerImplementation mergerImplementation)
		{
			return GeneralRulesHelper.GetIdProperty(mergerImplementation, typeof(TType));
		}
	}

	public static class GeneralRulesHelper
	{
		public static Property GetIdProperty(MergerImplementation mergerImplementation, Type type )
		{
			Property idProperty = null;

			IGeneralRules rules = mergerImplementation.GetMergerRulesFor<IGeneralRules>(type);

			if (rules != null)
				idProperty = rules.IdProperty;

			return idProperty;
		}
	}
}
