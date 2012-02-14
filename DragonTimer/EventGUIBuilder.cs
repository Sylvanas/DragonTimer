using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DragonTimer
{
    class EventGuiBuilder
    {
        private readonly string _labelText;
        private readonly List<Keys> _keyCombinationStart;
        private readonly List<Keys> _keyCombinationAction;
        private readonly List<int> _intervalSecondsValues;
        private readonly List<bool> _activateCurrentTimerTaskValue;
        private readonly bool _addStringBeforeActualSoundValue;
        private readonly string _finishedMessage;

        private static int _tabIndex;
        private static Point _basePoint = new Point(108, 35);
        private int _currentX;
        private readonly MainWindow _mainWindow;
        private readonly AlarmedEventTrigger _alarmedEventTrigger;
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

        public EventGuiBuilder(MainWindow mainWindow, AlarmedEventTrigger alarmedEventTrigger, string labelText,
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
            AddTimeOnlyCheck();
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
            _firstShortcutKey.Items.AddRange(AppKeys.GetStringValues());
            _firstShortcutKey.SelectedIndex = AppKeys.GetKeyPosition(_keyCombinationStart[0]);
            _firstShortcutKey.DropDownStyle = ComboBoxStyle.DropDownList;
            _firstShortcutKey.SelectedIndexChanged += ShortcutComboBoxSelectedIndexChanged;
            _currentX += _firstShortcutKey.Size.Width;
            _mainWindow.Controls.Add(_firstShortcutKey);
        }

        private void AddSecondShortcutKey()
        {
            _secondShortcutKey = new ComboBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y), Size = KeyComboBoxSize, TabIndex = _tabIndex++ };
            _secondShortcutKey.Items.AddRange(AppKeys.GetStringValues());
            _secondShortcutKey.SelectedIndex = AppKeys.GetKeyPosition(_keyCombinationStart[1]);
            _secondShortcutKey.DropDownStyle = ComboBoxStyle.DropDownList;
            _secondShortcutKey.SelectedIndexChanged += ShortcutComboBoxSelectedIndexChanged;
            _currentX += _secondShortcutKey.Size.Width;
            _mainWindow.Controls.Add(_secondShortcutKey);
        }

        private void ShortcutComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            _alarmedEventTrigger.SetKeycombinationStart(new List<Keys> { AppKeys.GetKeyFromStringValue(_firstShortcutKey.SelectedItem.ToString()), AppKeys.GetKeyFromStringValue(_secondShortcutKey.SelectedItem.ToString()) });
        }

        private void AddFirstActionKey()
        {
            _firstShortcutActionKey = new ComboBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y), Size = KeyComboBoxSize, TabIndex = _tabIndex++ };
            _firstShortcutActionKey.Items.AddRange(AppKeys.GetStringValues());
            _firstShortcutActionKey.SelectedIndex = AppKeys.GetKeyPosition(_keyCombinationAction[0]);
            _firstShortcutActionKey.DropDownStyle = ComboBoxStyle.DropDownList;
            _firstShortcutActionKey.SelectedIndexChanged += ActionComboBoxSelectedIndexChanged;
            _currentX += _firstShortcutActionKey.Size.Width;
            _mainWindow.Controls.Add(_firstShortcutActionKey);
        }

        private void AddSecondActionKey()
        {
            _secondShortcutActionKey = new ComboBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y), Size = KeyComboBoxSize, TabIndex = _tabIndex++ };
            _secondShortcutActionKey.Items.AddRange(AppKeys.GetStringValues());
            _secondShortcutActionKey.SelectedIndex = AppKeys.GetKeyPosition(_keyCombinationAction[1]);
            _secondShortcutActionKey.DropDownStyle = ComboBoxStyle.DropDownList;
            _secondShortcutActionKey.SelectedIndexChanged += ActionComboBoxSelectedIndexChanged;
            _currentX += _secondShortcutActionKey.Size.Width;
            _mainWindow.Controls.Add(_secondShortcutActionKey);
        }

        private void ActionComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            _alarmedEventTrigger.SetKeycombinationAction(new List<Keys> { AppKeys.GetKeyFromStringValue(_firstShortcutActionKey.SelectedItem.ToString()), AppKeys.GetKeyFromStringValue(_secondShortcutActionKey.SelectedItem.ToString()) });
        }

        private void AddFirstTimerCheckBox()
        {
            _firstTimerCheckBox = new CheckBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y + CheckBoxHeightdifference), Size = CheckBoxSize, TabIndex = _tabIndex++, Checked = _activateCurrentTimerTaskValue[0] };
            _firstTimerCheckBox.CheckedChanged += TimerCheckBoxChanged;
            _currentX += _firstTimerCheckBox.Size.Width;
            _mainWindow.Controls.Add(_firstTimerCheckBox);
        }

        private void AddFirstTimerTextBox()
        {
            _firstTimerTextBox = new TextBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y), Size = TimerTextBoxSize, TabIndex = _tabIndex++, Text = _intervalSecondsValues[0].ToString() };
            _firstTimerTextBox.KeyPress += IgnoreTextBoxNonDigit;
            _firstTimerTextBox.KeyUp += TimersChanged;
            _currentX += _firstTimerTextBox.Size.Width;
            _mainWindow.Controls.Add(_firstTimerTextBox);
        }

        private void AddSecondTimerCheckBox()
        {
            _secondTimerCheckBox = new CheckBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y + CheckBoxHeightdifference), Size = CheckBoxSize, TabIndex = _tabIndex++, Checked = _activateCurrentTimerTaskValue[1] };
            _secondTimerCheckBox.CheckedChanged += TimerCheckBoxChanged;
            _currentX += _secondTimerCheckBox.Size.Width;
            _mainWindow.Controls.Add(_secondTimerCheckBox);
        }

        private void AddSecondTimerTextBox()
        {
            _secondTimerTextBox = new TextBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y), Size = TimerTextBoxSize, TabIndex = _tabIndex++, Text = _intervalSecondsValues[1].ToString() };
            _secondTimerTextBox.KeyPress += IgnoreTextBoxNonDigit;
            _secondTimerTextBox.KeyUp += TimersChanged;
            _currentX += _secondTimerTextBox.Size.Width;
            _mainWindow.Controls.Add(_secondTimerTextBox);
        }

        private void AddThirdTimerCheckBox()
        {
            _thirdTimerCheckBox = new CheckBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y + CheckBoxHeightdifference), Size = CheckBoxSize, TabIndex = _tabIndex++, Checked = _activateCurrentTimerTaskValue[2] };
            _thirdTimerCheckBox.CheckedChanged += TimerCheckBoxChanged;
            _currentX += _thirdTimerCheckBox.Size.Width;
            _mainWindow.Controls.Add(_thirdTimerCheckBox);
        }

        void TimerCheckBoxChanged(object sender, EventArgs e)
        {
            _alarmedEventTrigger.SetActivationOfTimerTasks(new List<bool> { _firstTimerCheckBox.Checked, _secondTimerCheckBox.Checked, _thirdTimerCheckBox.Checked, true });
        }

        private void AddThirdTimerTextBox()
        {
            _thirdTimerTextBox = new TextBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y), Size = TimerTextBoxSize, TabIndex = _tabIndex++, Text = _intervalSecondsValues[2].ToString() };
            _thirdTimerTextBox.KeyPress += IgnoreTextBoxNonDigit;
            _thirdTimerTextBox.KeyUp += TimersChanged;
            _currentX += _thirdTimerTextBox.Size.Width;
            _mainWindow.Controls.Add(_thirdTimerTextBox);
        }

        static void IgnoreTextBoxNonDigit(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '\b':
                    break;
                default:
                    if (!Char.IsDigit(e.KeyChar))
                    {
                        e.Handled = true;
                    }
                    break;
            }
        }

        void TimersChanged(object sender, KeyEventArgs e)
        {
            _alarmedEventTrigger.SetAlarmTimers(new List<int> { int.Parse(_firstTimerTextBox.Text), int.Parse(_secondTimerTextBox.Text), int.Parse(_thirdTimerTextBox.Text) });
        }

        private void AddTimeOnlyCheck()
        {
            _timeOnlyCheckBox = new CheckBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y + CheckBoxHeightdifference), Size = CheckBoxSize, TabIndex = _tabIndex++, Checked = _addStringBeforeActualSoundValue };
            _timeOnlyCheckBox.CheckedChanged += TimeOnlyCheckBoxChanged;
            _currentX += _timeOnlyCheckBox.Size.Width;
            _mainWindow.Controls.Add(_timeOnlyCheckBox);
        }

        void TimeOnlyCheckBoxChanged(object sender, EventArgs e)
        {
            _alarmedEventTrigger.SetStringBeforeTimeWarning(_timeOnlyCheckBox.Checked);
        }

        private void AddFinishedMessage()
        {
            _finalMessageTextBox = new TextBox { Location = new Point(_basePoint.X + _currentX, _basePoint.Y), Size = FinalMessagesize, TabIndex = _tabIndex++, Text = _finishedMessage };
            _finalMessageTextBox.TextChanged += FinalMessageTextBoxTextChanged;
            _currentX += _finalMessageTextBox.Size.Width;
            _mainWindow.Controls.Add(_finalMessageTextBox);
        }

        void FinalMessageTextBoxTextChanged(object sender, EventArgs e)
        {
            _alarmedEventTrigger.SetFinishMessage(_finalMessageTextBox.Text);
        }

    }
}
