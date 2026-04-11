using System;

namespace Poly.Serialization
{
	public interface IPolyIniValueSerializer<T>
	{
		bool TryParse(ReadOnlySpan<char> text, out T value);
		string Format(T value);
	}
}