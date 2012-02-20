using System.Windows.Forms;

namespace DragonTimer {

    public static partial class HookManager
    {
        private static event KeyPressEventHandler SKeyPress;

        public static event KeyPressEventHandler KeyPress
        {
            add
            {
                EnsureSubscribedToGlobalKeyboardEvents();
                SKeyPress += value;
            }
            remove
            {
                SKeyPress -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }

        private static event KeyEventHandler SKeyDown;

        public static event KeyEventHandler KeyDown
        {
            add
            {
                EnsureSubscribedToGlobalKeyboardEvents();
                SKeyDown += value;
            }
            remove
            {
                SKeyDown -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }
    }
}
