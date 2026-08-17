using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private ApiService apiService;
    [SerializeField] private Transform contenedorCartas;
    [SerializeField] private CardItemUI prefabCarta;
    [SerializeField] private TMP_Text nombreUsuarioText;
    [SerializeField] private TMP_Text nombreTercerosText;
    [SerializeField] private TMP_Text estadoText;

    private List<Carta> todasLasCartas = new List<Carta>();
    private List<Usuario> todosLosUsuarios = new List<Usuario>();
    private int indiceUsuarioActual = 0;

    private void Start()
    {
        CargarDatosIniciales();
    }

    private void CargarDatosIniciales()
    {
        estadoText.text = "Cargando cartas...";

        apiService.ObtenerCartas(
            cartas =>
            {
                todasLasCartas = cartas;
                estadoText.text = "Cargando usuarios...";

                apiService.ObtenerUsuarios(
                    usuarios =>
                    {
                        todosLosUsuarios = usuarios;
                        estadoText.text = "";
                        MostrarUsuarioActual();
                    },
                    error => estadoText.text = error
                );
            },
            error => estadoText.text = error
        );

        // Consulta independiente a la API de terceros
        apiService.ObtenerUsuarioAleatorio(
            resultado => nombreTercerosText.text =
                $"Usuario (API terceros): {resultado.name.first} {resultado.name.last}",
            error => nombreTercerosText.text = "Error API terceros: " + error
        );
    }

    private void MostrarUsuarioActual()
    {
        if (todosLosUsuarios.Count == 0) return;

        Usuario usuario = todosLosUsuarios[indiceUsuarioActual];
        nombreUsuarioText.text = $"Usuario: {usuario.nombre}";

        // Limpiar cartas anteriores
        foreach (Transform hijo in contenedorCartas)
            Destroy(hijo.gameObject);

        // Filtrar las cartas del usuario actual
        List<Carta> cartasDelUsuario = todasLasCartas
            .Where(c => usuario.cartasIds.Contains(c.id))
            .ToList();

        foreach (Carta carta in cartasDelUsuario)
        {
            CardItemUI instancia = Instantiate(prefabCarta, contenedorCartas);
            instancia.Configurar(carta);
        }
    }

    // Conectar este método al OnClick del botón "Cambiar usuario"
    public void CambiarUsuario()
    {
        if (todosLosUsuarios.Count == 0) return;
        indiceUsuarioActual = (indiceUsuarioActual + 1) % todosLosUsuarios.Count;
        MostrarUsuarioActual();
    }
}