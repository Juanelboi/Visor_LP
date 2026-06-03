using StarterAssets;
using UnityEngine;

public class GuaguaDoble : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactionPrompt = "Pille la guagua turista";

    [SerializeField] private GameObject _guaguaTurista;
    [SerializeField] private GameObject _minimap;

    public string InteractionPrompt => _interactionPrompt;

    public void Interact(Interactor interactor)
    {
        _guaguaTurista.SetActive(true);
        interactor.gameObject.SetActive(false);
        _minimap.SetActive(false);
        this.gameObject.SetActive(false);
    }

}
