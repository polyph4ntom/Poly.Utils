using System.Runtime.CompilerServices;
using UnityEngine;

namespace Poly.Common
{
	public static class FPolyColor
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Color FromRGB(float r, float g, float b)
		{
			return new Color(r / 255f, g / 255f, b / 255f, 1f);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Color FromRGBA(float r, float g, float b, float a)
		{
			return new Color(r / 255f, g / 255f, b / 255f, a);
		}

		public static Color Maroon => FromRGB(216, 27, 96);
		public static Color SherpaBlue => FromRGB(0, 29, 31);
		public static Color SeaGreen => FromRGB(46, 139, 87);
		public static Color YellowGreen => FromRGB(204, 255, 0);
		public static Color Magenta => FromRGB(253, 61, 181);
		public static Color Gold => FromRGB(239, 191, 4);
		public static Color CornflowerBlue => FromRGB(99, 149, 238);
		public static Color Orange => FromRGB(255, 165, 0);
		public static Color Blue => FromRGB(0, 0, 255);
		public static Color Yellow => FromRGB(255, 193, 7);
		
		
	}
}