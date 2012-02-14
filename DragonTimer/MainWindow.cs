using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace DragonTimer
{
    ///<summary>
    ///</summary>
    public partial class MainWindow : Form
    {
        static readonly List<Keys> KeysPressed = new List<Keys> { Keys.Space, Keys.Space };
        private readonly AlarmedEventTrigger _alarmedDragonEvent;
        private readonly AlarmedEventTrigger _testEvent;
        private readonly List<BaseEventTrigger> _appEvents = new List<BaseEventTrigger>();

        ///<summary>
        ///</summary>
        public MainWindow()
        {
            InitializeComponent();
            AppKeys.Initialize();
            _alarmedDragonEvent = new AlarmedEventTrigger("Dragon", new List<Keys> { Keys.LControlKey, Keys.Oemtilde }, new List<Keys> { Keys.LControlKey, Keys.Z }, 19, new List<int> { 15, 10, 5 }, 30, true,
                 new List<bool> { true, true, true, true }, "Dragon", false, "Dragon is up", this);
            _testEvent = new AlarmedEventTrigger("test", new List<Keys> { Keys.LControlKey, Keys.X }, new List<Keys> { Keys.LControlKey, Keys.Z }, 19, new List<int> { 15, 10, 5 }, 30, true,
                 new List<bool> { true, true, true }, "test", false, "test is up", this);
            _appEvents.Add(_alarmedDragonEvent);
            _appEvents.Add(_testEvent);
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            HookManager.KeyDown += HookManagerKeyDown;
        }

        private void HookManagerKeyDown(object sender, KeyEventArgs e)
        {
            for (var i = 1; i < KeysPressed.Count; i++)
            {
                KeysPressed[i - 1] = KeysPressed[i];
            }
            KeysPressed[KeysPressed.Count - 1] = e.KeyCode;
            foreach (var currentEvent in _appEvents)
            {
                currentEvent.CheckShortcuts(KeysPressed);
            }
        }

        private void ClosePressed(object sender, EventArgs e)
        {
            Close();
        }
    }
}