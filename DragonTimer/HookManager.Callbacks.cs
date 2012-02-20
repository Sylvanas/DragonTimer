using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DragonTimer
{
    public static partial class HookManager
    {
        private delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        #region Keyboard hook processing

        private static HookProc _sKeyboardDelegate;

        private static int _sKeyboardHookHandle;

        private static int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            bool handled = false;

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
                return -1;
            return CallNextHookEx(_sKeyboardHookHandle, nCode, wParam, lParam);
        }

        private static void EnsureSubscribedToGlobalKeyboardEvents()
        {
            if (_sKeyboardHookHandle == 0)
            {
                _sKeyboardDelegate = KeyboardHookProc;
                _sKeyboardHookHandle = SetWindowsHookEx(
                    WhKeyboardLl,
                    _sKeyboardDelegate,
                    Marshal.GetHINSTANCE(
                        Assembly.GetExecutingAssembly().GetModules()[0]),
                    0);
                if (_sKeyboardHookHandle == 0)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode);
                }
            }
        }

        private static void TryUnsubscribeFromGlobalKeyboardEvents()
        {
            if (SKeyDown == null)
            {
                ForceUnsunscribeFromGlobalKeyboardEvents();
            }
        }

        private static void ForceUnsunscribeFromGlobalKeyboardEvents()
        {
            if (_sKeyboardHookHandle != 0)
            {
                int result = UnhookWindowsHookEx(_sKeyboardHookHandle);
                _sKeyboardHookHandle = 0;
                _sKeyboardDelegate = null;
                if (result == 0)
                {
                    var errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode);
                }
            }
        }

        #endregion

    }
}
