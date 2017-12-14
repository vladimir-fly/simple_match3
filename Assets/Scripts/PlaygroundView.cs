using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

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

            _playground.OnPlaygroundUpdated += UpdatePlaygroundView;
            
            // Initialization background
            _backgroundPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            _backgroundPlane.transform.SetParent(transform);
            _backgroundPlane.transform.localScale = new Vector3(playground.Width, 1, playground.Height);
            _backgroundPlane.transform.rotation = new Quaternion(0, -90, 90, 0);
            _backgroundPlane.GetComponent<MeshRenderer>().material = 
                new Material(Shader.Find(Defaults.BackgroundPlaneShader)) { color = Color.black };

            // Initialization game elements pallete
            _colors = new List<Color>();
            _colors.Add(Color.black);
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

            int index;

            playground.ToList().ForEach(element =>
            {   
                var @object = GameObject.CreatePrimitive(PrimitiveType.Cube);
                var elementComponent = @object.AddComponent<Element>();

                elementComponent.OnTrySwap += TrySwapElements;
                @object.transform.SetParent(_elements.transform);
                index = @object.transform.GetSiblingIndex();
                @object.GetComponent<MeshRenderer>().material = 
                    new Material(Shader.Find(Defaults.BackgroundPlaneShader)) { color = _colors[playground[index]] };
                
                var x = (index * 2) % (2 * playground.Width) - playground.Width;
                var y = playground.Height - (index / playground.Width * 2) % (playground.Height * 2);

                @object.name = $"{index}";
                @object.transform.localPosition = new Vector3(x, y, -1);
            });
        }

        private void TrySwapElements(Element source, Element target)
        {
            var sourceIndex = int.Parse(source.name);
            var targetIndex = int.Parse(target.name);

            var canSwap = _playground.CanSwap(sourceIndex, targetIndex);
            print(PrettyLog.GetMessage($"Opportunity to swap {sourceIndex} with {targetIndex} is {canSwap}"));
            if (canSwap)
                StartCoroutine(MakeSwap(sourceIndex, targetIndex));
        }

        private IEnumerator MakeSwap(int sourceIndex, int targetIndex)
        {
            _playground.Swap(sourceIndex, targetIndex);
            yield return new WaitForSeconds(0.5f);

            _playground.CleanAt(sourceIndex);
            yield return new WaitForSeconds(0.5f);
            
            _playground.CleanAt(targetIndex);
            yield return new WaitForSeconds(1f);

            _playground.Rearrange();
            yield return new WaitForSeconds(1f);
            
            _playground.Fill();
            yield return null;
        }

        private void UpdatePlaygroundView(List<Tuple<int, byte>> changedElements)
        {
            changedElements.ForEach(element =>
            {
                var @object = _elements.transform.GetChild(element.Item1);
                @object.GetComponent<MeshRenderer>().material.color = _colors[element.Item2] ;
            });
        }
    }
}