using System;
using System.Runtime.InteropServices;

namespace SwordArtOffline.Patches.Shared
{

    extern alias AssemblyNotice;

    public static class Win32MethodsMod
    {
        // Token: 0x06000285 RID: 645
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        // Token: 0x06000286 RID: 646
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Token: 0x06000289 RID: 649
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        // Token: 0x0600028A RID: 650
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        // Token: 0x0600028D RID: 653
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        // Token: 0x0600028E RID: 654
        [DllImport("user32.dll", SetLastError = true)]
        public static extern long GetWindowLong(IntPtr hWnd, int nIndex);

        // Token: 0x0600028F RID: 655
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        // Token: 0x06000290 RID: 656
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        // Token: 0x06000291 RID: 657
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, [MarshalAs(UnmanagedType.Bool)] bool fAttach);

        // Token: 0x06000292 RID: 658
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref uint pvParam, uint fWinIni);

        // Token: 0x06000293 RID: 659
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllowSetForegroundWindow(uint dwProcessId);

        // Token: 0x06000294 RID: 660
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // Token: 0x06000296 RID: 662 RVA: 0x0000AA27 File Offset: 0x00008E27
        public static void SetTopmostWindow(IntPtr hWnd, bool topmost)
        {
            if (IsWindow(hWnd))
            {
                SetWindowPos(hWnd, !topmost ? HWND_NOTOPMOST : HWND_TOPMOST, 0, 0, 0, 0, 3U);
            }
        }

        // Token: 0x06000297 RID: 663 RVA: 0x0000AA58 File Offset: 0x00008E58
        public static void SetForegroundWindowInternal(IntPtr hWnd)
        {
            if (IsWindow(hWnd))
            {
                uint num = 0U;
                uint num2 = 0U;
                uint num3 = 0U;
                IntPtr foregroundWindow = GetForegroundWindow();
                uint currentThreadId = GetCurrentThreadId();
                uint windowThreadProcessId = GetWindowThreadProcessId(foregroundWindow, out num3);
                if (currentThreadId != windowThreadProcessId)
                {
                    AttachThreadInput(currentThreadId, windowThreadProcessId, true);
                    SystemParametersInfo(8192U, 0U, ref num, 0U);
                    SystemParametersInfo(8193U, 0U, ref num2, 3U);
                    AllowSetForegroundWindow(uint.MaxValue);
                }
                SetForegroundWindow(hWnd);
                if (currentThreadId != windowThreadProcessId)
                {
                    SystemParametersInfo(8193U, 0U, ref num, 3U);
                    AttachThreadInput(currentThreadId, windowThreadProcessId, false);
                }
            }
        }

        // Token: 0x040001CB RID: 459
        public static readonly IntPtr HWND_TOPMOST = (IntPtr)(-1);

        // Token: 0x040001CC RID: 460
        public static readonly IntPtr HWND_NOTOPMOST = (IntPtr)(-2);
    }
}
