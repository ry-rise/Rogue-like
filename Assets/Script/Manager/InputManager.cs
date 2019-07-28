using UnityEngine;


namespace InputKey
{
    public static class InputManager
    {
        private static bool isCheck;
        public static bool GridInputKeyDown(KeyCode keyCode)
        {
            if (Input.anyKey == false) { isCheck = false; }
			if(isCheck==false)
			{
				if(Input.GetKey(keyCode))
				{
					isCheck=true;
					return true;
				}
			}
			return false;
        }


    }
}
