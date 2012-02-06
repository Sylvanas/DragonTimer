using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Speech.Synthesis;

namespace DragonTimer
{
    public class EventTrigger : BaseEventTrigger
    {
        public EventTrigger(List<Keys> keyCombinationStart, List<Keys> keyCombinationAction, int respawnSecondsValue, int firstIntervalSecondsValue, int otherIntervalsSecondsValue, bool useOtherIntervalsValue, string finishedMessage)
            : base(keyCombinationStart, keyCombinationAction, respawnSecondsValue, firstIntervalSecondsValue, otherIntervalsSecondsValue, useOtherIntervalsValue, finishedMessage) 
        { }

        protected override void OnFirstTimedEvent()
        {
            if (_elapsedSeconds == 0)
            {
                _elapsedSeconds += FirstInterval;
                Timer.Interval = OtherIntervalsSeconds * 1000;
                Timer.Start();
                OnEventTime(RespawnSeconds - FirstInterval);
                return;
            }
            _elapsedSeconds += OtherIntervalsSeconds;
            if (_elapsedSeconds < RespawnSeconds)
            {
                var timerIntervalToAdd = OtherIntervalsSeconds;
                if (RespawnSeconds - _elapsedSeconds < timerIntervalToAdd)
                {
                    timerIntervalToAdd = RespawnSeconds - _elapsedSeconds;
                }
                if (UseOtherIntervals)
                {
                    OnEventTime(RespawnSeconds - _elapsedSeconds);
                }
                Timer.Interval = timerIntervalToAdd * 1000;
                Timer.Start();
            }
            else
            {
                OnNullEventStartedTime(true);
                EventStartedTime = null;
            }
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
