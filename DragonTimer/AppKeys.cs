using System.Windows.Forms;

namespace DragonTimer
{
    class AppKeys
    {
        private static readonly string[] StringValues = new[] {"LeftCtrl", "X", "A", "`", "Z", "1", "2", "C"};

        public static string[] GetStringValues()
        {
            return StringValues;
        }

        public static int GetKeyPosition(Keys key)
        {
            for (var i = 0; i < StringValues.Length; i++)
            {
                if (GetKeyFromStringValue(StringValues[i]) == key)
                {
                    return i;
                }
            }
            return 0;
        }

        public static Keys GetKeyFromStringValue(string stringValue)
        {
            switch (stringValue)
            {
                case "LeftCtrl": return Keys.LControlKey;
                case "`": return Keys.Oemtilde;
                case "X": return Keys.X;
                case "A": return Keys.A;
                case "Z": return Keys.Z;
                case "1": return Keys.D1;
                case "2": return Keys.D2;
                case "C": return Keys.C;
            }
            return Keys.LControlKey;
        }
    }
}
