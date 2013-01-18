﻿using System;
using POCOMerger.definition.rules;
using POCOMerger.implementation;

namespace POCOMerger.definition
{
	internal interface IClassMergerDefinition
	{
		Type DefinedFor { get; }

		TRules GetRules<TRules>()
			where TRules : class, IAlgorithmRules;

		void Initialize(MergerImplementation mergerImplementation);
	}
}