using UnityEngine;

public class CarCamera : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform coche;

    [Header("Posición")]
    public float distancia = 6f;
    public float altura = 2.5f;
    public Vector3 offset = new Vector3(0f, 1f, 0f);

    [Header("Rotación con ratón")]
    public float sensibilidadX = 60f;
    public float sensibilidadY = 40f;
    public float limiteArriba = 70f;
    public float limiteAbajo = -10f;

    [Header("Seguimiento automático")]
    public float velocidadSeguimiento = 3f;
    public float velocidadMinimaSeguir = 5f;

    [Header("Suavizado (solo seguimiento automático)")]
    public float suavizadoPosicion = 8f;
    public float suavizadoRotacion = 6f;

    private float rotX = 15f;
    private float rotY = 0f;
    private bool ratónMovido = false;
    private Rigidbody rbCoche;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (coche != null)
        {
            rbCoche = coche.GetComponent<Rigidbody>();
            rotY = coche.eulerAngles.y;
        }
    }

    void LateUpdate()
    {
        if (coche == null) return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f)
        {
            rotY += mouseX * sensibilidadX * Time.deltaTime;
            rotX -= mouseY * sensibilidadY * Time.deltaTime;
            rotX = Mathf.Clamp(rotX, limiteAbajo, limiteArriba);
            ratónMovido = true;
        }
        else
        {
            ratónMovido = false;
        }

        float velocidad = rbCoche != null ? rbCoche.linearVelocity.magnitude : 0f;

        if (!ratónMovido && velocidad > velocidadMinimaSeguir)
        {
            float anguloObjetivo = coche.eulerAngles.y;
            rotY = Mathf.LerpAngle(rotY, anguloObjetivo, velocidadSeguimiento * Time.deltaTime);
        }

        Quaternion rotacion = Quaternion.Euler(rotX, rotY, 0f);
        Vector3 posicionObjetivo = coche.position + offset - (rotacion * Vector3.forward * distancia) + Vector3.up * altura;
        Vector3 puntoMira = coche.position + offset;

        if (ratónMovido)
        {
            // Con ratón: sin retardo, posición y rotación directas
            transform.position = posicionObjetivo;
            transform.rotation = Quaternion.LookRotation(puntoMira - transform.position);
        }
        else
        {
            // Seguimiento automático: suavizado normal
            transform.position = Vector3.Lerp(transform.position, posicionObjetivo, suavizadoPosicion * Time.deltaTime);
            Quaternion rotacionObjetivo = Quaternion.LookRotation(puntoMira - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, suavizadoRotacion * Time.deltaTime);
        }
    }
}