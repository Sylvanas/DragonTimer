using System.Runtime.InteropServices;

namespace DragonTimer {

    public static partial class HookManager {
        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardHookStruct
        {
            public readonly int VirtualKeyCode;
            private readonly int ScanCode;
            private readonly int Flags;
            private readonly int Time;
            private readonly int ExtraInfo;
        }
    }
}
