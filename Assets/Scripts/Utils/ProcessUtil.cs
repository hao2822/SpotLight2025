using System;
using System.Runtime.InteropServices;

public static class ProcessUtil
{
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
    [DllImport("kernel32.dll")]
    private static extern IntPtr OpenProcess(uint processAccess, bool inheritHandle, int processId);
    
    [DllImport("kernel32.dll")]
    private static extern bool TerminateProcess(IntPtr hProcess, uint exitCode);
    
    [DllImport("kernel32.dll")]
    private static extern bool CloseHandle(IntPtr hObject);
#endif
    public static void KillCurrentProcess() 
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        var process = System.Diagnostics.Process.GetCurrentProcess();
        IntPtr handle = OpenProcess(0x0001, false, process.Id); // PROCESS_TERMINATE
        TerminateProcess(handle, 0);
        CloseHandle(handle);
#endif
    }

}