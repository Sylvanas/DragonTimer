using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace DragonTimer
{
    class EventGUIBuilder
    {
        private readonly string _labelText;
        private List<Keys> _keyCombinationStart;
        private List<Keys> _keyCombinationAction;
        private readonly List<int> _intervalSecondsValues;
        private List<bool> _activateCurrentTimerTaskValue;
        private bool _addStringBeforeActualSoundValue;
        private readonly string _finishedMessage;

        private static int _tabIndex;
        private static Point _basePoint = new Point(108, 35);
        private int _currentX;
        private readonly MainWindow _mainWindow;
        private AlarmedEventTrigger _alarmedEventTrigger;
        private const int LabelXposition = 12;
        private ComboBox _firstShortcutKey;
        private ComboBox _secondShortcutKey;
        private ComboBox _firstShortcutActionKey;
        private ComboBox _secondShortcutActionKey;
        private static readonly Size KeyComboBoxSize = new Size(92, 21);
        private CheckBox _firstTimerCheckBox;
        private static readonly Size CheckBoxSize = new Size(15, 14);
        private const int CheckBoxHeightdifference = 3;
        private TextBox _firstTimerTextBox;
        private static readonly Size TimerTextBoxSize = new Size(30, 21);
        private CheckBox _secondTimerCheckBox;
        private TextBox _secondTimerTextBox;
        private CheckBox _thirdTimerCheckBox;
        private TextBox _thirdTimerTextBox;
        private CheckBox _timeOnlyCheckBox;
        private TextBox _finalMessageTextBox;
        private static readonly Size FinalMessagesize = new Size(92, 21);

        public EventGUIBuilder(MainWindow mainWindow, AlarmedEventTrigger alarmedEventTrigger, string labelText,
            List<Keys> keyCombinationStart, List<Keys> keyCombinationAction, List<int> intervalSecondsValues, List<bool> activateCurrentTimerTaskValue, bool addStringBeforeActualSoundValue, string finishedMessage)
        {
            _labelText = labelText;
            _mainWindow = mainWindow;
            _alarmedEventTrigger = alarmedEventTrigger;
            _keyCombinationStart = keyCombinationStart;
            _keyCombinationAction = keyCombinationAction;
            _activateCurrentTimerTaskValue = activateCurrentTimerTaskValue;
            _addStringBeforeActualSoundValue = addStringBeforeActualSoundValue;
            _finishedMessage = finishedMessage;
            _intervalSecondsValues = intervalSecondsValues;
            Buildforms();
            _basePoint.Y += 30;
        }

        private void Buildforms()
        {
            AddEventLabel();
            AddFirstShortcutKey();
            _currentX += 10;
            AddSecondShortcutKey();
            _currentX += 20;
            AddFirstActionKey();
            _currentX += 10;
            AddSecondActionKey();
            _currentX += 20;
            AddFirstTimerCheckBox();
            _currentX += 10;
            AddFirstTimerTextBox();
            _currentX += 40;
            AddSecondTimerCheckBox();
            _currentX += 10;
            AddSecondTimerTextBox();
            _currentX += 40;
            AddThirdTimerCheckBox();
            _currentX += 10;
            AddThirdTimerTextBox();
            _currentX += 70;
            AddTimeOnlycheck();
            _currentX += 90;
            AddFinishedMessage();
        }

        private void AddEventLabel()
        {
            var label = new Label { Location = new Point(LabelXposition, _basePoint.Y + CheckBoxHeightdifference), Size = FinalMessagesize, Text = _labelText };
            _mainWindow.Controls.Add(label);
        }

        private void AddFirstShortcutKey()
        {
            _firstShortcutKey = new ComboBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y), Size = KeyComboBoxSize, TabIndex = _tabIndex++ };
            _firstShortcutKey.Items.AddRange(new[] { "sdf", "sadf" });
            _currentX += _firstShortcutKey.Size.Width;
            _mainWindow.Controls.Add(_firstShortcutKey);
        }

        private void AddSecondShortcutKey()
        {
            _secondShortcutKey = new ComboBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y), Size = KeyComboBoxSize, TabIndex = _tabIndex++ };
            _secondShortcutKey.Items.AddRange(new[] { "sdf", "sadf" });
            _currentX += _secondShortcutKey.Size.Width;
            _mainWindow.Controls.Add(_secondShortcutKey);
        }

        private void AddFirstActionKey()
        {
            _firstShortcutActionKey = new ComboBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y), Size = KeyComboBoxSize, TabIndex = _tabIndex++ };
            _firstShortcutActionKey.Items.AddRange(new[] { "sdf", "sadf" });
            _currentX += _firstShortcutActionKey.Size.Width;
            _mainWindow.Controls.Add(_firstShortcutActionKey);
        }

        private void AddSecondActionKey()
        {
            _secondShortcutActionKey = new ComboBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y), Size = KeyComboBoxSize, TabIndex = _tabIndex++ };
            _secondShortcutActionKey.Items.AddRange(new[] { "sdf", "sadf" });
            _currentX += _secondShortcutActionKey.Size.Width;
            _mainWindow.Controls.Add(_secondShortcutActionKey);
        }

        private void AddFirstTimerCheckBox()
        {
            _firstTimerCheckBox = new CheckBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y + CheckBoxHeightdifference), Size = CheckBoxSize, TabIndex = _tabIndex++ };
            _currentX += _firstTimerCheckBox.Size.Width;
            _mainWindow.Controls.Add(_firstTimerCheckBox);
        }

        private void AddFirstTimerTextBox()
        {
            _firstTimerTextBox = new TextBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y), Size = TimerTextBoxSize, TabIndex = _tabIndex++, Text = _intervalSecondsValues[0].ToString() };
            _currentX += _firstTimerTextBox.Size.Width;
            _mainWindow.Controls.Add(_firstTimerTextBox);
        }

        private void AddSecondTimerCheckBox()
        {
            _secondTimerCheckBox = new CheckBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y + CheckBoxHeightdifference), Size = CheckBoxSize, TabIndex = _tabIndex++ };
            _currentX += _secondTimerCheckBox.Size.Width;
            _mainWindow.Controls.Add(_secondTimerCheckBox);
        }

        private void AddSecondTimerTextBox()
        {
            _secondTimerTextBox = new TextBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y), Size = TimerTextBoxSize, TabIndex = _tabIndex++, Text = _intervalSecondsValues[1].ToString() };
            _currentX += _secondTimerTextBox.Size.Width;
            _mainWindow.Controls.Add(_secondTimerTextBox);
        }

        private void AddThirdTimerCheckBox()
        {
            _thirdTimerCheckBox = new CheckBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y + CheckBoxHeightdifference), Size = CheckBoxSize, TabIndex = _tabIndex++ };
            _currentX += _thirdTimerCheckBox.Size.Width;
            _mainWindow.Controls.Add(_thirdTimerCheckBox);
        }

        private void AddThirdTimerTextBox()
        {
            _thirdTimerTextBox = new TextBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y), Size = TimerTextBoxSize, TabIndex = _tabIndex++, Text = _intervalSecondsValues[2].ToString() };
            _currentX += _thirdTimerTextBox.Size.Width;
            _mainWindow.Controls.Add(_thirdTimerTextBox);
        }

        private void AddTimeOnlycheck()
        {
            _timeOnlyCheckBox = new CheckBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y + CheckBoxHeightdifference), Size = CheckBoxSize, TabIndex = _tabIndex++ };
            _currentX += _timeOnlyCheckBox.Size.Width;
            _mainWindow.Controls.Add(_timeOnlyCheckBox);
        }

        private void AddFinishedMessage()
        {
            _finalMessageTextBox = new TextBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y), Size = FinalMessagesize, TabIndex = _tabIndex++, Text = _finishedMessage };
            _currentX += _finalMessageTextBox.Size.Width;
            _mainWindow.Controls.Add(_finalMessageTextBox);
        }
    }
}
