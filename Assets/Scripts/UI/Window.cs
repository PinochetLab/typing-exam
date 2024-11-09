using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.UI
{
    public class Window : MonoBehaviour
    {
        [SerializeField] private WindowName windowName;
        [SerializeField] private List<GameObject> relatedGameObjects;

        public WindowName Name => windowName;
        
        public void SetActive(bool active)
        {
            relatedGameObjects.ForEach(go => go.SetActive(active));
        }
    }
}