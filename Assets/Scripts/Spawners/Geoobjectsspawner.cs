using System.Collections;
using System.Collections.Generic;
using System.IO;
using CesiumForUnity;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Lee un archivo JSON con coordenadas geográficas y coloca prefabs en el globo de Cesium.
/// La altura se toma directamente del JSON. Para ajustar la posición vertical usa el campo
/// "localOffset" en el JSON (metros en espacio local del objeto).
/// </summary>
public class GeoObjectSpawner : MonoBehaviour
{
    [Header("Configuración de archivos")]
    [Tooltip("Nombre del archivo JSON dentro de StreamingAssets/")]
    public string jsonFileName = "objects_data.json";

    [Tooltip("Subcarpeta dentro de Resources/ donde están los prefabs")]
    public string prefabsFolder = "Prefabs";

    [Header("Cesium")]
    [Tooltip("Arrastra aquí el CesiumGeoreference de la escena")]
    public CesiumGeoreference georeference;

    // Registro interno: id → GameObject instanciado
    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();

    // ─── Ciclo de vida ───────────────────────────────────────────────────────

    private IEnumerator Start()
    {
        // Esperar un frame para que Cesium inicialice sus matrices de transformación
        yield return null;

        georeference ??= FindObjectOfType<CesiumGeoreference>();

        if (georeference == null)
        {
            Debug.LogError("[GeoObjectSpawner] No se encontró CesiumGeoreference en la escena.");
            yield break;
        }

        LoadAndSpawn();
    }

    // ─── Carga y spawn ───────────────────────────────────────────────────────

    public void LoadAndSpawn()
    {
        string path = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        if (!File.Exists(path))
        {
            Debug.LogError($"[GeoObjectSpawner] Archivo no encontrado: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        ObjectDataList dataList = JsonUtility.FromJson<ObjectDataList>(json);

        if (dataList == null || dataList.objects == null)
        {
            Debug.LogError("[GeoObjectSpawner] El archivo JSON está vacío o tiene formato incorrecto.");
            return;
        }

        Debug.Log($"[GeoObjectSpawner] Cargando {dataList.objects.Count} objetos...");

        foreach (ObjectData data in dataList.objects)
            SpawnObject(data);

        Debug.Log($"[GeoObjectSpawner] {spawnedObjects.Count} objetos colocados.");
    }

    // ─── Instanciación individual ────────────────────────────────────────────

    private void SpawnObject(ObjectData data)
    {
        GameObject prefab = Resources.Load<GameObject>($"{prefabsFolder}/{data.prefabName}");
        if (prefab == null)
        {
            Debug.LogWarning($"[GeoObjectSpawner] Prefab '{data.prefabName}' no encontrado " +
                             $"en Resources/{prefabsFolder}/");
            return;
        }

        // Instanciar en el origen — el anchor corregirá la posición con doble precisión
        GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
        go.name = !string.IsNullOrEmpty(data.nombre) ? data.nombre : data.id;
        go.transform.localScale = data.scale.ToVector3();

        // Anchor geográfico con doble precisión
        CesiumGlobeAnchor anchor = go.GetComponent<CesiumGlobeAnchor>()
                                ?? go.AddComponent<CesiumGlobeAnchor>();

        anchor.longitudeLatitudeHeight = new double3(
            data.longitude,
            data.latitude,
            data.height
        );

        // Offset local para corregir el pivote del prefab o ajustar la altura.
        // Se expresa en metros en espacio local (Y = arriba según la superficie del globo).
        // Si el prefab está bien centrado, deja localOffset en (0, 0, 0) en el JSON.
        if (data.localOffset != null)
        {
            Vector3 offset = data.localOffset.ToVector3();
            if (offset != Vector3.zero)
                go.transform.position += go.transform.TransformDirection(offset);
        }

        // Controlador de parada
        BusStopController controller = go.GetComponent<BusStopController>()
                                    ?? go.AddComponent<BusStopController>();
        controller.Initialize(data);

        spawnedObjects[data.id] = go;
    }

    // ─── Utilidades ──────────────────────────────────────────────────────────

    [ContextMenu("Recargar paradas")]
    public void Reload()
    {
        foreach (var go in spawnedObjects.Values)
            if (go != null) Destroy(go);

        spawnedObjects.Clear();
        LoadAndSpawn();
    }

    public GameObject GetBusStop(string id) =>
        spawnedObjects.TryGetValue(id, out var go) ? go : null;

    public IEnumerable<GameObject> GetAllBusStops() => spawnedObjects.Values;
}
