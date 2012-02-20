using System.ComponentModel;
using System.Windows.Forms;

namespace DragonTimer
{
    public class GlobalEventProvider : Component
    {
        protected override bool CanRaiseEvents
        {
            get
            {
                return true;
            }
        }

        private event KeyEventHandler MKeyDown;

        public event KeyEventHandler KeyDown
        {
            add
            {
                if (MKeyDown == null)
                {
                    HookManager.KeyDown += HookManagerKeyDown;
                }
                MKeyDown += value;
            }
            remove
            {
                MKeyDown -= value;
                if (MKeyDown == null)
                {
                    HookManager.KeyDown -= HookManagerKeyDown;
                }
            }
        }

        private void HookManagerKeyDown(object sender, KeyEventArgs e)
        {
            MKeyDown.Invoke(this, e);
        }       
    }
}
