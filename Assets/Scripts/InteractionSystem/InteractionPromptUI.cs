using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private TextMeshProUGUI _promptText;
    [SerializeField] private GameObject _uiPanel;

    private void Start()
    {
        _mainCamera = Camera.main;
        _uiPanel.SetActive(false);
    }

    private void LateUpdate()
    {
        var rotation = _mainCamera.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }

    public bool IsActive = false;
    public void SetUp(string prompt)
    {
        _promptText.text = prompt;
        _uiPanel.SetActive(true);
        IsActive = true;
    }


    public void Close()
    {
        IsActive = false;
        _uiPanel.SetActive(false);
    }
}
