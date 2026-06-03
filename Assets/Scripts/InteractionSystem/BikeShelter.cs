using rayzngames;
using StarterAssets;
using UnityEngine;

public class BikeShelter : MonoBehaviour, IInteractable
{
    [Header("Configuración")]
    [SerializeField] private string _promptTake = "Coger bici";
    [SerializeField] private string _promptLeave = "Dejar bici";

    private MinimapCamera _minimap;
    [SerializeField] private GameObject _bikePrefab;
    [SerializeField] private float spawnDistance = 5f;
    [SerializeField] private float spawnHeight = 3f;

    public string InteractionPrompt => _prompt;
    private string _prompt;

    private void Awake()
    {
        _prompt = _promptTake;

        GameObject minimapGO = GameObject.FindGameObjectWithTag("Minimap");
        if (minimapGO != null)
            minimapGO.TryGetComponent(out _minimap);
    }

    public void Interact(Interactor interactor)
    {
        if (interactor.GetComponent<ThirdPersonController>() != null)
        {
            TakeBike(interactor);
        }
        else if (interactor.GetComponent<BicycleVehicle>() != null)
        {
            LeaveBike(interactor);
        }
    }

    private void TakeBike(Interactor interactor)
    {
        Vector3 spawnPos = GetSpawnPos(interactor.transform);
        Quaternion spawnRot = GetSpawnRot(interactor.transform);

        GameObject bikeGO = Instantiate(_bikePrefab, spawnPos, spawnRot);

        BikeData bikeData = bikeGO.GetComponent<BikeData>();
        if (bikeData == null)
            bikeData = bikeGO.AddComponent<BikeData>();

        bikeData.player = interactor.gameObject;
        bikeData.playerCamera = interactor.GetComponentInChildren<Camera>();

        // Desactiva jugador y su cámara
        if (bikeData.playerCamera != null)
            bikeData.playerCamera.enabled = false;

        interactor.gameObject.SetActive(false);

        // El minimapa ahora sigue a la bici
        if (_minimap != null)
            _minimap.SetTarget(bikeGO.transform);

        _prompt = _promptLeave;
    }

    private void LeaveBike(Interactor interactor)
    {
        BikeData bikeData = interactor.GetComponent<BikeData>();

        if (bikeData == null || bikeData.player == null)
        {
            Debug.LogError("BikeData o player es null");
            return;
        }

        Vector3 playerPos = GetSpawnPos(interactor.transform);
        Quaternion playerRot = GetSpawnRot(interactor.transform);

        bikeData.player.SetActive(true);
        bikeData.player.transform.position = playerPos;
        bikeData.player.transform.rotation = playerRot;

        if (bikeData.playerCamera != null)
            bikeData.playerCamera.enabled = true;
        else
            Debug.LogWarning("playerCamera era null en BikeData");

        // El minimapa vuelve a seguir al jugador
        if (_minimap != null)
            _minimap.SetTarget(bikeData.player.transform);

        Destroy(interactor.gameObject);

        _prompt = _promptTake;
    }

    // ── Helpers ────────────────────────────────────────────────────
    private Vector3 GetSpawnPos(Transform origin)
    {
        Vector3 flat = origin.forward;
        flat.y = 0f;
        flat.Normalize();
        return origin.position + flat * spawnDistance + Vector3.up * spawnHeight;
    }

    private Quaternion GetSpawnRot(Transform origin)
    {
        Vector3 flat = origin.forward;
        flat.y = 0f;
        return Quaternion.LookRotation(flat);
    }
}