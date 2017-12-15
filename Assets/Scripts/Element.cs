using System;

using UnityEngine;
using UnityEngine.EventSystems;

using SM3.Helpers;

namespace SM3
{
    public class Element : MonoBehaviour
    {
        private Vector3 _screenPoint;
        private Vector3 _startPosition;
        private bool _isDragging;
        public Action<Element, Element> OnTrySwap;

        private void OnMouseDown()
        {
            _startPosition = transform.position;
            transform.position -= new Vector3(0f, 0f, 1f);
            _screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

    #if UNITY_EDITOR
            print(PrettyLog.GetMessage(OnMouseDown, $"_startPosition = {_startPosition}"));
    #endif
        }

        private void OnMouseUp()
        {
	        if (!_isDragging) return;
			
            var upPosition = transform.position;
            transform.position = _startPosition;

    #if UNITY_EDITOR
            print(PrettyLog.GetMessage(OnMouseUp, $"transform.position = {transform.position}"));
    #endif

            if (OnTrySwap != null)
            {
                if (Input.GetMouseButtonDown(0)) return;
                
                RaycastHit hit;
                Physics.Raycast(upPosition, new Vector3(0f, 0f, 1f), out hit);
                var selectedItem = hit.transform?.gameObject.GetComponent<Element>();

    #if UNITY_EDITOR
                print(PrettyLog.GetMessage(OnMouseUp, $"selectedItem: {selectedItem}"));
    #endif
                if (this != selectedItem)
                    OnTrySwap(this, selectedItem);
            }
        }

        private void OnMouseDrag()
        {
			_isDragging = true;

            var point = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
            var position = Camera.main.ScreenToWorldPoint(point);

            var startX = _startPosition.x;
            var currentX = position.x;
            var x = currentX >= startX - Defaults.ElementOffset && currentX <= startX + Defaults.ElementOffset ? currentX :
                    currentX >= startX - Defaults.ElementOffset ? startX + Defaults.ElementOffset : startX - Defaults.ElementOffset;

            var startY = _startPosition.y;
            var currentY = position.y;
            var y = currentY >= startY - Defaults.ElementOffset && currentY <= startY + Defaults.ElementOffset ? currentY :
                    currentY >= startY - Defaults.ElementOffset ? startY + Defaults.ElementOffset : startY - Defaults.ElementOffset;

            transform.position =  new Vector3(x, y, position.z);
        }
    }
}