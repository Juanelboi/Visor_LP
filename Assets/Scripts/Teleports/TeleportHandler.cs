using UnityEngine;

public class TeleportHandler : MonoBehaviour 
{
    private bool isTeleporting = false;
    public GameObject teleportMenu;
    public GameObject Loading;
    public GameObject player;

    void Update()
    {

        if (Input.GetKeyUp(KeyCode.T) && teleportMenu != null)
        {
            {
                if (!isTeleporting)
                {
                    if (isTeleporting == false)
                    {
                        teleportMenu.SetActive(true);
                        isTeleporting = true;
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                    }
                }
                else if (isTeleporting == true) 
                {
                    CloseTeleportMenu();
                }
            }
        }
    }


    public void CloseTeleportMenu()
    {
        if (teleportMenu != null)
        {
            teleportMenu.SetActive(false);
            isTeleporting = false;
            Time.timeScale = 1f; // Reanuda el tiempo en el juego
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }



    public void TeleportPlayer(Transform transform)
    {
        teleportMenu.SetActive(false);
        Loading.GetComponent<CesiumLoadingScreen>().Show();
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        float newY = transform.position.y;
        player.transform.position = new Vector3(transform.position.x, newY+2, transform.position.z);

        if (cc != null) cc.enabled = true;

    }

}
