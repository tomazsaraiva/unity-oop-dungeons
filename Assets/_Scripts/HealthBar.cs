#region Includes
using UnityEngine;
using UnityEngine.UI;
#endregion

namespace OOPDungeons
{
    public class HealthBar : MonoBehaviour
    {
        #region Variables

        private Slider _slider;

        #endregion

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        public void Increase(float amount)
        {
            _slider.value += amount;
        }
        public void Decrease(float amount)
        {
            _slider.value -= amount;
            if (_slider.value <= 0)
            {
                _slider.value = 0.1f;
            }
        }
        public void Fill()
        {
            _slider.value = 1f;
        }
    }
}