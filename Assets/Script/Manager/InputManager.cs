using UnityEngine;

namespace InputKey
{
    public static class InputManager
    {
        private static bool isCheck;
        public static bool GridInputKeyDown(KeyCode keyCode)
        {
            if (Input.anyKey == false) { isCheck = false; }
            if (isCheck == false)
            {
                if (Input.GetKey(keyCode))
                {
                    isCheck = true;
                    return true;
                }
            }
            return false;
        }
        public static bool GridInputKeyDown(KeyCode keyCode1,KeyCode keyCode2)
        {
            if (Input.anyKey == false) { isCheck = false; }
            if(isCheck==false)
            {
                if(Input.GetKey(keyCode1)&&Input.GetKey(keyCode2))
                {
                    isCheck = true;
                    return true;
                }
            }
            return false;
        }
    }
}
