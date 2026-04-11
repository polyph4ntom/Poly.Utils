namespace Poly.Serialization
{
	public static class FPolyIniValueSerializers
	{
		public static FPolyStringIniValueSerializer String => FPolyStringIniValueSerializer.instance;
		public static FPolyBooleanIniValueSerializer Boolean => FPolyBooleanIniValueSerializer.instance;
		public static FPolyIntegerIniValueSerializer Integer => FPolyIntegerIniValueSerializer.instance;
		public static FPolySingleIniValueSerializer Single => FPolySingleIniValueSerializer.instance;
	}
}


