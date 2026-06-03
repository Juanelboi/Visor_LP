using UnityEngine;

/// <summary>
/// Script que se adjunta automáticamente a cada parada de guagua
/// al ser instanciada por GeoObjectSpawner.
/// </summary>
public class BusStopController : MonoBehaviour
{
    [Header("Datos de la parada")]
    public string stopId;
    public string displayName;
    public double longitude;
    public double latitude;
    public double height;

    /// <summary>
    /// Inicializa la parada con los datos leídos del archivo JSON.
    /// Llamado automáticamente por GeoObjectSpawner.
    /// </summary>
    public void Initialize(ObjectData data)
    {
        stopId = data.id;
        displayName = !string.IsNullOrEmpty(data.nombre) ? data.nombre : data.id;
        longitude = data.longitude;
        latitude = data.latitude;
        height = data.height;

        if (!string.IsNullOrEmpty(data.tag))
            gameObject.tag = data.tag;

    }

}