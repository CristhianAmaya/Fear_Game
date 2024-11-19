using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights_mechanics : MonoBehaviour
{
    public GameObject planeLight; // Este objeto simular� el plano que se requiere para la luz
    private Renderer materialLight;
    public Color initialColor;
    public List<Color> colorslight; // Colores para la mec�nica
    private bool powerActivate;

    void Start()
    {
        // Asignamos el componente renderer y el color para trabajar con la transparencia
        materialLight = planeLight.GetComponent<Renderer>();

        // Colores necesarios para la mec�nica
        colorslight = new List<Color>
        {
            Color.red,   // Izquierda
            Color.green, // Derecha
            Color.blue,  // Adelante
            Color.yellow // Atr�s
        };

        // Establecer el color inicial con alpha
        initialColor = materialLight.material.color;
        initialColor.a = 0f; // Transparente
        materialLight.material.color = initialColor;

        powerActivate = false;
    }

    void Update()
    {
        ActivateLight();
    }

    public void ActivateLight()
    {
        if (Input.GetKey(KeyCode.Q) && !powerActivate)
        {
            StartCoroutine(IncreaseTransparency());
        }
    }

    private IEnumerator IncreaseTransparency()
    {
        powerActivate = true;
        float transparency = 0f;

        while (transparency < 0.8f)
        {
            transparency += Time.deltaTime * 0.5f; // Cambia la velocidad de aumento aqu�
            Color changeColor = initialColor;
            changeColor.a = Mathf.Clamp01(transparency); // Se utiliza Mathf.Clamp01 para asegurarse de que el valor de transparency no exceda 1.
            materialLight.material.color = changeColor; // Se asigna el cambio de color al material
            yield return null; // esto sirve para permitir que Unity actualice el renderizado correctamente entre cada paso de la transici�n.
        }

        yield return new WaitForSeconds(0.5f);

        while (transparency > 0f)
        {
            transparency -= Time.deltaTime * 0.5f; // Cambia la velocidad de aumento aqu�
            Color changeColor = initialColor;
            changeColor.a = Mathf.Clamp01(transparency); // Se utiliza Mathf.Clamp01 para asegurarse de que el valor de transparency no exceda 1.
            materialLight.material.color = changeColor; // Se asigna el cambio de color al material
            yield return null; // esto sirve para permitir que Unity actualice el renderizado correctamente entre cada paso de la transici�n.
        }

        powerActivate = false;
    }
}

