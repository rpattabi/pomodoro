using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Model
{
    public interface ITimerDuration
    {
        TimeSpan WorkDuration { get; set; }
        TimeSpan ShortBreak { get; set; }
        TimeSpan LongBreak { get; set; }
    }
}
