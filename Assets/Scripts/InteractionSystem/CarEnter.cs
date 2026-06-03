using rayzngames;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarEnter : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt = "Enter the car";

    public GameObject _CarCamera;
    public GameObject _PlayerCamera;
    public GameObject _PlayerPlayer;
    public GameObject _PlayerCar;
    public GameObject _CarMinimap;

    private MinimapCamera _minimap;

    private void Awake()
    {
        if (_PlayerCar != null)
            _PlayerCar.GetComponent<PrometeoCarController>().enabled = false;

        GameObject minimapGO = GameObject.FindGameObjectWithTag("Minimap");
        if (minimapGO != null)
            minimapGO.TryGetComponent(out _minimap);
    }

    private void Update()
    {
        if (_PlayerCar != null && _PlayerCar.activeSelf && Keyboard.current.f2Key.wasPressedThisFrame)
        {
            ExitCar();
        }
    }

    public string InteractionPrompt => _prompt;

    public void Interact(Interactor interactor)
    {
        _PlayerPlayer.SetActive(false);
        _CarCamera.SetActive(true);
        _PlayerCar.SetActive(true);
        _PlayerCar.GetComponent<PrometeoCarController>().enabled = true;

        // El minimapa ahora sigue al coche
        if (_minimap != null)
            _minimap.SetTarget(_PlayerCar.transform);
    }

    public void ExitCar()
    {
        _PlayerPlayer.transform.position = _PlayerCar.transform.position + _PlayerCar.transform.forward * 2f;
        _PlayerPlayer.SetActive(true);
        _CarCamera.SetActive(false);
        _PlayerCar.GetComponent<PrometeoCarController>().enabled = false;
        _PlayerCar.SetActive(false);

        // El minimapa vuelve a seguir al jugador
        if (_minimap != null)
            _minimap.SetTarget(_PlayerPlayer.transform);
    }
}