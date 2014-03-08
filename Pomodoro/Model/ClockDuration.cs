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

        public ClockDuration()
            : this( TimeSpan.FromMinutes(25),
                    TimeSpan.FromMinutes(5),
                    TimeSpan.FromMinutes(15))
        {
        }
                                
        public ClockDuration(   int workDuration_minutes, 
                                int shortBreak_minutes, 
                                int longBreak_minutes)
            : this( TimeSpan.FromMinutes(workDuration_minutes),
                    TimeSpan.FromMinutes(shortBreak_minutes),
                    TimeSpan.FromMinutes(longBreak_minutes))
        {
        }

        internal ClockDuration(TimeSpan workDuration, TimeSpan shortBreak, TimeSpan longBreak)
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
