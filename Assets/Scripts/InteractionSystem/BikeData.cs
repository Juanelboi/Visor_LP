using UnityEngine;

// Componente que vive en el prefab de la bici
// Guarda quién la está usando
public class BikeData : MonoBehaviour
{
    [HideInInspector] public GameObject player;
    [HideInInspector] public Camera playerCamera;
}