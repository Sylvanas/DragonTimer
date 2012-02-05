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
        protected Keys[] KeyCombinationStart;
        protected Keys[] KeyCombinationAction;
        protected int FirstIntervalSeconds;
        protected int OtherIntervalsSeconds;
        protected bool UseOtherIntervals;
        protected int _elapsedSeconds;
        protected int RespawnSeconds;
        protected int FirstInterval;
        private string finishedMessage;

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
        public BaseEventTrigger(Keys[] keyCombinationStart, Keys[] keyCombinationAction, int respawnSecondsValue, int firstIntervalSecondsValue, int otherIntervalsSecondsValue, bool useOtherIntervalsValue, string finishedMessageValue)
        {
            KeyCombinationStart = keyCombinationStart;
            KeyCombinationAction = keyCombinationAction;
            FirstIntervalSeconds = firstIntervalSecondsValue;
            OtherIntervalsSeconds = otherIntervalsSecondsValue;
            UseOtherIntervals = useOtherIntervalsValue;
            RespawnSeconds = respawnSecondsValue;
            finishedMessage = finishedMessageValue;
            InitializeOtherNonParametricObjects();
            Timer.Elapsed += OnTimedEvent;
        }

        ///<summary>
        ///</summary>
        ///<param name="keyCombination"></param>
        public void CheckShortcuts(Keys[] keyCombination)
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
            Timer.Interval = FirstInterval * 1000;
            _elapsedSeconds = 0;
        }

        private static bool CheckShortcut(Keys[] imputKeyCombination, Keys[] keyCombination)
        {
            if (imputKeyCombination.Length < keyCombination.Length)
            {
                return false;
            }
            for (int i = 1; i <= keyCombination.Length; i++)
            {
                if (keyCombination[keyCombination.Length - i] != imputKeyCombination[imputKeyCombination.Length - i])
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
                Speech.SpeakAsync(finishedMessage);
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
    }
}
