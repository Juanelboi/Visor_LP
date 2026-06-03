using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Serialization.Json;

public class Info : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt = "Más información";
    public TextAsset ArchivoJson;
    public TextMeshProUGUI textoTitulo;
    public TextMeshProUGUI textoContenido;
    public Sprite imagenFondo; // null si no hay fondo
    public Image imagenUI;// null si no hay imagen

    public GameObject _GrupoInfo;
    public string InteractionPrompt => _prompt;

    [System.Serializable]
    class InfoData
    {
        public string titulo;
        public string contenido;
    }

    void Start()
    {
        if (imagenFondo != null && imagenUI != null)
            imagenUI.sprite = imagenFondo;
        CargarDatos();
    }

    void CargarDatos()
    {
        // TEXTO
        if (ArchivoJson != null)
        {
            InfoData data = JsonUtility.FromJson<InfoData>(ArchivoJson.text);
            textoTitulo.text = data.titulo;
            textoContenido.text = data.contenido;
        }

    }

    public void Interact(Interactor interactor)
    {
        if(_GrupoInfo.activeSelf)
            _GrupoInfo.SetActive(false);
        else
            _GrupoInfo.SetActive(true);

    }
}
