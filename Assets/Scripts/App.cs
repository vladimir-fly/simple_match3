using System;

using UnityEngine;
using UnityEngine.Assertions;

using SM3.UI;
using SM3.Helpers;

namespace SM3
{
    public class App : MonoBehaviour
    {
        [SerializeField][Range(byte.MinValue, byte.MaxValue)] private byte _n;
        [SerializeField][Range(byte.MaxValue, byte.MaxValue)] private byte _m;
        [SerializeField][Range(byte.MaxValue, byte.MaxValue)] private byte _elementTypesCount;

        [SerializeField] private Camera _mainCamera;
        [SerializeField] private PlaygroundView _playgroundView;
        [SerializeField] private MainMenu _mainMenu;

        private Playground _playground;

        private void Start()
        {
            print(PrettyLog.GetMessage(Start));

            _playgroundView = new GameObject(nameof(PlaygroundView)).AddComponent<PlaygroundView>();
            _mainMenu.Init(StartGame);
        }

        public void StartGame(byte n, byte m, byte count)
        {
            Assert.IsTrue(count < n * m, "Element types count higher than n * m!");

            var nValue = n != 0 ? n : _n;
            var mValue = m != 0 ? m : _m;
            var typesCountValue = count != 0 ? count : _elementTypesCount;

            _playground = new Playground(nValue, mValue, typesCountValue);
            _playgroundView.Init(_playground);

            _mainCamera.orthographicSize = Math.Max(n, m);

            print(PrettyLog.GetMessage(nameof(App), nameof(StartGame), $"n = {n}, m = {m}, count = {count}"));
        }
    }
}