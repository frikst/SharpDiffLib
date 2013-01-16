using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

			IClassMergerDefinition mergerDefinition = mergerImplementation.GetMergerFor(type);

			if (mergerDefinition != null)
			{
				IGeneralRules rules = mergerDefinition.GetRules<IGeneralRules>();
				if (rules != null)
					idProperty = rules.IdProperty;
			}

			return idProperty;
		}
	}
}
