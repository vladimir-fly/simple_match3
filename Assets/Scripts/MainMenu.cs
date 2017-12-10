using System;

using UnityEngine;
using UnityEngine.UI;

namespace SM3.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Slider _nValueSlider;
        [SerializeField] private Slider _mValueSlider;
        [SerializeField] private Slider _elementTypesSlider;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Text _nValue;
        [SerializeField] private Text _mValue;
        [SerializeField] private Text _elementTypesValue
            ;

        private Action<byte, byte, byte> _startGame;

        public void Init(Action<byte, byte, byte> startGame)
        {
            _startGame = startGame;
        }

        private void Start()
        {
            _nValueSlider.wholeNumbers = true;
            _nValueSlider.minValue = byte.MinValue;
            _nValueSlider.maxValue = byte.MaxValue;
            _nValueSlider.onValueChanged.AddListener(
                @value => _nValue.text = @value.ToString());

            _mValueSlider.wholeNumbers = true;
            _mValueSlider.minValue = byte.MinValue;
            _mValueSlider.maxValue = byte.MaxValue;
            _mValueSlider.onValueChanged.AddListener(
                @value => _mValue.text = @value.ToString());

            _elementTypesSlider.wholeNumbers = true;
            _elementTypesSlider.minValue = byte.MinValue;
            _elementTypesSlider.maxValue = byte.MaxValue;
            _elementTypesSlider.onValueChanged.AddListener(
                @value => _elementTypesValue.text = @value.ToString());

            _startGameButton.onClick.AddListener(() =>
            {
                _startGame?.Invoke((byte) _nValueSlider.value, (byte) _mValueSlider.value, (byte) _elementTypesSlider.value);
                gameObject.SetActive(false);
            });
        }
    }
}