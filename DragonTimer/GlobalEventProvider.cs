using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DragonTimer
{
    /// <summary>
    /// This component monitors all mouse activities globally (also outside of the application) 
    /// and provides appropriate events.
    /// </summary>
    public class GlobalEventProvider : Component
    {
        /// <summary>
        /// This component raises events. The value is always true.
        /// </summary>
        protected override bool CanRaiseEvents
        {
            get
            {
                return true;
            }
        }

        //################################################################
        #region Keyboard events

        private event KeyPressEventHandler m_KeyPress;

        /// <summary>
        /// Occurs when a key is pressed.
        /// </summary>
        /// <remarks>
        /// Key events occur in the following order: 
        /// <list type="number">
        /// <item>KeyDown</item>
        /// <item>KeyPress</item>
        /// <item>KeyUp</item>
        /// </list>
        ///The KeyPress event is not raised by noncharacter keys; however, the noncharacter keys do raise the KeyDown and KeyUp events. 
        ///Use the KeyChar property to sample keystrokes at run time and to consume or modify a subset of common keystrokes. 
        ///To handle keyboard events only in your application and not enable other applications to receive keyboard events, 
        /// set the KeyPressEventArgs.Handled property in your form's KeyPress event-handling method to <b>true</b>. 
        /// </remarks>
        public event KeyPressEventHandler KeyPress
        {
            add
            {
                if (m_KeyPress==null)
                {
                    HookManager.KeyPress +=HookManager_KeyPress;
                }
                m_KeyPress += value;
            }
            remove
            {
                m_KeyPress -= value;
                if (m_KeyPress == null)
                {
                    HookManager.KeyPress -= HookManager_KeyPress;
                }
            }
        }

        void HookManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (m_KeyPress != null)
            {
                m_KeyPress.Invoke(this, e);
            }
        }

        private event KeyEventHandler m_KeyUp;

        /// <summary>
        /// Occurs when a key is released. 
        /// </summary>
        public event KeyEventHandler KeyUp
        {
            add
            {
                if (m_KeyUp == null)
                {
                    HookManager.KeyUp += HookManager_KeyUp;
                }
                m_KeyUp += value;
            }
            remove
            {
                m_KeyUp -= value;
                if (m_KeyUp == null)
                {
                    HookManager.KeyUp -= HookManager_KeyUp;
                }
            }
        }

        private void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            if (m_KeyUp != null)
            {
                m_KeyUp.Invoke(this, e);
            }
        }

        private event KeyEventHandler m_KeyDown;

        /// <summary>
        /// Occurs when a key is preseed. 
        /// </summary>
        public event KeyEventHandler KeyDown
        {
            add
            {
                if (m_KeyDown == null)
                {
                    HookManager.KeyDown += HookManager_KeyDown;
                }
                m_KeyDown += value;
            }
            remove
            {
                m_KeyDown -= value;
                if (m_KeyDown == null)
                {
                    HookManager.KeyDown -= HookManager_KeyDown;
                }
            }
        }

        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            m_KeyDown.Invoke(this, e);
        }

        #endregion

        
    }
}
