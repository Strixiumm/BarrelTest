using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomToggle : MonoBehaviour
{
    [SerializeField]
    GameObject enabledObject;
    [SerializeField]
    GameObject disabledObject;
    Toggle.ToggleEvent toggleEvent = new Toggle.ToggleEvent();

    public bool isEnabled { get; private set; }
    Button button;


    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        isEnabled = false;
    }

    public void OnClick()
    {
        SetEnabled(!isEnabled);
    }

    public void SetToogleEvent(UnityAction<bool> onClickAction)
    {
        toggleEvent.AddListener(onClickAction);
    }
    
    public void SetEnabled(bool value)
    {
        isEnabled = value;
        enabledObject.SetActive(isEnabled);
        disabledObject.SetActive(!isEnabled);
        toggleEvent?.Invoke(isEnabled);
    }
}
