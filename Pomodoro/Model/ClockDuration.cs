using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Model
{
    public class ClockDuration : IClockDuration
    {
        private static ClockDuration s_defaultClockDuraton;

        public static ClockDuration Default
        {
            get 
            {
                if (s_defaultClockDuraton == null)
                    s_defaultClockDuraton = new ClockDuration();

                return s_defaultClockDuraton;
            }
        }

        public class FixedDuration
        {
            public static TimeSpan TwentyFive = new TimeSpan(hours: 0, minutes: 25, seconds: 0);
            public static TimeSpan Five = new TimeSpan(hours: 0, minutes: 5, seconds: 0);
            public static TimeSpan Fifteen = new TimeSpan(hours: 0, minutes: 15, seconds: 0);
        }

        public ClockDuration()
            : this( FixedDuration.TwentyFive,
                    FixedDuration.Five,
                    FixedDuration.Fifteen)
        {
        }
                                
        public ClockDuration(int workDuration, int shortBreak, int longBreak)
            : this( new TimeSpan(hours: 0, minutes: workDuration, seconds: 0),
                    new TimeSpan(hours: 0, minutes: shortBreak, seconds: 0),
                    new TimeSpan(hours: 0, minutes: longBreak, seconds: 0))
        {
        }

        private ClockDuration(TimeSpan workDuration, TimeSpan shortBreak, TimeSpan longBreak)
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
