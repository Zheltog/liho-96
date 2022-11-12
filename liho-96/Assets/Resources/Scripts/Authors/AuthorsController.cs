using Common;
using UnityEngine;

namespace Authors
{
    public class AuthorsController : MonoBehaviour
    {
        public GameObject rolesOpened;
        public GameObject rolesClosed;
        public GameObject secretGuestOpened;
        public GameObject secretGuestClosed;
        public AudioSource audioPlayer;

        private void Start()
        {
            var isGameFinished = GameFinishedController.IsGameFinished;
            rolesOpened.SetActive(isGameFinished);
            rolesClosed.SetActive(!isGameFinished);
            secretGuestOpened.SetActive(isGameFinished);
            secretGuestClosed.SetActive(!isGameFinished);

            if (isGameFinished)
            {
                audioPlayer.Play();
            }
        }
    }
}