using System;
using System.Windows.Forms;

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

        private void Form1Load(object sender, EventArgs e)
        {
            HookManager.KeyDown += HookManagerKeyDown;
            _dragonEvent = new EventTrigger(new[] { Keys.LControlKey, Keys.Oemtilde }, new[] { Keys.LControlKey, Keys.Z }, 360, 120, 30, true);
        }

        private void HookManagerKeyDown(object sender, KeyEventArgs e)
        {
            for (var i = 1; i < KeysPressed.Length; i++)
            {
                KeysPressed[i - 1] = KeysPressed[i];
            }
            KeysPressed[KeysPressed.Length - 1] = e.KeyCode;
            _dragonEvent.CheckShortcuts(KeysPressed);
        }
    }
}