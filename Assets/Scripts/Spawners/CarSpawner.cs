using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject carPrefab;         

    [Header("Referencias de Escena (se inyectan al spawneado)")]
    [SerializeField] private GameObject playerObject;      
    [SerializeField] private GameObject playerCamera;     

    [Header("Spawn")]
    [SerializeField] private KeyCode spawnKey = KeyCode.C;
    [SerializeField] private KeyCode DespawnKey = KeyCode.F3;

    [SerializeField] private float spawnDistance = 5f;    
    [SerializeField] private float spawnHeight = 0f;       

    private GameObject currentCar;                         

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

        CarEnter carEnter = currentCar.GetComponentInChildren<CarEnter>();

        if (carEnter == null)
        {
            Debug.LogError("El prefab no tiene el componente CarEnter. ¿Está en un hijo?");
            return;
        }

        carEnter._PlayerPlayer = playerObject;
        carEnter._PlayerCamera = playerCamera;

        carEnter._PlayerCar = currentCar;

        Transform carCamTransform = currentCar.transform.Find("CarCamera");
        if (carCamTransform != null)
            carEnter._CarCamera = carCamTransform.gameObject;
        else
            Debug.LogError("No se encontró 'CarCamera' dentro del prefab. " +
                           "Revisa el nombre exacto del GameObject de la cámara.");

        Debug.Log($"Coche spawneado en {spawnPos} con todas las referencias inyectadas ✅");
    }

    public void DespawnCar()
    {
        if (currentCar != null)
        {
            Destroy(currentCar);
            currentCar = null;
        }
    }
}