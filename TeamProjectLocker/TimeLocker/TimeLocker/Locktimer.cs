﻿using System;
using System.IO;

namespace TimeLocker
{
    public sealed class Locktimer
    {
        RemainingTimeController _timeController;
		Locker _locker;

        public Locktimer(Locker locker)
        {
			_locker = locker;
			_timeController = new RemainingTimeController(LastSessionSynchronizer.GetAllowedTime());

            locker.LockStatusChanged += SessionSwitchEvent;
			locker.SystemShutdown += SaveData;
            _timeController.TimeOut += Lock;
        }

        public TimeSpan GetRemainingTime()
        {
			if (_timeController.RemaningTimeToLock < Properties.Settings.Default.MaxAllowedTime)
				return _timeController.RemaningTimeToLock;
			else
				return _timeController.RemaningTimeToLock - Properties.Settings.Default.MaxAllowedTime;
        }

        private void SaveData(object o, EventArgs e)
        {
			var remaningTimeToLock = _timeController.RemaningTimeToLock;
			if (remaningTimeToLock > Properties.Settings.Default.MaxAllowedTime)
				remaningTimeToLock -= Properties.Settings.Default.MaxAllowedTime;

			LastSessionSynchronizer.SaveSessionData(remaningTimeToLock);
        }

        private void Lock(object o, EventArgs e)
        {
			_locker.LockSystem();
        }

		private void SessionSwitchEvent(object o, LockStatusChangedEventArgs e)
        {
            if (e.Reason == LockStatusChangedReason.Lock)
                _timeController.StopTimer();
			else if (e.Reason == LockStatusChangedReason.Unlock)
                _timeController.StartTimer();
        }
    }
}
