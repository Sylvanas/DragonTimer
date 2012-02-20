using System.Windows.Forms;

namespace DragonTimer {

    public static partial class HookManager
    {

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
            }
        }
    }
}
