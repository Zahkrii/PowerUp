using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Collections;

namespace PowerUp.Tools
{
    public class Hotkey
    {
        // 热键所在窗体
        private Window _window;

        public Window KeyWindow { get => _window; set => _window = value; }

        // 窗体句柄
        private IntPtr _handle;

        public IntPtr Handle { get => _handle; set => _handle = value; }

        // 热键编号
        private int _keyID;

        public int KeyID { get => _keyID; set => _keyID = value; }

        // 热键控制键
        private uint _controlKey;

        public uint ControlKey { get => _controlKey; set => _controlKey = value; }

        // 热键主键
        private uint _mainKey;

        public uint MainKey { get => _mainKey; set => _mainKey = value; }

        private const int WM_HOTKEY = 0x0312; // 热键消息编号
        private static Hashtable KeyPair = new Hashtable(); // 热键哈希表

        public enum ControlKeyCode // 控制键编码
        {
            ALT = 0x1,
            CTRL = 0x2,
            SHIFT = 0x4,
            WIN = 0x8,
            CTRL_ALT = 0x2 + 0x1
        }

        public delegate void OnHotkeyEventHandeler(); // 热键事件委托

        public event OnHotkeyEventHandeler OnHotKey = null; // 热键事件

        /// <summary>
        /// 构造函数，注册热键
        /// </summary>
        /// <param name="win">注册窗体</param>
        /// <param name="control">控制键</param>
        /// <param name="key">主键</param>
        /// <exception cref="Exception"></exception>
        public Hotkey(Window win, ControlKeyCode control, Keys key)
        {
            KeyWindow = win;
            Handle = new WindowInteropHelper(win).Handle;

            ControlKey = (uint)control;
            MainKey = (uint)key;
            KeyID = (int)ControlKey + (int)key * 10;
            if (KeyPair.ContainsKey(KeyID))
            {
                throw new Exception("热键已经被注册!");
            }

            // 注册热键
            if (RegisterHotKey(Handle, KeyID, ControlKey, MainKey) == false)
            {
                throw new Exception("热键注册失败!");
            }

            // 消息挂钩只能连接一次!!
            if (KeyPair.Count == 0 && InstallHotKeyHook(this) == false)
            {
                throw new Exception("消息挂钩连接失败!");
            }

            // 添加热键索引
            KeyPair.Add(KeyID, this);
        }

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint controlKey, uint virtualKey);

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // 安装热键处理挂钩
        private static bool InstallHotKeyHook(Hotkey hotkey)
        {
            if (hotkey.KeyWindow == null || hotkey.Handle == IntPtr.Zero)
                return false;

            // 获得消息源
            HwndSource source = HwndSource.FromHwnd(hotkey.Handle);
            if (source == null)
                return false;

            // 挂接事件
            source.AddHook(HotkeyHook);
            return true;
        }

        // 热键处理过程
        private static IntPtr HotkeyHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_HOTKEY)
            {
                Hotkey hotkey = (Hotkey)KeyPair[(int)wParam];
                if (hotkey.OnHotKey != null)
                    hotkey.OnHotKey();
            }
            return IntPtr.Zero;
        }

        // 析构函数，解除热键
        ~Hotkey()
        {
            UnregisterHotKey(Handle, KeyID);
        }
    }
}