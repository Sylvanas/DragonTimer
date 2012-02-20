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
            //indicates if any of underlaing events set e.Handled flag
            bool handled = false;

            if (nCode >= 0)
            {
                //read structure KeyboardHookStruct at lParam
                var myKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                //raise KeyDown
                if (SKeyDown != null && (wParam == WmKeydown || wParam == WmSyskeydown))
                {
                    var keyData = (Keys)myKeyboardHookStruct.VirtualKeyCode;
                    var e = new KeyEventArgs(keyData);
                    SKeyDown.Invoke(null, e);
                    handled = e.Handled;
                }

                if (SKeyPress != null && wParam == WmKeydown)
                {
                    bool isDownShift = ((GetKeyState(VkShift) & 0x80) == 0x80 ? true : false);
                    bool isDownCapslock = (GetKeyState(VkCapital) != 0 ? true : false);

                    var keyState = new byte[256];
                    GetKeyboardState(keyState);
                    var inBuffer = new byte[2];
                    if (ToAscii(myKeyboardHookStruct.VirtualKeyCode,
                              myKeyboardHookStruct.ScanCode,
                              keyState,
                              inBuffer,
                              myKeyboardHookStruct.Flags) == 1)
                    {
                        var key = (char)inBuffer[0];
                        if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key)) key = Char.ToUpper(key);
                        var e = new KeyPressEventArgs(key);
                        SKeyPress.Invoke(null, e);
                        handled = handled || e.Handled;
                    }
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
            if (SKeyDown == null &&
                SKeyPress == null)
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
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode);
                }
            }
        }

        #endregion

    }
}
