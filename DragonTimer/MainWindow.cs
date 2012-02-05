using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace DragonTimer
{

    ///<summary>
    ///</summary>
    public partial class MainWindow : Form
    {
        ///<summary>
        ///</summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        static readonly Keys[] KeysPressed = new[] { Keys.Space, Keys.Space };
        private EventTrigger _dragonEvent;
        private AlarmedEventTrigger _alarmedDragonEvent;
        private List<BaseEventTrigger> appEvents = new List<BaseEventTrigger>();

        private void Form1Load(object sender, EventArgs e)
        {
            HookManager.KeyDown += HookManagerKeyDown;
            //_dragonEvent = new EventTrigger(new[] { Keys.LControlKey, Keys.Oemtilde }, new[] { Keys.LControlKey, Keys.Z }, 20, 15, 5, true, "Dragon is up");
            _alarmedDragonEvent = new AlarmedEventTrigger(new[] { Keys.LControlKey, Keys.Oemtilde }, new[] { Keys.LControlKey, Keys.Z }, 19, new List<int>() { 15, 10, 5 }, 30, true,
                 new List<bool>() { true, true, true, true }, "Dragon", false, "Dragon is up");
            appEvents.Add(_alarmedDragonEvent);
        }

        private void HookManagerKeyDown(object sender, KeyEventArgs e)
        {
            for (var i = 1; i < KeysPressed.Length; i++)
            {
                KeysPressed[i - 1] = KeysPressed[i];
            }
            KeysPressed[KeysPressed.Length - 1] = e.KeyCode;
            foreach (var currentEvent in appEvents)
            {
                currentEvent.CheckShortcuts(KeysPressed);
            }
        }
    }
}