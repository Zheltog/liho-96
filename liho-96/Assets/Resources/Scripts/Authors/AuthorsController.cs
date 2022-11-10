using Final;
using UnityEngine;

namespace Authors
{
    public class AuthorsController : MonoBehaviour
    {
        public GameObject rolesOpened;
        public GameObject rolesClosed;
        public GameObject secretGuestOpened;
        public GameObject secretGuestClosed;

        private void Start()
        {
            var isGameFinished = Final.StateHolder.CurrentState == State.Final;
            rolesOpened.SetActive(isGameFinished);
            rolesClosed.SetActive(!isGameFinished);
            secretGuestOpened.SetActive(isGameFinished);
            secretGuestClosed.SetActive(!isGameFinished);
        }
    }
}