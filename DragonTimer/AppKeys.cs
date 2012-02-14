using System.Collections.Generic;
using System.Windows.Forms;

namespace DragonTimer
{
    class AppKeys
    {
        private static string[] _stringValues;
        private static Dictionary<string, Keys> _dictionary;

        public static void Initialize()
        {
            _dictionary = new Dictionary<string, Keys>
                                 {
                                     {"LeftCtrl", Keys.LControlKey},
                                     {"Z", Keys.Z},
                                     {"X", Keys.X},
                                     {"C", Keys.C},
                                     {"A", Keys.A},
                                     {"1", Keys.D1},
                                     {"2", Keys.D2},
                                     {"`", Keys.Oemtilde}
                                 };
            var tempList = new List<string>();
            foreach (var element in _dictionary)
            {
                tempList.Add(element.Key);
            }
            _stringValues = tempList.ToArray();
        }

        public static string[] GetStringValues()
        {
            return _stringValues;
        }

        public static int GetKeyPosition(Keys key)
        {
            var position = 0;
            foreach (var element in _dictionary)
            {
                if(element.Value == key)
                {
                    return position;
                }
                position++;
            }
            return position;
        }

        public static Keys GetKeyFromStringValue(string stringValue)
        {
            return _dictionary[stringValue];
        }
    }
}
