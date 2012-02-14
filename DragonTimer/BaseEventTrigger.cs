using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Timers;
using System.Windows.Forms;

namespace DragonTimer
{
    ///<summary>
    ///</summary>
    public class BaseEventTrigger
    {
        protected List<Keys> KeyCombinationStart;
        protected List<Keys> KeyCombinationAction;
        protected int FirstIntervalSeconds;
        protected int OtherIntervalsSeconds;
        protected bool UseOtherIntervals;
        protected int ElapsedSeconds;
        protected int RespawnSeconds;
        protected int FirstInterval;
        private string _finishedMessage;

        protected System.Timers.Timer Timer;
        protected DateTime? EventStartedTime;
        protected SpeechSynthesizer Speech;
        protected const int LowestFrequency = 2500;

        private void InitializeOtherNonParametricObjects()
        {
            Timer = new System.Timers.Timer();
            Speech = new SpeechSynthesizer { Volume = 100, Rate = -2 };
            FirstInterval = (RespawnSeconds - FirstIntervalSeconds);
        }

        ///<summary>
        ///</summary>
        ///<param name="keyCombinationStart"></param>
        ///<param name="keyCombinationAction"></param>
        ///<param name="respawnSecondsValue"></param>
        ///<param name="firstIntervalSecondsValue"></param>
        ///<param name="otherIntervalsSecondsValue"></param>
        ///<param name="useOtherIntervalsValue"></param>
        ///<param name="finishedMessageValue"></param>
        public BaseEventTrigger(List<Keys> keyCombinationStart, List<Keys> keyCombinationAction, int respawnSecondsValue, int firstIntervalSecondsValue, int otherIntervalsSecondsValue, bool useOtherIntervalsValue, string finishedMessageValue)
        {
            KeyCombinationStart = keyCombinationStart;
            KeyCombinationAction = keyCombinationAction;
            FirstIntervalSeconds = firstIntervalSecondsValue;
            OtherIntervalsSeconds = otherIntervalsSecondsValue;
            UseOtherIntervals = useOtherIntervalsValue;
            RespawnSeconds = respawnSecondsValue;
            _finishedMessage = finishedMessageValue;
            InitializeOtherNonParametricObjects();
            Timer.Elapsed += OnTimedEvent;
        }

        ///<summary>
        ///</summary>
        ///<param name="keyCombinationStart"></param>
        public void SetKeycombinationStart(List<Keys> keyCombinationStart)
        {
            KeyCombinationStart = keyCombinationStart;
        }

        ///<summary>
        ///</summary>
        ///<param name="keyCombinationAction"></param>
        public void SetKeycombinationAction(List<Keys> keyCombinationAction)
        {
            KeyCombinationAction = keyCombinationAction;
        }

        ///<summary>
        ///</summary>
        ///<param name="keyCombination"></param>
        public void CheckShortcuts(List<Keys> keyCombination)
        {
            if (CheckShortcut(keyCombination, KeyCombinationStart))
            {
                ResetEventTrigger();
                return;
            }
            if (CheckShortcut(keyCombination, KeyCombinationAction))
            {
                OnCheckEvent();
            }
        }

        private void ResetEventTrigger()
        {
            EventStartedTime = DateTime.Now;
            Timer.Start();
            Timer.Interval = 1 * 1000;
            ElapsedSeconds = 0;
        }

        private static bool CheckShortcut(List<Keys> imputKeyCombination, List<Keys> keyCombination)
        {
            if (imputKeyCombination.Count < keyCombination.Count)
            {
                return false;
            }
            for (int i = 1; i <= keyCombination.Count; i++)
            {
                if (keyCombination[keyCombination.Count - i] != imputKeyCombination[imputKeyCombination.Count - i])
                {
                    return false;
                }
            }
            return true;
        }

        protected void OnCheckEvent()
        {
            if (EventStartedTime == null)
            {
                OnNullEventStartedTime(false);
                return;
            }
            var dragonTimeSpan = Convert.ToDateTime(EventStartedTime).AddSeconds(RespawnSeconds) - DateTime.Now;
            if ((int)dragonTimeSpan.TotalSeconds > 1)
            {
                OnEventTime((int)dragonTimeSpan.TotalSeconds);
            }
        }

        protected void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            OnFirstTimedEvent();
        }

        protected void OnNullEventStartedTime(bool voice)
        {
            EventStartedTime = null;
            if (voice)
            {
                Speech.SpeakAsync(_finishedMessage);
            }
        }

        protected virtual void OnFirstTimedEvent() { }
        protected virtual void OnEventTime(int dragonTimeSpan) { }

        protected List<int> GetMinutesAndSeconds(int seconds)
        {
            var result = new List<int> { seconds / 60 };
            result.Add(seconds - result[0] * 60);
            return result;
        }

        ///<summary>
        ///</summary>
        ///<param name="finishMessage"></param>
        public void SetFinishMessage(string finishMessage)
        {
            _finishedMessage = finishMessage;
        }
    }
}
