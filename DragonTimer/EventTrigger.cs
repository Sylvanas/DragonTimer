using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DragonTimer
{
    ///<summary>
    ///</summary>
    public class EventTrigger : BaseEventTrigger
    {
        ///<summary>
        ///</summary>
        ///<param name="keyCombinationStart"></param>
        ///<param name="keyCombinationAction"></param>
        ///<param name="respawnSecondsValue"></param>
        ///<param name="firstIntervalSecondsValue"></param>
        ///<param name="otherIntervalsSecondsValue"></param>
        ///<param name="useOtherIntervalsValue"></param>
        ///<param name="finishedMessage"></param>
        public EventTrigger(List<Keys> keyCombinationStart, List<Keys> keyCombinationAction, int respawnSecondsValue, int firstIntervalSecondsValue, int otherIntervalsSecondsValue, bool useOtherIntervalsValue, string finishedMessage)
            : base(keyCombinationStart, keyCombinationAction, respawnSecondsValue, firstIntervalSecondsValue, otherIntervalsSecondsValue, useOtherIntervalsValue, finishedMessage) 
        { }

        protected override void OnFirstTimedEvent()
        {
            if (ElapsedSeconds == 0)
            {
                ElapsedSeconds += FirstInterval;
                Timer.Interval = OtherIntervalsSeconds * 1000;
                Timer.Start();
                OnEventTime(RespawnSeconds - FirstInterval);
                return;
            }
            ElapsedSeconds += OtherIntervalsSeconds;
            if (ElapsedSeconds < RespawnSeconds)
            {
                var timerIntervalToAdd = OtherIntervalsSeconds;
                if (RespawnSeconds - ElapsedSeconds < timerIntervalToAdd)
                {
                    timerIntervalToAdd = RespawnSeconds - ElapsedSeconds;
                }
                if (UseOtherIntervals)
                {
                    OnEventTime(RespawnSeconds - ElapsedSeconds);
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
