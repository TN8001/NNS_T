using System;
using System.Windows.Threading;

namespace NNS_T.Utility
{
    ///<summary>DispatcherTimerにポーズ機能つけただけ</summary>
    public class Timer : DispatcherTimer
    {
        public new TimeSpan Interval
        {
            get => base.Interval;
            set
            {
                base.Interval = value;
                baseInterval = value;
            }
        }

        private DateTime startTime;
        private TimeSpan baseInterval;


        public Timer() => Tick += TimerTick;

        ///<summary>リセットしてスタート</summary>
        public new void Start()
        {
            base.Interval = baseInterval;
            startTime = DateTime.Now;
            base.Start();
        }
        ///<summary>ポーズ</summary>
        public void Pause()
        {
            if(!IsEnabled) return;

            Stop();
            var i = base.Interval - (DateTime.Now - startTime);
            base.Interval = i.Ticks > 0 ? i : TimeSpan.Zero;
        }
        ///<summary>ポーズから復帰</summary>
        public void Resume()
        {
            startTime = DateTime.Now;
            base.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            startTime = DateTime.Now;
            if(base.Interval != baseInterval)
                base.Interval = baseInterval;
        }
    }
}
