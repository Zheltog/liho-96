using UnityEngine;

namespace Common
{
    public class CursorController : MonoBehaviour
    {
        public Texture2D cursor;
        public int cursorSize = 20;

        private void Start() {
            Cursor.visible = false;
        }

        public void OnGUI()
        {
            var cursorPosition = Event.current.mousePosition;
            GUI.Label(
                new Rect(cursorPosition.x, cursorPosition.y, cursorSize, cursorSize),
                cursor
            );
        }
    }
}