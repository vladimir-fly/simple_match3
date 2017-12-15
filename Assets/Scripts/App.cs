using System;

using UnityEngine;
using UnityEngine.Assertions;

using SM3.UI;
using SM3.Helpers;

namespace SM3
{
    public class App : MonoBehaviour
    {
        [SerializeField][Range(1, byte.MaxValue)] private byte _n;
        [SerializeField][Range(1, byte.MaxValue)] private byte _m;
        [SerializeField][Range(1, byte.MaxValue)] private byte _elementTypesCount;

        [SerializeField] private Camera _mainCamera;
        [SerializeField] private PlaygroundView _playgroundView;
        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private IngameMenu _ingameMenu;

        private Playground _playground;

        private void Start()
        {
    #if UNITY_EDITOR
            print(PrettyLog.GetMessage(Start));
    #endif
            _mainMenu.Init(StartGame);
            _ingameMenu.Init(EndGame);
            _ingameMenu.gameObject.SetActive(false);
        }

        public void StartGame(byte n, byte m, byte count)
        {
            Assert.IsTrue(count < n * m, "Element types count higher than n * m!");

            var nValue = n != 0 ? n : _n;
            var mValue = m != 0 ? m : _m;
            var typesCountValue = count != 0 ? count : _elementTypesCount;

            _playground = new Playground(nValue, mValue, typesCountValue);
            _playgroundView = new GameObject(nameof(PlaygroundView)).AddComponent<PlaygroundView>();
            _playgroundView.Init(_playground);

            _mainCamera.orthographicSize = Math.Max(n, m);
            _ingameMenu.gameObject.SetActive(true);
    #if UNITY_EDITOR
            print(PrettyLog.GetMessage(nameof(App), nameof(StartGame), $"n = {n}, m = {m}, count = {count}"));
    #endif
        }

        public void EndGame()
        {
            Destroy(_playgroundView.gameObject);
            _mainMenu.gameObject.SetActive(true);
        }
    }
}