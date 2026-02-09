using System;

namespace Poly.BuildPipeline
{
	[Flags]
	public enum EPolyBuildConfig
	{
		None = 0,
		Editor = 1,
		Debug =  2,
		Development = 4,
		Release = 8,
	}
}
