using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ZeroLockCheck : MonoBehaviour
{
    [SerializeField] private Button lockCheck;
    [SerializeField] private TMP_Text[] lockCheckText;
    private bool isLocked = true;
    void Awake()
    {
        lockCheck.onClick.AddListener(ToggleLock);
        SetButton();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ToggleLock();
        }
    }
    private void ToggleLock()
    {
        isLocked = !isLocked;
        SetButton();
    }
    public bool IsLocked()
    {
        return isLocked;
    }
    public void SetButton()
    {
        foreach (TMP_Text text in lockCheckText)
        {
            text.fontStyle = isLocked ? FontStyles.Normal : FontStyles.Italic;
        }
    }
}
