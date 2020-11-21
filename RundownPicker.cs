using CellMenu;
using GTFO.Custom.Rundown.CRundown;
using System.Collections.Generic;

namespace GTFO.Custom.Rundown
{
    public static class RundownPicker
    {
        private static CM_Item _Button;

        public static CM_Item Button
        {
            get { return _Button; }
            set
            {
                if (value != null)
                {
                    _Button = value;
                    UpdateText();
                }
            }
        }

        public static bool IsMainScreen { get; set; } = false;

        public static bool IsDefault
        {
            get
            {
                return SelectedID == uint.MaxValue;
            }
        }

        public static uint SelectedID
        {
            get
            {
                return _Rundowns[_CurrentIndex].ID;
            }
        }

        public static string SelectedName
        {
            get
            {
                return _Rundowns[_CurrentIndex].Name;
            }
        }

        public static string SelectedChecksum
        {
            get
            {
                return _Rundowns[_CurrentIndex].Checksum;
            }
        }

        public static bool IsNextAvailable
        {
            get
            {
                return _CurrentIndex < _Rundowns.Count - 1;
            }
        }

        public static bool IsPreviousAvailable
        {
            get
            {
                return _CurrentIndex > 0;
            }
        }

        private static List<RundownPair> _Rundowns = new List<RundownPair>();
        private static int _CurrentIndex = 0;

        static RundownPicker()
        {
            AddItem("Default", "", uint.MaxValue);
        }

        public static void AddItem(string name, string checksum, uint id)
        {
            _Rundowns.Add(new RundownPair()
            {
                Name = name,
                ID = id,
                Checksum = checksum
            });
        }

        public static void Next()
        {
            if (IsNextAvailable)
            {
                _CurrentIndex++;
                UpdateText();
            }
        }

        public static void Previous()
        {
            if (IsPreviousAvailable)
            {
                _CurrentIndex--;
                UpdateText();
            }
        }

        private static void UpdateText()
        {
            UpdateTextStyle2();
        }

        private static void UpdateTextStyle1()
        {
            string text = string.Empty;

            if (IsDefault)
            {
                text = "Start Default";
            }
            else
            {
                text = $"Start Custom: {SelectedName}";
            }

            if (IsPreviousAvailable) text = $"< {text}";
            if (IsNextAvailable) text = $"{text} >";

            Button?.SetText(text);
        }

        private static void UpdateTextStyle2()
        {
            string text = string.Empty;

            if (IsDefault)
            {
                text = "Start Default Rundown";
            }
            else
            {
                text = $"Start Custom: {SelectedName}";
            }

            string text2 = string.Empty;
            for (int i = 0; i < _Rundowns.Count; i++)
            {
                if (i == _CurrentIndex)
                {
                    text2 += "o";
                }
                else
                {
                    text2 += ".";
                }
            }

            Button.SetText(text + "\n<size=60%>" + text2);
        }
    }
}