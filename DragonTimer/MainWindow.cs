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
            _alarmedDragonEvent = new AlarmedEventTrigger("Dragon", new List<Keys> { Keys.LControlKey, Keys.Oemtilde }, new List<Keys> { Keys.LControlKey, Keys.Z }, 19, new List<int> { 15, 10, 5 }, 30, true,
                 new List<bool> { true, true, true, true }, "Dragon", false, "Dragon is up", this);
            _appEvents.Add(_alarmedDragonEvent);
        }

        static readonly List<Keys> KeysPressed = new List<Keys> { Keys.Space, Keys.Space };
        //private EventTrigger _dragonEvent;
        private readonly AlarmedEventTrigger _alarmedDragonEvent;
        private readonly List<BaseEventTrigger> _appEvents = new List<BaseEventTrigger>();

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

        #region
        private List<int> SetTaskValues()
        {
            try
            {
            }
            catch
            {
                label8.Text = "Please set valid numbers for timer warnings";
                return null;
            }
            return new List<int>();
        }
        #endregion

        private void ActivatePressed(object sender, EventArgs e)
        {
            if (SetTaskValues() != null)
            {
                
            }
        }

        private void ClosePressed(object sender, EventArgs e)
        {
            Close();
        }
    }
}