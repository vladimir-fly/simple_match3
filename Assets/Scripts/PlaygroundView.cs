using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using SM3.Helpers;

namespace SM3
{
    public class PlaygroundView : MonoBehaviour
    {
        private Playground _playground;
        private GameObject _backgroundPlane;
        private List<Color> _colors;
        private GameObject _elements;

        public void Init(Playground playground)
        {
            print(PrettyLog.GetMessage(typeof(PlaygroundView).GetMethod(nameof(Init)), playground.ToString())); // =)

            _playground = playground;

            // Initialization background
            _backgroundPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            _backgroundPlane.transform.SetParent(transform);
            _backgroundPlane.transform.localScale = new Vector3(playground.Width, 1, playground.Height);
            _backgroundPlane.transform.rotation = new Quaternion(0, -90, 90, 0);
            _backgroundPlane.GetComponent<MeshRenderer>().material = 
                new Material(Shader.Find(Defaults.BackgroundPlaneShader)) { color = Color.black };

            // Initialization game elements pallete
            _colors = new List<Color>();
            _colors.Add(Color.white);
            print(PrettyLog.GetMessage(Color.white, "is color of empty element 0"));

            for (var i = 1; i <= playground.ElementTypesCount; i++)
            {
                var color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                _colors.Add(color);
                
                print(PrettyLog.GetMessage(color, $"is color of element {i}"));
            }

            // Initialization game elements
            _elements = new GameObject(Defaults.ElementsGameObjectName);
            _elements.transform.SetParent(transform);

            playground.ToList().ForEach(element =>
            {   
                var @object = GameObject.CreatePrimitive(PrimitiveType.Cube);
                var elementComponent = @object.AddComponent<Element>();
                elementComponent.OnTrySwap += TrySwapElements;

                var colorIndex = new System.Random().Next(1, _colors.Count);
                @object.GetComponent<MeshRenderer>().material = 
                    new Material(Shader.Find(Defaults.BackgroundPlaneShader)) { color = _colors[colorIndex] };
                @object.transform.SetParent(_elements.transform);

                var index = @object.transform.GetSiblingIndex();
                var x = (index * 2) % (2 * playground.Width) - playground.Width;
                var y = playground.Height - ((index * 2) % (playground.Height * 2));

                @object.name = $"{index}";
                @object.transform.localPosition = new Vector3(x, y, -1);
            });
        }

        private void TrySwapElements(Element source, Element target)
        {
            var sourceId = int.Parse(source.name);
            var targetId = int.Parse(target.name);

            var canSwap = _playground.CanSwap(sourceId, targetId);
            print(PrettyLog.GetMessage($"Opportunity to swap {sourceId} with {targetId} is {canSwap}"));
            if (canSwap)
                _playground.Swap(sourceId, targetId);
        }
    }
}