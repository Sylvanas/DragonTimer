using System;
using System.Runtime.InteropServices;

namespace DragonTimer
{
    public static partial class HookManager
    {
        private const int WhKeyboardLl = 13;

        private const int WmKeydown = 0x100;

        private const int WmSyskeydown = 0x104;

        [DllImport("user32.dll", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(
            int idHook,
            int nCode,
            int wParam,
            IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int SetWindowsHookEx(
            int idHook,
            HookProc lpfn,
            IntPtr hMod,
            int dwThreadId);
    }
}