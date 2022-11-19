using UnityEngine;

namespace Common
{
    public static class Utils
    {
        public static string FormatButtonText(string originalText)
        {
            return "> " + originalText;
        }
        
        public static bool SpaceOrEnterPressed()
        {
            return (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return))
                   && Input.anyKeyDown
                   && !Input.GetMouseButtonDown(0)
                   && !Input.GetMouseButtonDown(1)
                   && !Input.GetMouseButtonDown(2);
        }
    }
}