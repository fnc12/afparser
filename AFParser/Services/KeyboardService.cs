using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace AFParser.Services
{
    public class KeyboardService
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag
        public const int VK_LCONTROL = 0xA2; //Left Control key code
        public const int A = 0x41; //A key code
        public const int C = 0x43; //C key code
        public const byte VK_CONTROL = 0x11;
        private const byte VK_TAB = 0x09;
        const int VK_SPACE = 0x20;

        public KeyboardService()
        {
            //..
        }

        public void Space()
        {
            keybd_event(VK_SPACE, 0x9d, 0, 0);
            keybd_event(VK_SPACE, 0x9d, KEYEVENTF_KEYUP, 0);
        }

        public void Tab()
        {
            keybd_event(VK_TAB, 0x9d, 0, 0);
            keybd_event(VK_TAB, 0x9d, KEYEVENTF_KEYUP, 0);
        }

        public void Ctrl()
        {
            /*keybd_event(VK_LCONTROL, 0, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_LCONTROL, 0, KEYEVENTF_KEYUP, 0);*/
        }

        public void CtrlC()
        {
            // Hold Control down and press C
            keybd_event(VK_CONTROL, 0x9d, 0, 0);
            Thread.Sleep(150);
            keybd_event(C, 0x9d, 0, 0);
            Thread.Sleep(150);
            keybd_event(C, 0x9d, KEYEVENTF_KEYUP, 0);
            Thread.Sleep(150);
            keybd_event(VK_CONTROL, 0x9d, KEYEVENTF_KEYUP, 0);
            Thread.Sleep(150);
        }

        public void CtrlUp()
        {
            keybd_event(VK_CONTROL, 0x9d, KEYEVENTF_KEYUP, 0);
        }

        public void CtrlA()
        {
            // Hold Control down and press A
            keybd_event(VK_CONTROL, 0x9d, 0, 0);
            Thread.Sleep(150);
            keybd_event(A, 0x9d, 0, 0);
            Thread.Sleep(150);
            keybd_event(A, 0x9d, KEYEVENTF_KEYUP, 0);
            Thread.Sleep(150);
            keybd_event(VK_CONTROL, 0x9d, KEYEVENTF_KEYUP, 0);
            Thread.Sleep(150);
        }
    }
}
