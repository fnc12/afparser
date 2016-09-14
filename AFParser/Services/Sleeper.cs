using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AFParser.Services
{
    public class Sleeper
    {
        Timer timer=new Timer();

        public Sleeper()
        {
            //..
        }

        public delegate void DelayDelegate();

        public void Delay(int ms,DelayDelegate delayDelegate)
        {
            timer.Stop();
            timer.Interval = ms;
            timer.Start();
            timer.Tick += (s, e) =>
            {
                //timer.Stop();
                ((System.Windows.Forms.Timer)s).Stop(); //s is the Timer
                if (delayDelegate != null)
                {
                    delayDelegate();
                }
            };
        }
    }
}
