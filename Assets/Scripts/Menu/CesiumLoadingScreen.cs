using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CesiumForUnity;

[RequireComponent(typeof(CanvasGroup))]
public class CesiumLoadingScreen : MonoBehaviour
{
    [Header("Cesium")]
    [SerializeField] private Cesium3DTileset[] tilesets;

    [Header("UI de carga")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image artworkImage;
    [SerializeField] private Sprite[] artworks;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Text tipText;
    [SerializeField] private string[] tips;

    [Header("Ajustes de carga")]
    [SerializeField] private float loadThreshold = 95f;
    [SerializeField] private int stableFramesRequired = 15;
    [SerializeField] private float timeout = 30f;

    [Header("Ajustes visuales")]
    [SerializeField] private float secondsPerArtwork = 4f;
    [SerializeField] private float crossfadeDuration = 0.5f;
    [SerializeField] private float finalFadeDuration = 1f;
    [SerializeField] private bool randomOrder = false;

    [Header("Comportamiento al iniciar")]
    [Tooltip("Mostrar la pantalla de carga automáticamente al arrancar la escena")]
    [SerializeField] private bool showOnStart = true;

    private bool isLoaded = false;
    private bool isActive = false;

    // evento opcional para avisar cuando termina (útil para el teleport)
    public System.Action OnLoadingComplete;

    void Awake()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        // empezar oculto si no es carga inicial
        if (!showOnStart) HideImmediate();
    }

    void Start()
    {
        if (showOnStart) Show();
    }

    /// <summary>Lanza la pantalla de carga y espera a que Cesium cargue.</summary>
    public void Show()
    {
        if (isActive) return; // ya está corriendo
        isActive = true;
        isLoaded = false;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        StopAllCoroutines();
        StartCoroutine(CycleArtworks());
        StartCoroutine(WaitForTilesToLoad());
    }

    private void HideImmediate()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    // ---- Carga de Cesium ----
    private IEnumerator WaitForTilesToLoad()
    {
        float elapsed = 0f;
        int stableFrames = 0;

        yield return null;

        while (stableFrames < stableFramesRequired && elapsed < timeout)
        {
            float minProgress = 100f;
            foreach (var tileset in tilesets)
            {
                if (tileset == null) continue;
                minProgress = Mathf.Min(minProgress, tileset.ComputeLoadProgress());
            }

            if (progressBar != null)
                progressBar.value = minProgress / 100f;

            if (minProgress >= loadThreshold)
                stableFrames++;
            else
                stableFrames = 0;

            elapsed += Time.deltaTime;
            yield return null;
        }

        isLoaded = true;
        StartCoroutine(RevealScene());
    }

    // ---- Rotación de imágenes tipo GTA ----
    private IEnumerator CycleArtworks()
    {
        if (artworks == null || artworks.Length == 0) yield break;

        int index = 0;
        SetArtwork(index);
        ShowRandomTip();
        SetImageAlpha(1f);

        while (!isLoaded)
        {
            yield return new WaitForSeconds(secondsPerArtwork);
            if (isLoaded) break;

            index = randomOrder
                ? Random.Range(0, artworks.Length)
                : (index + 1) % artworks.Length;

            yield return StartCoroutine(CrossfadeTo(index));
            ShowRandomTip();
        }
    }

    private IEnumerator CrossfadeTo(int index)
    {
        float t = 0f;
        while (t < crossfadeDuration)
        {
            t += Time.deltaTime;
            SetImageAlpha(Mathf.Lerp(1f, 0f, t / crossfadeDuration));
            yield return null;
        }

        SetArtwork(index);

        t = 0f;
        while (t < crossfadeDuration)
        {
            t += Time.deltaTime;
            SetImageAlpha(Mathf.Lerp(0f, 1f, t / crossfadeDuration));
            yield return null;
        }
    }

    private void SetArtwork(int index)
    {
        if (artworkImage != null && artworks != null && artworks.Length > 0)
            artworkImage.sprite = artworks[index];
    }

    private void SetImageAlpha(float a)
    {
        if (artworkImage == null) return;
        Color c = artworkImage.color;
        c.a = a;
        artworkImage.color = c;
    }

    private void ShowRandomTip()
    {
        if (tipText != null && tips != null && tips.Length > 0)
            tipText.text = tips[Random.Range(0, tips.Length)];
    }

    // ---- Revelar la escena ----
    private IEnumerator RevealScene()
    {
        float t = 0f;
        while (t < finalFadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / finalFadeDuration);
            yield return null;
        }

        HideImmediate();
        isActive = false;

        OnLoadingComplete?.Invoke();
    }
}