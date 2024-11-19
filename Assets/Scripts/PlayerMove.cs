using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public CharacterController controller;
    public float walkSpeed = 4f; // Velocidad de caminar
    public float runSpeed = 8f; // Velocidad de carrera

    private float x, y;
    private Vector3 move;

    private Vector3 velocity; // Velocidad actual, incluye gravedad
    public float gravity = -9.8f; // Gravedad
    public Transform groundCheck; // Punto para verificar si el jugador est� en el suelo
    public float radius = 0.4f; // Radio para la verificaci�n del suelo
    public LayerMask mask; // M�scara para definir qu� capas se consideran suelo
    private bool isGrounded = false; // Estado de si el jugador est� en el suelo
    public float jumpForce = 3f; // Fuerza del salto
    private Animator playerAnimator;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // Verifica si el jugador est� en el suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, radius, mask);

        // Resetea la velocidad vertical a la gravedad si est� en el suelo y est� cayendo
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Peque�o valor para mantener el personaje en el suelo
        }

        // Captura las entradas de movimiento
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        // Verifica si el jugador est� corriendo
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W);

        // Permite solo movimiento hacia adelante si est� corriendo
        if (isRunning)
        {
            x = 0; // Bloquear movimiento lateral
        }

        move = transform.right * x + transform.forward * y;

        // Actualiza las animaciones
        UpdateAnimations(isRunning, x, y);

        // Mueve al jugador
        controller.Move(move * (isRunning ? runSpeed : walkSpeed) * Time.deltaTime); // Aqu� se utiliza una condici�n ternariua. Si isRunning es true, se utiliza runSpeed, pero si es falsa, se utiliza walkSpeed)

        // Funci�n de salto
        jumpingFunction();

        // Aplica la gravedad a la velocidad
        velocity.y += gravity * Time.deltaTime;
        // Mueve al jugador con la velocidad calculada
        controller.Move(velocity * Time.deltaTime);
    }

    public void jumpingFunction()
    {
        if (Input.GetButtonDown("Jump") && isGrounded) // Saltar si est� en el suelo
        {
            // Calcula la velocidad vertical para el salto
            velocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);
        }
    }

    private void UpdateAnimations(bool isRunning, float x, float y)
    {
        playerAnimator.SetBool("Running", isRunning);
        playerAnimator.SetFloat("VelX", x);
        playerAnimator.SetFloat("VelY", y);
    }
}

