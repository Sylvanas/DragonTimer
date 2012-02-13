using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DragonTimer
{
    ///<summary>
    ///</summary>
    public class AlarmedEventTrigger : BaseEventTrigger
    {
        private readonly List<int> _alarmTimers = new List<int>();
        private List<bool> _activateCurrentTimerTask = new List<bool>();
        private string _stringBeforeSound;
        private bool _addStringBeforeActualSound;
        private int _currentTimerTask;

        ///<summary>
        ///</summary>
        ///<param name="labelText"></param>
        ///<param name="keyCombinationStart"></param>
        ///<param name="keyCombinationAction"></param>
        ///<param name="respawnSecondsValue"></param>
        ///<param name="firstIntervalSecondsValue"></param>
        ///<param name="otherIntervalsSecondsValue"></param>
        ///<param name="useOtherIntervalsValue"></param>
        ///<param name="activateCurrentTimerTaskValue"></param>
        ///<param name="stringBeforeSoundValue"></param>
        ///<param name="addStringBeforeActualSoundValue"></param>
        ///<param name="finishedMessage"></param>
        ///<param name="mainWindow"></param>
        public AlarmedEventTrigger(string labelText, List<Keys> keyCombinationStart, List<Keys> keyCombinationAction,
            int respawnSecondsValue, List<int> firstIntervalSecondsValue, int otherIntervalsSecondsValue, bool useOtherIntervalsValue,
            List<bool> activateCurrentTimerTaskValue, string stringBeforeSoundValue, bool addStringBeforeActualSoundValue, string finishedMessage, MainWindow mainWindow)
            : base(keyCombinationStart, keyCombinationAction, respawnSecondsValue, firstIntervalSecondsValue[0], otherIntervalsSecondsValue, useOtherIntervalsValue, finishedMessage) 
        {
            _activateCurrentTimerTask = activateCurrentTimerTaskValue;
            firstIntervalSecondsValue.Add(0);
            _alarmTimers.Add(RespawnSeconds - firstIntervalSecondsValue[0]);
            for (var i = 1; i < firstIntervalSecondsValue.Count; i++)
            {
                _alarmTimers.Add(firstIntervalSecondsValue[i-1] - firstIntervalSecondsValue[i]);
            }
            _stringBeforeSound = stringBeforeSoundValue;
            _addStringBeforeActualSound = addStringBeforeActualSoundValue;
            new EventGuiBuilder(mainWindow, this, labelText, keyCombinationStart, keyCombinationAction, firstIntervalSecondsValue, activateCurrentTimerTaskValue, addStringBeforeActualSoundValue, finishedMessage);
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
            _addStringBeforeActualSound = value;
        }

        protected override void OnFirstTimedEvent()
        {
            Timer.Stop();
            ElapsedSeconds += _alarmTimers[_currentTimerTask];
            if (ElapsedSeconds != RespawnSeconds)
            {
                if (_activateCurrentTimerTask[_currentTimerTask])
                {
                    OnEventTime(RespawnSeconds - ElapsedSeconds);
                }
            }
            else
            {
                OnNullEventStartedTime(_activateCurrentTimerTask[_currentTimerTask]);
                _currentTimerTask = 0;
                return;
            }
            Timer.Interval = _alarmTimers[_currentTimerTask + 1] * 1000;
            Timer.Start();
            _currentTimerTask++;
        }

        protected override void OnEventTime(int dragonTimeSpan)
        {
            var minutesSeconds = GetMinutesAndSeconds(dragonTimeSpan);
            var stringBeforeSound = "";
            if (_addStringBeforeActualSound)
            {
                stringBeforeSound = _stringBeforeSound;
            }

            if(minutesSeconds[0] > 0)
            {
                Speech.SpeakAsync(String.Format(stringBeforeSound + " {0} minutes and {1} seconds", minutesSeconds[0], minutesSeconds[1]));
            } else
            {
                Speech.SpeakAsync(String.Format(stringBeforeSound + " {0} seconds", minutesSeconds[1]));
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="newValues"></param>
        public void SetCurrentTimerTask(List<bool> newValues)
        {
            _activateCurrentTimerTask = newValues;
        }
    }
}