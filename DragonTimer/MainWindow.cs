using System;
using System.Windows.Forms;
using System.Timers;
using System.Speech.Synthesis;
namespace DragonTimer
{

    ///<summary>
    ///</summary>
    public partial class MainWindow : Form
    {
        private readonly SpeechSynthesizer _speech;

        ///<summary>
        ///</summary>
        public MainWindow()
        {
            InitializeComponent();
            _speech = new SpeechSynthesizer {Volume = 100, Rate = -2};
        }        

        #region key combinations
        /// <summary>
        /// Key combinations
        /// </summary>
        static readonly Keys[] KeyComb = new[] { Keys.LControlKey, Keys.Oemtilde };
        static readonly Keys[] VoiceKeyComb = new[] { Keys.LControlKey, Keys.Z };
        #endregion

        #region Jungle static variables
        /// <summary>
        /// Jungle static variables
        /// </summary>
        const int DragonRespawnSeconds = 36;
        #endregion

        #region timer
        /// <summary>
        /// timer declared and timer settings
        /// </summary>
        static readonly System.Timers.Timer Timer = new System.Timers.Timer();
        const int LowestFrequency = 2500;
        static int _interval = (DragonRespawnSeconds - 12);
        const int AlertInterval = 30;
        #endregion

        #region static variables
        /// <summary>
        /// other static variables
        /// </summary>
        static Keys[] _keysPressed = new[] { Keys.Space, Keys.Space };
        static int _count;
        static DateTime? _dragonKilledTime;
        //static SoundPlayer _simpleSound = new SoundPlayer("sound.WAV");
        #endregion

        private EventTrigger dragonEvent;

        private void Form1Load(object sender, EventArgs e)
        {
            HookManager.KeyDown += HookManagerKeyDown;
            Timer.Elapsed += OnTimedEvent;
            dragonEvent = new EventTrigger(new[] { Keys.LControlKey, Keys.Oemtilde }, new[] { Keys.LControlKey, Keys.Z }, 130, 120, 30, true);
        }

        private void HookManagerKeyDown(object sender, KeyEventArgs e)
        {
            for (var i = 1; i < _keysPressed.Length; i++)
            {
                _keysPressed[i - 1] = _keysPressed[i];
            }

            _keysPressed[_keysPressed.Length - 1] = e.KeyCode;
            dragonEvent.CheckShortcuts(_keysPressed);
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            TimerEvent();
        }

        private static void TimerEvent()
        {
            Timer.Stop();
            //Console.Beep(LowestFrequency + _count * 500, 400);
            //Console.Beep(LowestFrequency + _count * 500, 400);
            //Console.Beep(LowestFrequency + _count * 500, 400);
            //simpleSound.Play(); 

            if (_interval < DragonRespawnSeconds)
            {
                _interval += 3;
                _count++;
                Timer.Interval = AlertInterval * 1000;
                Timer.Start();
            }             
        }
    }
}