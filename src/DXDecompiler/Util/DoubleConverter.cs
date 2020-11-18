using System;
using System.Globalization;
using System.Linq;

namespace DXDecompiler.Util
{
	/// <summary>
	/// A class to allow the conversion of doubles to string representations of
	/// their exact decimal values. The implementation aims for readability over
	/// efficiency.
	/// </summary>
	public static class DoubleConverter
	{
		/// <summary>
		/// Converts the given double to a string representation of its
		/// exact decimal value.
		/// </summary>
		/// <param name="d">The double to convert.</param>
		/// <returns>A string representation of the double's exact decimal value.</returns>
		public static string ToExactString(double d, int precision = 6)
		{
			if(double.IsPositiveInfinity(d))
				return "+Infinity";
			if(double.IsNegativeInfinity(d))
				return "-Infinity";
			if(double.IsNaN(d))
				return "NaN";

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
				return "0";
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

			// Finally, return the string with an appropriate sign
			if(negative)
				return "-" + ad.ToString(precision);
			else
				return ad.ToString(precision);
		}


		/// <summary>Private class used for manipulating
		public class ArbitraryDecimal
		{
			/// <summary>Digits in the decimal expansion, one byte per digit
			public byte[] digits;
			/// <summary> 
			/// How many digits are *after* the decimal point
			/// </summary>
			public int decimalPoint = 0;

			/// <summary> 
			/// Constructs an arbitrary decimal expansion from the given long.
			/// The long must not be negative.
			/// </summary>
			public ArbitraryDecimal(long x)
			{
				string tmp = x.ToString(CultureInfo.InvariantCulture);
				digits = new byte[tmp.Length];
				for(int i = 0; i < tmp.Length; i++)
					digits[i] = (byte)(tmp[i] - '0');
				Normalize();
			}
			public ArbitraryDecimal(string tmp, int decimalPoint)
			{
				digits = new byte[tmp.Length];
				for(int i = 0; i < tmp.Length; i++)
					digits[i] = (byte)(tmp[i] - '0');
				this.decimalPoint = decimalPoint;
			}

			/// <summary>
			/// Multiplies the current expansion by the given amount, which should
			/// only be 2 or 5.
			/// </summary>
			public void MultiplyBy(int amount)
			{
				byte[] result = new byte[digits.Length + 1];
				for(int i = digits.Length - 1; i >= 0; i--)
				{
					int resultDigit = digits[i] * amount + result[i + 1];
					result[i] = (byte)(resultDigit / 10);
					result[i + 1] = (byte)(resultDigit % 10);
				}
				if(result[0] != 0)
				{
					digits = result;
				}
				else
				{
					Array.Copy(result, 1, digits, 0, digits.Length);
				}
				Normalize();
			}

			/// <summary>
			/// Shifts the decimal point; a negative value makes
			/// the decimal expansion bigger (as fewer digits come after the
			/// decimal place) and a positive value makes the decimal
			/// expansion smaller.
			/// </summary>
			public void Shift(int amount)
			{
				decimalPoint += amount;
			}

			/// <summary>
			/// Removes leading/trailing zeroes from the expansion.
			/// </summary>
			public void Normalize()
			{
				int first;
				for(first = 0; first < digits.Length; first++)
					if(digits[first] != 0)
						break;
				int last;
				for(last = digits.Length - 1; last >= 0; last--)
					if(digits[last] != 0)
						break;

				if(first == 0 && last == digits.Length - 1)
					return;

				byte[] tmp = new byte[last - first + 1];
				for(int i = 0; i < tmp.Length; i++)
					tmp[i] = digits[i + first];

				decimalPoint -= digits.Length - (last + 1);
				digits = tmp;
			}
			/// <summary>
			/// Converts the value to a proper decimal string representation.
			/// </summary>
			public override string ToString()
			{
				return ToString(-1);
			}
			/// <summary>
			/// Round the digits
			/// </summary>
			/// <param name="precision"></param>
			public void RoundDigits(int precision)
			{
				int roundPoint;
				if(digits.Length - decimalPoint > 0)
				{
					// When number > 1 eg 18.09871673583984375 -> 18.098717
					// RoundPoin
					roundPoint = digits.Length - decimalPoint + precision;
				}
				else if(decimalPoint - digits.Length == precision)
				{
					//When significate figures are just behind the rounding point
					//EG 0.0000004 -> 0
					var digit = digits[0];
					digit = (byte)(digit >= 5 ? 1 : 0);
					if(digit > 0)
					{
						decimalPoint -= 1;
					}
					digits = new byte[] { digit };
					return;
				}
				else
				{
					//For number < 1, EG 0.003508398309350013732910156
					roundPoint = precision + (digits.Length - decimalPoint);
				}
				if(roundPoint >= digits.Length) return;
				if(roundPoint < 1) return;
				var result = new byte[roundPoint];
				bool carry = digits.Length > roundPoint && digits[roundPoint] >= 5;
				for(var i = roundPoint - 1; i >= 0; i--)
				{
					var digit = digits[i];
					if(carry) digit += 1;
					carry = digit > 9 ? true : false;
					if(carry)
					{
						result[i] = 0;
					}
					else
					{
						result[i] = digit;
					}
				}
				if(carry)
				{
					result[0] = 0;
					result = (new byte[] { 1 }).Concat(result).ToArray();
					decimalPoint -= 1;
				}
				digits = result;
			}
			/// <summary>
			/// Converts the value to a proper decimal string representation.
			/// </summary>
			public string ToString(int requestedPrecision = 6)
			{
				int precision = requestedPrecision == -1 ? digits.Length : requestedPrecision;
				char[] digitString = new char[digits.Length];
				string debugRounded = "";
				string debug = "";
				for(int i = 0; i < digits.Length; i++)
				{
					debug += (char)(digits[i] + '0');
				}
				if(requestedPrecision != -1) RoundDigits(precision);
				for(int i = 0; i < digits.Length; i++)
				{
					debugRounded += (char)(digits[i] + '0');
					digitString[i] = (char)(digits[i] + '0');
				}
				// Simplest case - nothing after the decimal point,
				// and last real digit is non-zero, eg value=35
				if(decimalPoint == 0)
				{
					return new string(digitString) + '.' + new string('0', 6);
				}

				// Fairly simple case - nothing after the decimal
				// point, but some 0s to add, eg value=350
				if(decimalPoint < 0)
				{
					return new string(digitString)
						+ new string('0', -decimalPoint)
						+ '.' + new string('0', precision);
				}
				// Value is just below the rounding point, eg 0.0000006
				if(decimalPoint - digitString.Length == precision)
				{
					return "0." +
						new string('0', decimalPoint - digitString.Length - 1) +
						(digits[0] >= 5 ? "1" : "0");
				}

				// Nothing before the decimal point, eg 0.035
				if(decimalPoint >= digitString.Length)
				{
					return "0." +
						(new string('0', (decimalPoint - digitString.Length)) +
						new string(digitString)).Substring(0, Math.Min(precision, digitString.Length)) +
						new string('0', Math.Max(precision - digitString.Length, 0));
				}
				// Most complicated case - part of the string comes
				// before the decimal point, part comes after it,
				// eg 3.5
				return new string(digitString, 0,
								   digitString.Length - decimalPoint) +
					"." +
					new string(digitString,
								digitString.Length - decimalPoint,
								Math.Min(decimalPoint, precision))
								+ ((decimalPoint <= (precision - 1) && requestedPrecision != -1) ?
								new string('0', precision - decimalPoint) :
								string.Empty);
			}
		}
	}
}