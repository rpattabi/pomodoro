using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Model
{
    public class TimerDuration : ITimerDuration
    {
        private static TimerDuration s_defaultTimerDuraton;

        public static TimerDuration Default
        {
            get 
            {
                if (s_defaultTimerDuraton == null)
                    s_defaultTimerDuraton = new TimerDuration();

                return s_defaultTimerDuraton;
            }
        }

        public class FixedDuration
        {
            public static TimeSpan TwentyFive = new TimeSpan(hours: 0, minutes: 25, seconds: 0);
            public static TimeSpan Five = new TimeSpan(hours: 0, minutes: 5, seconds: 0);
            public static TimeSpan Fifteen = new TimeSpan(hours: 0, minutes: 15, seconds: 0);
        }

        public TimerDuration()
            : this( FixedDuration.TwentyFive,
                    FixedDuration.Five,
                    FixedDuration.Fifteen)
        {
        }
                                
        public TimerDuration(int workDuration, int shortBreak, int longBreak)
            : this( new TimeSpan(hours: 0, minutes: workDuration, seconds: 0),
                    new TimeSpan(hours: 0, minutes: shortBreak, seconds: 0),
                    new TimeSpan(hours: 0, minutes: longBreak, seconds: 0))
        {
        }

        private TimerDuration(TimeSpan workDuration, TimeSpan shortBreak, TimeSpan longBreak)
        {
            this.WorkDuration = workDuration;
            this.ShortBreak = shortBreak;
            this.LongBreak = longBreak;
        }

        public TimeSpan WorkDuration { get; set; }
        public TimeSpan ShortBreak { get; set; }
        public TimeSpan LongBreak { get; set; }
    }
}
