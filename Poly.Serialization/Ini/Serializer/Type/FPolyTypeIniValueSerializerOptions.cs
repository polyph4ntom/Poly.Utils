using System;

namespace Poly.Serialization
{
	public sealed class FPolyTypeIniValueSerializerOptions
	{
		public Func<string, Type> Resolver { get; set; }
		public bool UseTypeGetTypeFallback { get; set; } = true;
		public bool UseAssemblyQualifiedName { get; set; } = true;
	}
}