namespace KST.SharpDiffLib.DiffResult.Factory
{
	public static class DiffFactory
	{
		public static InnerDiffFactory.IDiffFactory<TType, TType> Create<TType>()
		{
			return new InnerDiffFactory.DiffFactoryImplementation<TType, TType>();
		}
	}
}
