using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider;
    
    private void OnEnable()
    {
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }
    
    private void OnDisable()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
        }
    }
    
    private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
    }
    
    public void CloseSettings()
    {
        UIManager.Instance.HideSettings();
    }
}
