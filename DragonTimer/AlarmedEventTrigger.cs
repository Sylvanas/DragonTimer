using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Speech.Synthesis;

namespace DragonTimer
{
    public class AlarmedEventTrigger : BaseEventTrigger
    {
        private List<int> alarmTimers = new List<int>();
        private List<bool> activateCurrentTimerTask = new List<bool>();
        private string stringBeforeSound;
        private bool addStringBeforeActualSound;
        private int currentTimerTask;

        public AlarmedEventTrigger(Keys[] keyCombinationStart, Keys[] keyCombinationAction,
            int respawnSecondsValue, List<int> firstIntervalSecondsValue, int otherIntervalsSecondsValue, bool useOtherIntervalsValue,
            List<bool> activateCurrentTimerTaskValue, string stringBeforeSoundValue, bool addStringBeforeActualSoundValue, string finishedMessage)
            : base(keyCombinationStart, keyCombinationAction, respawnSecondsValue, firstIntervalSecondsValue[0], otherIntervalsSecondsValue, useOtherIntervalsValue, finishedMessage) 
        {
            activateCurrentTimerTask = activateCurrentTimerTaskValue;
            firstIntervalSecondsValue.Add(0);
            alarmTimers.Add(RespawnSeconds - firstIntervalSecondsValue[0]);
            for (int i = 1; i < firstIntervalSecondsValue.Count; i++)
            {
                alarmTimers.Add(firstIntervalSecondsValue[i-1] - firstIntervalSecondsValue[i]);
            }
            stringBeforeSound = stringBeforeSoundValue;
            addStringBeforeActualSound = addStringBeforeActualSoundValue;
        }

        protected override void OnFirstTimedEvent()
        {
            Timer.Stop();
            _elapsedSeconds += alarmTimers[currentTimerTask];
            if (_elapsedSeconds != RespawnSeconds)
            {
                if (activateCurrentTimerTask[currentTimerTask])
                {
                    OnEventTime(RespawnSeconds - _elapsedSeconds);
                }
            }
            else
            {
                OnNullEventStartedTime(activateCurrentTimerTask[currentTimerTask]);
                currentTimerTask = 0;
                return;
            }
            Timer.Interval = alarmTimers[currentTimerTask + 1] * 1000;
            Timer.Start();
            currentTimerTask++;
        }

        protected override void OnEventTime(int dragonTimeSpan)
        {
            var minutesSeconds = GetMinutesAndSeconds(dragonTimeSpan);
            if(minutesSeconds[0] > 0)
            {
                Speech.SpeakAsync(String.Format("Dragon in {0} minutes and {1} seconds", minutesSeconds[0], minutesSeconds[1]));
            } else
            {
                Speech.SpeakAsync(String.Format("Dragon in {0} seconds", minutesSeconds[1]));
            }
        }

        
    }
}