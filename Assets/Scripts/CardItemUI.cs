using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nombreText;
    [SerializeField] private TMP_Text valorText;
    [SerializeField] private Image fondoImagen; // opcional, para colorear por palo

    public void Configurar(Carta carta)
    {
        nombreText.text = carta.nombre;
        valorText.text = $"Valor: {carta.valor}";

        // Color simple según el palo, a modo de ejemplo visual
        switch (carta.palo)
        {
            case "Corazones":
            case "Diamantes":
                fondoImagen.color = new Color(0.85f, 0.2f, 0.2f);
                break;
            default:
                fondoImagen.color = new Color(0.15f, 0.15f, 0.15f);
                break;
        }
    }
}