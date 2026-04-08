using UnityEngine;

public class AboutPanel : MonoBehaviour
{
    public void CloseAbout()
    {
        UIManager.Instance.HideAbout();
    }
}
