using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WPFDemo.Common.Extensions
{
    [System.Diagnostics.DebuggerStepThrough]
    public static class Structs
    {

        #region Constants

        private const decimal DEFAULT_EPSILON = .0001m;
        private const double DEFAULT_DOUBLE_EPSILON = .0001d;

        private const decimal Ten = 10M;

        //The limit of the precision and scale relates to the ability to
        //Pow 10.  At 29 it overflows.
        public const int MAXPRECISION = 28;

        #endregion

        #region Static Methods

        public static bool AlmostEquals(this decimal Op1, decimal Op2, decimal Epsilon)
        {
            return Math.Abs(Op1 - Op2) < Epsilon;
        }
        public static bool AlmostEquals(this decimal Op1, decimal Op2)
        {
            return AlmostEquals(Op1, Op2, DEFAULT_EPSILON);
        }

        public static bool AlmostEquals(this double Op1, double Op2, double Epsilon)
        {
            return Math.Abs(Op1 - Op2) < Epsilon;
        }
        public static bool AlmostEquals(this double Op1, double Op2)
        {
            return AlmostEquals(Op1, Op2, DEFAULT_DOUBLE_EPSILON);
        }

        public static bool IsSameDate(this DateTime op1, DateTime op2)
        {
            return op1.Date == op2.Date;
        }
        public static bool IsSameMonth(this DateTime op1, DateTime op2)
        {
            return op1.Month == op2.Month;
        }
        public static bool IsSameDay(this DateTime op1, DateTime op2)
        {
            return op1.Day == op2.Day;
        }
        public static bool IsSameYear(this DateTime op1, DateTime op2)
        {
            return op1.Year == op2.Year;
        }

        public static byte[] CopyBytes(this byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            var Result = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
                Result[i] = bytes[i];
            return Result;
        }

        //http://stackoverflow.com/a/12408927
        // From http://www.daimi.au.dk/~ivan/FastExpproject.pdf
        // Left to Right Binary Exponentiation
        public static decimal Pow(this decimal x, uint y)
        {
            decimal A = 1m;
            BitArray e = new BitArray(BitConverter.GetBytes(y));
            int t = e.Count;

            for (int i = t - 1; i >= 0; --i)
            {
                A *= A;
                if (e[i] == true)
                {
                    A *= x;
                }
            }
            return A;
        }
        /// <summary>
        /// Returns the fractional part of a decimal.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal FractionalPart(this decimal x)
        {
            if (x == decimal.Zero)
                return decimal.Zero;

            //12345.6789 - 12345 = .6789
            return x - Math.Truncate(x);
        }

        /// <summary>
        /// Returns true if the value is valid for DECIMAL(precision, scale).
        /// </summary>
        /// <param name="value"></param>
        /// <param name="precision">Total number of digits, including the decimal places.</param>
        /// <param name="scale">Total number of decimal digits.</param>
        /// <returns></returns>
        public static bool IsValidPrecision(this decimal value, uint precision, uint scale)
        {
            if (precision < scale)
                throw new ArgumentException("Precision must be greater than or equal to the scale.", "precision");
            if (precision > Structs.MAXPRECISION)
                throw new ArgumentException(string.Format("Precision must be less than {0} due to decimal data type constraints.", Structs.MAXPRECISION + 1), "precision");
            if (scale > Structs.MAXPRECISION)
                throw new ArgumentException(string.Format("Scale must be less than {0} due to decimal data type constraints.", Structs.MAXPRECISION + 1), "scale");

            //Drop negative sign
            value = Math.Abs(value);

            //Get the integral and fractional parts
            var Integral = Math.Truncate(value);
            var Fractional = value - Integral;

            //First examine the scale.

            //Example:
            //Let Value = 12345.6789
            //Integral = 12345, Fractional = .6789

            //If Scale = 4, 10^4 = 10000.  .6789 * 10000 = 6789 has no fractional part so valid.
            //If Scale = 2, 10^2 = 100.  .6789 * 100 = 67.89 has a fractional part so invalid.
            //If scale is zero, then the fractional part must be zero

            if (scale == 0 && Fractional != decimal.Zero)
                return false;
            if (scale > 0 && (Fractional * Ten.Pow(scale)).FractionalPart() != decimal.Zero)
                return false;

            //The number of integral places is Precision - Scale
            //Guaranteed to be zero or greater.
            var IntegralParts = precision - scale;

            //If IntegralParts = 5, 10^5 = 100000.  12345 <= 100000 so valid.
            //If IntegralParts = 3, 10^3 = 1000.  12345 > 1000 so invalid.
            //If IntegralParts is zero, then the integral part must be zero
            if (IntegralParts == 0 && Integral != decimal.Zero)
                return false;
            if (IntegralParts > 0 && Integral > Ten.Pow((uint)IntegralParts))
                return false;

            return true;
        }

        /// <summary>
        /// Returns the DECIMAL(precision, scale) of a value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="precision">Total number of digits, including the decimal places.</param>
        /// <param name="scale">Total number of decimal digits.</param>
        public static void GetPrecision(this decimal value, out uint precision, out uint scale)
        {
            uint Temp;
            GetPrecision(value, out precision, out scale, out Temp);
        }

        /// <summary>
        /// Returns the DECIMAL(precision, scale) of a value.  Also will return the number of integral digits.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="precision">Total number of digits, including the decimal places.</param>
        /// <param name="scale">Total number of decimal digits.</param>
        /// <param name="integralScale">Total number of integral digits.</param>
        public static void GetPrecision(this decimal value, out uint precision, out uint scale, out uint integralScale)
        {
            integralScale = 0;
            precision = 0;
            scale = 0;

            if (value == decimal.Zero)
                return;

            //Drop negative sign
            value = Math.Abs(value);

            //Get the integral and fractional parts
            var Integral = Math.Truncate(value);
            var Fractional = value - Integral;

            //Move the decimal left on the Integral value
            //until we run out of digits.
            decimal Temp = Integral;
            while (Temp != decimal.Zero)
            {
                Temp = Math.Truncate(Temp / Ten);
                precision++;
                integralScale++;
            }

            //Move the decimal right on the Fractional value
            //until we run out of digits.
            Temp = Fractional;
            while (Temp != decimal.Zero)
            {
                Temp = Ten * Temp - Math.Truncate(Ten * Temp);
                precision++;
                scale++;
            }
        }

        /// <summary>
        /// Get the value string of a decimal with its full scale.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="scale">Total number of decimal digits.</param>
        /// <returns></returns>
        public static string ToPrecisionString(this decimal value, uint scale)
        {
            //ToString(f#) pads # zero's onto the decimal portion
            return value.ToString(string.Format("f{0}", scale.ToString(System.Globalization.CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Get the value string of a decimal with its full scale.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="scale">Total number of decimal digits.</param>
        /// <returns></returns>
        public static string ToPrecisionCurrencyString(this decimal value, uint scale)
        {
            //Same as ToPrecisionString, but with c instead of f.
            return value.ToString(string.Format("c{0}", scale.ToString(System.Globalization.CultureInfo.InvariantCulture)));
        }

        #endregion

    }
}