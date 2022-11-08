using UnityEngine;

namespace Boss
{
    public class CursorController : MonoBehaviour
    {
        public Texture2D cursorDefault;
        public Texture2D cursorAim;
        public int aimAreaCenterX = 320;
        public int aimAreaCenterY = 320;
        public int aimAreaWith = 575;
        public int aimAreaHeight = 245;
        public int cursorSize = 20;

        private Texture2D _currentCursor;
        private Rect _aimRect;

        private void Start() {
            Cursor.visible = false;
            _currentCursor = cursorDefault;
            _aimRect = new Rect(
                aimAreaCenterX - aimAreaWith / 2,
                aimAreaCenterY - aimAreaHeight / 2,
                aimAreaWith,
                aimAreaHeight
            );
        }

        private void Update()
        {
            var mousePosition = Input.mousePosition;
            var mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);
            _currentCursor = _aimRect.Contains(mousePosition2D) ? cursorAim : cursorDefault;
        }

        public void OnGUI()
        {
            var cursorPosition = CalculateCursorPosition();
            GUI.Label(
                new Rect(cursorPosition.x, cursorPosition.y, cursorSize, cursorSize),
                _currentCursor
            );
        }

        private Vector2 CalculateCursorPosition()
        {
            var mousePosition = Event.current.mousePosition;
            return _currentCursor == cursorDefault ?
                new Vector2(mousePosition.x, mousePosition.y) :
                new Vector2(mousePosition.x - cursorSize / 2, mousePosition.y - cursorSize / 2);
        }
    }
}