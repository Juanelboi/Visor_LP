using System.Collections;
using CesiumForUnity;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Añade este componente a los prefabs georreferenciados para que se corrijan
/// automáticamente sobre la superficie real del tileset de Cesium.
///
/// Flujo:
///   1. El objeto aparece a la altura del JSON (spawn inmediato).
///   2. Esta coroutine espera <checkInterval> segundos y lanza un rayo hacia abajo.
///   3. Si el rayo impacta la malla del tileset, reposiciona el objeto.
///   4. Si los tiles aún no han cargado, reintenta hasta <maxAttempts> veces.
///   5. Una vez anclado correctamente, el componente se deshabilita.
/// </summary>
public class TerrainSnapper : MonoBehaviour
{
    [Header("Raycast")]
    [Tooltip("Metros por encima del objeto desde donde sale el rayo. " +
             "Debe superar la altura máxima de edificios cercanos (~300 m en zonas urbanas).")]
    public float raycastStartHeight = 500f;

    [Tooltip("Distancia máxima del rayo en metros")]
    public float raycastMaxDistance = 1500f;

    [Tooltip("Capas a comprobar. Asigna la capa del tileset de Cesium.")]
    public LayerMask tilesetLayerMask = Physics.DefaultRaycastLayers;

    [Header("Posicionamiento")]
    [Tooltip("Metros sobre el punto de impacto donde queda el objeto")]
    public float heightOffset = 0.3f;

    [Tooltip("Diferencia mínima (metros) para considerar que la corrección es necesaria. " +
             "Evita micro-ajustes si el JSON ya era correcto.")]
    public float snapThreshold = 0.5f;

    [Header("Reintentos")]
    [Tooltip("Segundos entre intentos de raycast (los tiles tardan en cargarse)")]
    public float checkInterval = 1f;

    [Tooltip("Número máximo de intentos antes de rendirse")]
    public int maxAttempts = 100;

    // ─── Estado ──────────────────────────────────────────────────────────────

    private CesiumGlobeAnchor anchor;
    private bool snapped = false;

    // ─── Ciclo de vida ───────────────────────────────────────────────────────

    private void Start()
    {
        anchor = GetComponent<CesiumGlobeAnchor>();

        if (anchor == null)
        {
            Debug.LogWarning($"[TerrainSnapper] '{name}' no tiene CesiumGlobeAnchor. " +
                             "El componente se deshabilitará.");
            enabled = false;
            return;
        }

        StartCoroutine(SnapLoop());
    }

    // ─── Lógica de snap ──────────────────────────────────────────────────────

    private IEnumerator SnapLoop()
    {
        // Esperar dos frames para que el anchor inicialice su posición en el mundo
        yield return null;
        yield return null;

        int intentos = 0;

        while (!snapped && intentos < maxAttempts)
        {
            intentos++;

            // Lanzar rayo desde un punto elevado sobre el objeto hacia el suelo.
            // transform.up apunta a la normal de la superficie del globo en esta   
            // posición (CesiumGlobeAnchor la alinea automáticamente).
            Vector3 origenRayo = transform.position + transform.up * raycastStartHeight;
            Vector3 direccion = -transform.up;

            if (Physics.Raycast(origenRayo, direccion, out RaycastHit hit,
                                raycastMaxDistance, tilesetLayerMask))
            {
                double3 pos = anchor.longitudeLatitudeHeight;
                double alturaFinal = pos.z + raycastStartHeight - hit.distance + heightOffset;
                double diferencia = System.Math.Abs(alturaFinal - pos.z);

                if (diferencia > snapThreshold)
                {
                    anchor.longitudeLatitudeHeight = new double3(pos.x, pos.y, alturaFinal);

                    Debug.Log($"[TerrainSnapper] '{name}' corregido: " +
                              $"{pos.z:F1} m → {alturaFinal:F1} m " +
                              $"(Δ {diferencia:F1} m, intento {intentos})");
                }
                else
                {
                    Debug.Log($"[TerrainSnapper] '{name}' ya estaba correctamente " +
                              $"posicionado (Δ {diferencia:F2} m < umbral {snapThreshold} m).");
                }

                snapped = true;
                enabled = false; // deshabilitar: ya no hace falta seguir comprobando
                yield break;
            }

            // El tile aún no ha cargado — esperar e intentar de nuevo
            yield return new WaitForSeconds(checkInterval);
        }

        // Agotados los intentos: el objeto se queda donde estaba
        Debug.LogWarning($"[TerrainSnapper] '{name}' no encontró terreno tras " +
                         $"{maxAttempts} intentos ({maxAttempts * checkInterval:F0} s). " +
                         $"El objeto permanece a la altura del JSON. " +
                         $"Comprueba: (1) 'Create Physics Meshes' activo en el Cesium3DTileset, " +
                         $"(2) la capa del tileset incluida en tilesetLayerMask.");

        enabled = false;
    }

    // ─── Utilidad pública ────────────────────────────────────────────────────

    /// <summary>
    /// Fuerza un reintento manual aunque el componente esté deshabilitado.
    /// Útil para re-snap después de teletransportar el jugador a una nueva zona.
    /// </summary>
    public void ForceReSnap()
    {
        snapped = false;
        enabled = true;
        StartCoroutine(SnapLoop());
    }
}
