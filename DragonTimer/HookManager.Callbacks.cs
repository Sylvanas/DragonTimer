using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DragonTimer
{
    public static partial class HookManager
    {
        private delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        private static HookProc _sKeyboardDelegate;

        private static int _sKeyboardHookHandle;

        private static int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            var handled = false;
            if (nCode >= 0)
            {
                var myKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                if (SKeyDown != null && (wParam == WmKeydown || wParam == WmSyskeydown))
                {
                    var keyData = (Keys)myKeyboardHookStruct.VirtualKeyCode;
                    var e = new KeyEventArgs(keyData);
                    SKeyDown.Invoke(null, e);
                    handled = e.Handled;
                }
            }
            if (handled)
            {
                return -1;
            }
            return CallNextHookEx(_sKeyboardHookHandle, nCode, wParam, lParam);
        }

        private static void EnsureSubscribedToGlobalKeyboardEvents()
        {
            if (_sKeyboardHookHandle != 0)
            {
                return;
            }
            _sKeyboardDelegate = KeyboardHookProc;
            _sKeyboardHookHandle = SetWindowsHookEx(
                WhKeyboardLl,
                _sKeyboardDelegate,
                Marshal.GetHINSTANCE(
                    Assembly.GetExecutingAssembly().GetModules()[0]),
                0);
        }
    }
}
