using UnityEngine;
using UnityEngine.UI;

namespace PacMan.UI
{
    /*
     * Base button operation to allow us to do more complex tasks then what the base Buttons UnityEvent can allow us to do
     */
    [RequireComponent(typeof(Button))]
    public abstract class ButtonOperation : MonoBehaviour
    {
        private Button _button; 
        
        protected virtual void Awake()
        {
            _button = GetComponent<Button>();

            if (_button != null)
            {
                _button.onClick.AddListener(OnClicked);
            }
        }

        protected abstract void OnClicked();
    }
}