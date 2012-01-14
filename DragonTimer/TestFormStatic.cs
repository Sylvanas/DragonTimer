using System;
using System.Windows.Forms;
using System.Timers;
using System.Speech.Synthesis;

namespace DragonTimer
{

    ///<summary>
    ///</summary>
    public partial class TestFormStatic : Form
    {
        private readonly SpeechSynthesizer _speech;

        ///<summary>
        ///</summary>
        public TestFormStatic()
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
        const int DragonRespawnSeconds = 360;
        #endregion

        #region timer
        /// <summary>
        /// timer declared and timer settings
        /// </summary>
        static readonly System.Timers.Timer Timer = new System.Timers.Timer();
        const int LowestFrequency = 2500;
        static int _interval = (DragonRespawnSeconds - 120);
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


        private void Form1Load(object sender, EventArgs e)
        {
            HookManager.KeyDown += HookManagerKeyDown;
            Timer.Elapsed += OnTimedEvent;
        }

        private void HookManagerKeyDown(object sender, KeyEventArgs e)
        {
            for (var i = 1; i < _keysPressed.Length; i++)
            {
                _keysPressed[i - 1] = _keysPressed[i];
            }

            _keysPressed[_keysPressed.Length - 1] = e.KeyCode;

            bool checkDragonKill = true;
            bool checkDragonCheck = true;
            for (var i = 0; i < KeyComb.Length; i++)
            {
                // check for Dragon Kill Comb
                if (KeyComb[i] != _keysPressed[i])
                {
                    checkDragonKill = false;
                }

                // check for Dragon Check Comb
                if (VoiceKeyComb[i] != _keysPressed[i])
                {
                    checkDragonCheck = false;
                }
            }

            // check if Dragon Kill Comb was pressed
            if (checkDragonKill)
            {
                _keysPressed = new[] { Keys.Space, Keys.Space };
                if (_interval == 0)
                    Timer.Interval = 1;
                else
                    Timer.Interval = _interval * 1000;
                _count = 0;
                _dragonKilledTime = DateTime.Now;

                Timer.Start();
            }

            // check if Dragon Check Comb was pressed
            if (checkDragonCheck)
            {
                if (_dragonKilledTime == null)
                {
                    // zi ca "Dragon is up"
                    _speech.SpeakAsync("Dragon is up!");
                }
                else
                {
                    TimeSpan dragonTimeSpan = Convert.ToDateTime(_dragonKilledTime).AddSeconds(DragonRespawnSeconds) - DateTime.Now;
                    if (dragonTimeSpan.TotalMilliseconds <= 0)
                    {
                        // zi ca "Dragon is up"
                        _speech.SpeakAsync("Dragon is up!");
                        _dragonKilledTime = null;
                    }
                    else
                    {
                        int minutes = dragonTimeSpan.Minutes;
                        int seconds = dragonTimeSpan.Seconds;
                        _speech.SpeakAsync(String.Format("Dragon in {0} minutes and {1} seconds", minutes, seconds));
                        // rosteste minutes si seconds
                    }
                }                
            }
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            TimerEvent();
        }

        private static void TimerEvent()
        {
            Timer.Stop();
            Console.Beep(LowestFrequency + _count * 500, 400);
            Console.Beep(LowestFrequency + _count * 500, 400);
            Console.Beep(LowestFrequency + _count * 500, 400);
            //simpleSound.Play(); 

            if (_interval < DragonRespawnSeconds)
            {
                _interval += 30;
                _count++;
                Timer.Interval = AlertInterval * 1000;
                Timer.Start();
            }             
        }
    }
}