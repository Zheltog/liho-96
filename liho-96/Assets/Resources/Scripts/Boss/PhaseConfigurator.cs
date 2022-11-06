using Common;
using UnityEngine;

namespace Boss
{
    public class PhaseConfigurator : MonoBehaviour
    {
        public TextBoxController text;

        private MainController _mainController;

        private void Start()
        {
            _mainController = GetComponent<MainController>();
        }

        public void NewPhase()
        {
            var phase = StateHolder.NextPhase();
            text.NewText(phase.StartText);
            _mainController.ApplyEffect(phase.Effect);
        }
    }
}