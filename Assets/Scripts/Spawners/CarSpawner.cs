using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject carPrefab;         // Arrastra tu prefab aquí

    [Header("Referencias de Escena (se inyectan al spawneado)")]
    [SerializeField] private GameObject playerObject;      // El jugador
    [SerializeField] private GameObject playerCamera;      // Cámara del jugador
    // La cámara del coche viene DENTRO del prefab, no hace falta asignarla aquí

    [Header("Spawn")]
    [SerializeField] private KeyCode spawnKey = KeyCode.F1;
    [SerializeField] private KeyCode DespawnKey = KeyCode.F3;

    [SerializeField] private float spawnDistance = 5f;     // Metros delante del jugador
    [SerializeField] private float spawnHeight = 0f;       // Ajuste de altura si hace falta

    private GameObject currentCar;                         // Solo un coche a la vez

    private void Update()
    {
        if (Input.GetKeyDown(spawnKey))
            SpawnCar();
        if(Input.GetKeyDown(DespawnKey))
            DespawnCar();
    }

    private void SpawnCar()
    {
        // Evita spawns duplicados
        if (currentCar != null)
        {
            Debug.LogWarning("Ya hay un coche spawneado. Destrúyelo primero.");
            return;
        }

        // Calcula posición delante del jugador (ignora el eje Y para no flotar)
        Vector3 flatForward = playerObject.transform.forward;
        flatForward.y = 0f;
        flatForward.Normalize();

        Vector3 spawnPos = playerObject.transform.position
                         + flatForward * spawnDistance
                         + Vector3.up * spawnHeight;

        // Rota el coche para que mire en la misma dirección que el jugador
        Quaternion spawnRot = Quaternion.LookRotation(flatForward);

        // Instancia el prefab
        currentCar = Instantiate(carPrefab, spawnPos, spawnRot);

        // ── Inyección de referencias ──────────────────────────────────────
        CarEnter carEnter = currentCar.GetComponentInChildren<CarEnter>();

        if (carEnter == null)
        {
            Debug.LogError("El prefab no tiene el componente CarEnter. ¿Está en un hijo?");
            return;
        }

        // Objetos de escena → se los pasamos nosotros
        carEnter._PlayerPlayer = playerObject;
        carEnter._PlayerCamera = playerCamera;

        // El PlayerCar es el propio prefab spawneado (tiene el PrometeoCarController)
        carEnter._PlayerCar = currentCar;

        // La CarCamera está DENTRO del prefab → la buscamos ahí
        // Asegúrate de que la cámara del coche se llame "CarCamera" en el prefab
        // o ajusta el nombre aquí:
        Transform carCamTransform = currentCar.transform.Find("CarCamera");
        if (carCamTransform != null)
            carEnter._CarCamera = carCamTransform.gameObject;
        else
            Debug.LogError("No se encontró 'CarCamera' dentro del prefab. " +
                           "Revisa el nombre exacto del GameObject de la cámara.");

        Debug.Log($"Coche spawneado en {spawnPos} con todas las referencias inyectadas ✅");
    }

    // Opcional: destruir el coche con F2
    // private void Update() → añade esto dentro del Update de arriba:
    //   if (Input.GetKeyDown(KeyCode.F2)) DespawnCar();

    public void DespawnCar()
    {
        if (currentCar != null)
        {
            Destroy(currentCar);
            currentCar = null;
        }
    }
}