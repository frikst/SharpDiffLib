using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using KST.SharpDiffLib.Definition;

namespace KST.SharpDiffLib.Internal.Members
{
    public static class RulesNotFoundFallbackMembers
    {
	    public static MethodInfo RulesNotFoundFallback(Type tAlgorithmRules, Type tType)
	    {
		    return typeof(IRulesNotFoundFallback).GetMethod(nameof(IRulesNotFoundFallback.RulesNotFoundFallback)).MakeGenericMethod(tAlgorithmRules, tType);
	    }
    }
}
