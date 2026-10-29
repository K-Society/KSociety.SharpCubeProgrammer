// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer
{
    using System;
    using System.Globalization;
    using SharpCubeProgrammer.Enum;

    /// <include file='.\Doc\HexConverterUtil.xml' path='docs/members[@name="hexConverterUtil"]/HexConverterUtil/*'/>
    public static class HexConverterUtil
    {
        private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture.NumberFormat;

        /// <include file='.\Doc\HexConverterUtil.xml' path='docs/members[@name="hexConverterUtil"]/HexConverterToUint/*'/>
        public static uint HexConverterToUint(string hex)
        {
            var parseResult = UInt32.TryParse(StringFilter(hex), NumberStyles.HexNumber, FormatProvider, out var result);

            return parseResult ? result : 0;
        }

        /// <include file='.\Doc\HexConverterUtil.xml' path='docs/members[@name="hexConverterUtil"]/HexConverterToInt/*'/>
        public static int HexConverterToInt(string hex)
        {
            var parseResult = Int32.TryParse(StringFilter(hex), NumberStyles.HexNumber, FormatProvider, out var result);

            return parseResult ? result : 0;
        }

        /// <include file='.\Doc\HexConverterUtil.xml' path='docs/members[@name="hexConverterUtil"]/HexConverterToString/*'/>
        public static string HexConverterToString(uint hex)
        {
            var output = "0x" + hex.ToString("X");
            return output;
        }

        /// <include file='.\Doc\HexConverterUtil.xml' path='docs/members[@name="hexConverterUtil"]/HexConverterToString/*'/>
        public static string HexConverterToString(int hex)
        {
            var output = "0x" + hex.ToString("X");
            return output;
        }

        private static string StringFilter(string hex)
        {
            if (String.IsNullOrEmpty(hex))
            {
                return String.Empty;
            }

            if (hex.StartsWith("0x") || hex.StartsWith("0X"))
            {
                var output = hex.Split('x', 'X');
                if (output.Length > 1)
                {
                    return output[1];
                }
            }

            return hex;
        }

        internal static CubeProgrammerError CheckResult(int result)
        {
            var output = CubeProgrammerError.CubeProgrammerErrorOther;

            if (result >= (int)CubeProgrammerError.CubeProgrammerErrorOther && result <= (int)CubeProgrammerError.CubeProgrammerNoError)
            {
                if (System.Enum.IsDefined(typeof(CubeProgrammerError), result))
                {
                    output = (CubeProgrammerError)result;
                }
            }

            return output;
        }
    }
}
