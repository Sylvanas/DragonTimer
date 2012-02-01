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
        public EventTrigger(Keys[] keyCombinationStart, Keys[] keyCombinationAction, int respawnSecondsValue, int firstIntervalSecondsValue, int otherIntervalsSecondsValue, bool useOtherIntervalsValue)
            : base(keyCombinationStart, keyCombinationAction, respawnSecondsValue, firstIntervalSecondsValue, otherIntervalsSecondsValue, useOtherIntervalsValue) 
        { }



        protected override void OnNullEventStartedTime()
        {
            Speech.SpeakAsync("Dragon is up!");
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
