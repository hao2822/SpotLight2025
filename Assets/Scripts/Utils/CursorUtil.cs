using UnityEngine;

namespace Utils
{
    public static class CursorUtil
    {
        public static bool IsLocked;
        public static void Lock(bool v)
        {
            Debug.Log("Lock " + v);
            if (v)
            {
                Lock();
            }
            else
            {
                Unlock();
            }
        }
        public static void Lock()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            IsLocked = true;
        }

        public static void Unlock()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            IsLocked = false;
        }
    }
}