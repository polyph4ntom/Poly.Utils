namespace Poly.Serialization
{
	public static class FPolyIniValueSerializers
	{
		public static FPolyStringIniValueSerializer String => FPolyStringIniValueSerializer.instance;
		public static FPolyBooleanIniValueSerializer Boolean => FPolyBooleanIniValueSerializer.instance;
		public static FPolyIntegerIniValueSerializer Integer => FPolyIntegerIniValueSerializer.instance;
		public static FPolySingleIniValueSerializer Single => FPolySingleIniValueSerializer.instance;
		public static FPolyGuidIniValueSerializer Guid => FPolyGuidIniValueSerializer.instance;
		public static FPolyVector2IniValueSerializer Vector2 => FPolyVector2IniValueSerializer.instance;
		public static FPolyVector3IniValueSerializer Vector3 => FPolyVector3IniValueSerializer.instance;
		public static FPolyColorIniValueSerializer Color => FPolyColorIniValueSerializer.instance;
		public static FPolyQuaternionIniValueSerializer Quaternion => FPolyQuaternionIniValueSerializer.instance;
	}
}


