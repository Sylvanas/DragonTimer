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

        /// <summary>
        /// This field is not objectively needed but we need to keep a reference on a delegate which will be 
        /// passed to unmanaged code. To avoid GC to clean it up.
        /// When passing delegates to unmanaged code, they must be kept alive by the managed application 
        /// until it is guaranteed that they will never be called.
        /// </summary>
        private static HookProc _sKeyboardDelegate;

        /// <summary>
        /// Stores the handle to the Keyboard hook procedure.
        /// </summary>
        private static int _sKeyboardHookHandle;

        /// <summary>
        /// A callback function which will be called every Time a keyboard activity detected.
        /// </summary>
        /// <param name="nCode">
        /// [in] Specifies whether the hook procedure must process the message. 
        /// If nCode is HC_ACTION, the hook procedure must process the message. 
        /// If nCode is less than zero, the hook procedure must pass the message to the 
        /// CallNextHookEx function without further processing and must return the 
        /// value returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies whether the message was sent by the current thread. 
        /// If the message was sent by the current thread, it is nonzero; otherwise, it is zero. 
        /// </param>
        /// <param name="lParam">
        /// [in] Pointer to a CWPSTRUCT structure that contains details about the message. 
        /// </param>
        /// <returns>
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx. 
        /// If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx 
        /// and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC 
        /// hooks will not receive hook notifications and may behave incorrectly as a result. If the hook 
        /// procedure does not call CallNextHookEx, the return value should be zero. 
        /// </returns>
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

                // raise KeyPress
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

                // raise KeyUp
                if (SKeyUp != null && (wParam == WmKeyup || wParam == WmSyskeyup))
                {
                    var keyData = (Keys)myKeyboardHookStruct.VirtualKeyCode;
                    var e = new KeyEventArgs(keyData);
                    SKeyUp.Invoke(null, e);
                    handled = handled || e.Handled;
                }

            }

            //if event handled in application do not handoff to other listeners
            if (handled)
                return -1;

            //forward to other application
            return CallNextHookEx(_sKeyboardHookHandle, nCode, wParam, lParam);
        }

        private static void EnsureSubscribedToGlobalKeyboardEvents()
        {
            // install Keyboard hook only if it is not installed and must be installed
            if (_sKeyboardHookHandle == 0)
            {
                //See comment of this field. To avoid GC to clean it up.
                _sKeyboardDelegate = KeyboardHookProc;
                //install hook
                _sKeyboardHookHandle = SetWindowsHookEx(
                    WhKeyboardLl,
                    _sKeyboardDelegate,
                    Marshal.GetHINSTANCE(
                        Assembly.GetExecutingAssembly().GetModules()[0]),
                    0);
                //If SetWindowsHookEx fails.
                if (_sKeyboardHookHandle == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //do cleanup

                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        private static void TryUnsubscribeFromGlobalKeyboardEvents()
        {
            //if no subsribers are registered unsubsribe from hook
            if (SKeyDown == null &&
                SKeyUp == null &&
                SKeyPress == null)
            {
                ForceUnsunscribeFromGlobalKeyboardEvents();
            }
        }

        private static void ForceUnsunscribeFromGlobalKeyboardEvents()
        {
            if (_sKeyboardHookHandle != 0)
            {
                //uninstall hook
                int result = UnhookWindowsHookEx(_sKeyboardHookHandle);
                //reset invalid handle
                _sKeyboardHookHandle = 0;
                //Free up for GC
                _sKeyboardDelegate = null;
                //if failed and exception must be thrown
                if (result == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        #endregion

    }
}
