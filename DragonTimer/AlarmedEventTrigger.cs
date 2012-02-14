using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Windows.Forms;
using SpeechLib;

namespace DragonTimer
{
    ///<summary>
    ///</summary>
    public class AlarmedEventTrigger : BaseEventTrigger
    {
        private readonly List<int> _alarmTimers = new List<int>();
        private List<bool> _activateCurrentTimerTask = new List<bool>();
        private readonly string _stringBeforeSound;
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
                _alarmTimers.Add(firstIntervalSecondsValue[i - 1] - firstIntervalSecondsValue[i]);
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

        public bool ReadStringAmplified(string txtToRead, string wavDirectory, string finalDirectory)
        {
            string finalFileName = "myAmplifiedFile.WAV";
            string tmpFileName = "tmpHoldingFile.WAV";
            string soxEXE = @"C:\SOX\sox.exe";
            string soxArgs = "-v 3.0 ";
            SpVoice spVoice = new SpVoice();

            spVoice.Volume = 100;
            SpFileStream fileStream = new SpFileStream();
            fileStream.Open(@tmpFileName, SpeechStreamFileMode.SSFMCreateForWrite, false);
            spVoice.AudioOutputStream = fileStream;
            spVoice.Speak(txtToRead, SpeechVoiceSpeakFlags.SVSFDefault);
            fileStream.Close();
            fileStream = null;

            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo = new System.Diagnostics.ProcessStartInfo();
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.FileName = soxEXE;
                process.StartInfo.Arguments = string.Format("{0} {1} {2}",
                                         soxArgs, tmpFileName, finalFileName);
                process.Start();
                process.WaitForExit();
                int exitCode = process.ExitCode;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return false;
            }

            try
            {
                SoundPlayer simpleSound = new SoundPlayer(@finalFileName);
                simpleSound.PlaySync();
                FileInfo readFile = new FileInfo(finalFileName);
                string finalDestination = finalDirectory + "/" + readFile.Name;
                readFile.MoveTo(finalDestination);
            }
            catch (Exception e)
            {
                string errmsg = e.Message;
                return false;
            }
            finalFileName = "";
            tmpFileName = "";
            spVoice = null;
            return true;
        }
    }
}