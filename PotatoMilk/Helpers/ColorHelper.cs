using SFML.Graphics;
using System;
using System.Text.RegularExpressions;

namespace PotatoMilk.Helpers
{
    internal static class ColorHelper
    {
        internal static Color FromFloats(float r, float g, float b, float a = 1f)
        {
            return new Color(
                (byte)(Math.Clamp(r, 0, 1) * 255),
                (byte)(Math.Clamp(g, 0, 1) * 255),
                (byte)(Math.Clamp(b, 0, 1) * 255),
                (byte)(Math.Clamp(a, 0, 1) * 255)
                );
        }

        internal static Color FromString(string color)
        {
            string text = Regex.Replace(color.ToUpperInvariant(), @"[^0-9A-F]", "");
            if (text.Length == 3)
                return new Color(ToByte(new string(text[0], 2)), ToByte(new string(text[1], 2)), ToByte(new string(text[2], 2)));
            else if (text.Length == 4)
                return new Color(ToByte(new string(text[0], 2)), ToByte(new string(text[1], 2)), ToByte(new string(text[2], 2)), ToByte(new string(text[3], 2)));
            else if (text.Length == 6)
                return new Color(ToByte(text.Substring(0, 2)), ToByte(text.Substring(2, 2)), ToByte(text.Substring(4, 2)));
            else if (text.Length == 8)
                return new Color(ToByte(text.Substring(0, 2)), ToByte(text.Substring(2, 2)), ToByte(text.Substring(4, 2)), ToByte(text.Substring(6, 2)));
            throw new Exception("Could not parse color \"" + color + "\"");
        }

        private static byte ToByte(string twoChars)
        {
            return (byte)(
                (twoChars[0] <= '9' ? (twoChars[0] - '0') * 16 : (twoChars[0] - 'A' + 10) * 16) +
                (twoChars[1] <= '9' ? twoChars[1] - '0' : twoChars[1] - 'A' + 10)
                );
        }
    }
}
