using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SimulationMouse
{
    class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Int32 SendInput(Int32 cInputs, ref INPUT pInputs, Int32 cbSize);

        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 28)]
        public struct INPUT
        {
            [FieldOffset(0)]
            public INPUTTYPE dwType;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBOARDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MOUSEINPUT
        {
            public Int32 dx;
            public Int32 dy;
            public Int32 mouseData;
            public MOUSEFLAG dwFlags;
            public Int32 time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct KEYBOARDINPUT
        {
            public Int16 wVk;
            public Int16 wScan;
            public KEYBOARDFLAG dwFlags;
            public Int32 time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct HARDWAREINPUT
        {
            public Int32 uMsg;
            public Int16 wParamL;
            public Int16 wParamH;
        }

        public enum INPUTTYPE : int
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }

        [Flags()]
        public enum MOUSEFLAG : int
        {
            MOVE = 0x1,
            LEFTDOWN = 0x2,
            LEFTUP = 0x4,
            RIGHTDOWN = 0x8,
            RIGHTUP = 0x10,
            MIDDLEDOWN = 0x20,
            MIDDLEUP = 0x40,
            XDOWN = 0x80,
            XUP = 0x100,
            VIRTUALDESK = 0x400,
            WHEEL = 0x800,
            ABSOLUTE = 0x8000
        }

        [Flags()]
        public enum KEYBOARDFLAG : int
        {
            EXTENDEDKEY = 1,
            KEYUP = 2,
            UNICODE = 4,
            SCANCODE = 8
        }

        static public void LeftDown()
        {
            INPUT leftDown = new INPUT();

            leftDown.dwType = 0;
            leftDown.mi = new MOUSEINPUT();
            leftDown.mi.dwExtraInfo = IntPtr.Zero;
            leftDown.mi.dx = p.X;
            leftDown.mi.dy = p.Y;
            leftDown.mi.time = 0;
            leftDown.mi.mouseData = 0;
            leftDown.mi.dwFlags = MOUSEFLAG.LEFTDOWN;

            SendInput(1, ref leftDown, Marshal.SizeOf(typeof(INPUT)));
        }

        static public void LeftUp()
        {
            INPUT leftup = new INPUT();

            leftup.dwType = 0;
            leftup.mi = new MOUSEINPUT();
            leftup.mi.dwExtraInfo = IntPtr.Zero;
            leftup.mi.dx = p.X;
            leftup.mi.dy = p.Y;
            leftup.mi.time = 0;
            leftup.mi.mouseData = 0;
            leftup.mi.dwFlags = MOUSEFLAG.LEFTUP;

            SendInput(1, ref leftup, Marshal.SizeOf(typeof(INPUT)));
        }

        static bool isStopClick = false;
        static System.Drawing.Point p;
        static void Main(string[] args)
        {
            
            bool isFinish = false;
            string command = "";
            while (!isFinish)
            {
                command = Console.ReadLine();
                switch(command)
                {
                    case "close":
                        isFinish = true;
                        break;
                    case "start":
                        p = Cursor.Position;
                        Console.WriteLine("Mouse Position: " + p.X + " " + p.Y);
                        
                        Thread thread = new Thread(new ThreadStart(HandleMouseClick));
                        thread.IsBackground = true;
                        thread.Start();
                        
                        isStopClick = false;
                        break;
                    case "stop":
                        isStopClick = true;
                        break;
                }
            }
        }

        private static void HandleMouseClick()
        {
            while(!isStopClick)
            {              
                //大蛇等級畫面
                LeftDown();
                Thread.Sleep(50);
                LeftUp();

                Thread.Sleep(1000);
            }
        }
    }
}
