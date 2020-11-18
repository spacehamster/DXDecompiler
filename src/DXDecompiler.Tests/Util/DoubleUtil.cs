using System;
using System.Linq;
using static DXDecompiler.Util.DoubleConverter;

namespace DXDecompiler.Tests.Util
{
	public class DoubleUtil
	{
		/// <summary>
		/// Converts the given double to a string representation of its
		/// exact decimal value.
		/// </summary>
		/// <param name="d">The double to convert.</param>
		/// <returns>A string representation of the double's exact decimal value.</returns>
		public static ArbitraryDecimal ToArbitaryDecimal(double d, int precision = 6)
		{

			// Translate the double into sign, exponent and mantissa.
			long bits = BitConverter.DoubleToInt64Bits(d);
			// Note that the shift is sign-extended, hence the test against -1 not 1
			bool negative = (bits < 0);
			int exponent = (int)((bits >> 52) & 0x7ffL);
			long mantissa = bits & 0xfffffffffffffL;

			// Subnormal numbers; exponent is effectively one higher,
			// but there's no extra normalisation bit in the mantissa
			if(exponent == 0)
			{
				exponent++;
			}
			// Normal numbers; leave exponent as it is but add extra
			// bit to the front of the mantissa
			else
			{
				mantissa = mantissa | (1L << 52);
			}

			// Bias the exponent. It's actually biased by 1023, but we're
			// treating the mantissa as m.0 rather than 0.m, so we need
			// to subtract another 52 from it.
			exponent -= 1075;

			if(mantissa == 0)
			{
				throw new NotImplementedException();
				//return "0";
			}

			/* Normalize */
			while((mantissa & 1) == 0)
			{    /*  i.e., Mantissa is even */
				mantissa >>= 1;
				exponent++;
			}

			// Construct a new decimal expansion with the mantissa
			ArbitraryDecimal ad = new ArbitraryDecimal(mantissa);

			// If the exponent is less than 0, we need to repeatedly
			// divide by 2 - which is the equivalent of multiplying
			// by 5 and dividing by 10.
			if(exponent < 0)
			{
				for(int i = 0; i < -exponent; i++)
					ad.MultiplyBy(5);
				ad.Shift(-exponent);
			}
			// Otherwise, we need to repeatedly multiply by 2
			else
			{
				for(int i = 0; i < exponent; i++)
					ad.MultiplyBy(2);
			}
			return ad;
		}
		public static string FormatFloat(float value)
		{
			if(value == 0f) return "0";
			var result = ((double)value).ToString("G32");
			if(result.Contains("E"))
			{
				var parts = result.Split(new char[] { '.', 'E' });
				var exponent = int.Parse(parts.Last());
				if(exponent < 6)
				{
					string negative = result.StartsWith("-") ? "-" : "";
					return negative + "0.000000";
				}
				else if(exponent < 0)
				{
					string negative = result.StartsWith("-") ? "-" : "";
					if(parts[0].StartsWith("-")) parts[0] = parts[0].Substring(1, parts[0].Length - 1);
					return negative + "0." + new string('0', exponent * -1 - 1) + parts[0] + parts[1];
				}
				else if(exponent < parts[1].Length)
				{
					var second = parts[1].Substring(0, exponent);
					var third = parts[1].Substring(exponent, parts[1].Length - exponent);
					return parts[0] + second + "." + third;
				}
				else
				{
					return parts[0] + parts[1] + new string('0', exponent - parts[1].Length) + ".000000";
				}

			}
			else if(!result.Contains("."))
			{
				return result + ".000000";
			}
			return result;
		}
	}
}
