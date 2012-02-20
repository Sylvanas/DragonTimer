using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DragonTimer
{
    ///<summary>
    ///</summary>
    public class AlarmedEventTrigger : BaseEventTrigger
    {
        private List<int> _alarmTimers = new List<int>();
        private List<bool> _activateCurrentTimerTask = new List<bool>();
        private readonly string _stringBeforeSound;
        //private bool _addStringBeforeActualSound;

        ///<summary>
        ///</summary>
        ///<param name="labelText"></param>
        ///<param name="keyCombinationStart"></param>
        ///<param name="keyCombinationAction"></param>
        ///<param name="respawnSecondsValue"></param>
        ///<param name="alarmTimers"></param>
        ///<param name="otherIntervalsSecondsValue"></param>
        ///<param name="useOtherIntervalsValue"></param>
        ///<param name="activateCurrentTimerTaskValue"></param>
        ///<param name="stringBeforeSoundValue"></param>
        ///<param name="timeOnly"></param>
        ///<param name="finishedMessage"></param>
        ///<param name="mainWindow"></param>
        public AlarmedEventTrigger(string labelText, List<Keys> keyCombinationStart, List<Keys> keyCombinationAction,
            int respawnSecondsValue, List<int> alarmTimers, int otherIntervalsSecondsValue, bool useOtherIntervalsValue,
            List<bool> activateCurrentTimerTaskValue, string stringBeforeSoundValue, bool timeOnly, string finishedMessage, MainWindow mainWindow)
            : base(keyCombinationStart, keyCombinationAction, respawnSecondsValue, alarmTimers[0], otherIntervalsSecondsValue, useOtherIntervalsValue, finishedMessage)
        {
            _activateCurrentTimerTask = activateCurrentTimerTaskValue;
            _alarmTimers = alarmTimers;
            _stringBeforeSound = stringBeforeSoundValue;
            //_addStringBeforeActualSound = !timeOnly;
            new EventGuiBuilder(mainWindow, this, labelText, keyCombinationStart, keyCombinationAction, alarmTimers, activateCurrentTimerTaskValue, timeOnly, finishedMessage);
        }

        ///<summary>
        ///</summary>
        ///<param name="list"></param>
        public void SetActivationOfTimerTasks(List<bool> list)
        {
            _activateCurrentTimerTask = list;
        }

        ///<summary>
        ///</summary>
        ///<param name="value"></param>
        public void SetStringBeforeTimeWarning(bool value)
        {
            //_addStringBeforeActualSound = !value;
        }

        private void CheckForAlarmTimer()
        {
            for (var i = 0; i < _alarmTimers.Count; i++)
            {
                if (_activateCurrentTimerTask[i] && (RespawnSeconds - ElapsedSeconds) == _alarmTimers[i])
                {
                    OnEventTime(_alarmTimers[i], true);
                }
            }
        }

        protected override void OnTimedEvent()
        {
            Timer.Stop();
            ElapsedSeconds++;
            if (ElapsedSeconds != RespawnSeconds)
            {
                CheckForAlarmTimer();
            }
            else
            {
                OnNullEventStartedTime(true);
                return;
            }
            Timer.Interval = 1 * 1000;
            Timer.Start();
        }

        protected override void OnEventTime(int dragonTimeSpan, bool addString)
        {
            var minutesSeconds = GetMinutesAndSeconds(dragonTimeSpan);
            var stringBeforeSound = "";
            if (addString)
            {
                stringBeforeSound = _stringBeforeSound;
            }

            if (minutesSeconds[0] > 0)
            {
                Speech.SpeakAsync(String.Format(stringBeforeSound + minutesSeconds[0] + " minutes and " + minutesSeconds[1] + " seconds"));
                //ReadStringAmplified(stringBeforeSound + minutesSeconds[0]+" minutes and "+minutesSeconds[1]+" seconds", "C:", "D:");
            }
            else
            {
                Speech.SpeakAsync(String.Format(stringBeforeSound + minutesSeconds[1] + "seconds"));
                //ReadStringAmplified(stringBeforeSound + minutesSeconds[1] + "seconds", "C:", "D:");
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="newValues"></param>
        public void SetCurrentTimerTask(List<bool> newValues)
        {
            _activateCurrentTimerTask = newValues;
        }

        ///<summary>
        ///</summary>
        ///<param name="newValues"></param>
        public void SetAlarmTimers(List<int> newValues)
        {
            _alarmTimers = newValues;
        }

    }
}