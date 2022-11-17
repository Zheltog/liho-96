using UnityEngine;

namespace Boss
{
    public class CursorController : MonoBehaviour
    {
        public Texture2D cursorDefault;
        public Texture2D cursorAim;
        public int cursorSize = 20;
        public bool IsCursorInsideCrtLines { get; set; }

        private Texture2D _currentCursor;

        private void Start() {
            Cursor.visible = false;
            _currentCursor = cursorDefault;
        }

        private void Update()
        {
            _currentCursor = IsCursorInsideCrtLines ? cursorAim : cursorDefault;
        }

        public void SetCursorInsideCrtLines()
        {
            IsCursorInsideCrtLines = true;
        }

        public void SetCursorOutsideCrtLines()
        {
            IsCursorInsideCrtLines = false;
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