using System;
using System.Runtime.InteropServices;

namespace SwordArtOffline.Patches.Shared {

    extern alias AssemblyNotice;

    public static class Win32MethodsMod {

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern long GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, [MarshalAs(UnmanagedType.Bool)] bool fAttach);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref uint pvParam, uint fWinIni);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllowSetForegroundWindow(uint dwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void SetTopmostWindow(IntPtr hWnd, bool topmost) {
            if (IsWindow(hWnd)) {
                SetWindowPos(hWnd, !topmost ? HWND_NOTOPMOST : HWND_TOPMOST, 0, 0, 0, 0, 3U);
            }
        }

        // Token: 0x06000297 RID: 663 RVA: 0x0000AA58 File Offset: 0x00008E58
        public static void SetForegroundWindowInternal(IntPtr hWnd) {
            if (IsWindow(hWnd)) {
                uint pvParam = 0U;
                uint pvParam2 = 0U;
                IntPtr foregroundWindow = GetForegroundWindow();
                uint currentThreadId = GetCurrentThreadId();
                uint windowThreadProcessId = GetWindowThreadProcessId(foregroundWindow, out _);
                if (currentThreadId != windowThreadProcessId) {
                    AttachThreadInput(currentThreadId, windowThreadProcessId, true);
                    SystemParametersInfo(8192U, 0U, ref pvParam, 0U);
                    SystemParametersInfo(8193U, 0U, ref pvParam2, 3U);
                    AllowSetForegroundWindow(uint.MaxValue);
                }
                SetForegroundWindow(hWnd);
                if (currentThreadId != windowThreadProcessId) {
                    SystemParametersInfo(8193U, 0U, ref pvParam, 3U);
                    AttachThreadInput(currentThreadId, windowThreadProcessId, false);
                }
            }
        }

        public static readonly IntPtr HWND_TOPMOST = (IntPtr)(-1);

        public static readonly IntPtr HWND_NOTOPMOST = (IntPtr)(-2);
    }
}
