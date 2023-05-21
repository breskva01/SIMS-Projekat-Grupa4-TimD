using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace InitialProject.Application.Util
{
    public class ActionScheduler
    {
        private readonly Timer _timer;
        private readonly Action _action;

        public ActionScheduler(TimeSpan interval, Action action)
        {
            _timer = new Timer(interval.TotalMilliseconds);
            _timer.Elapsed += OnTimer_Elapsed;
            _action = action;
        }

        public void Start()
        {
            _action.Invoke();
            _timer.Start();
        }

        private void OnTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _action.Invoke();
        }
    }
}
