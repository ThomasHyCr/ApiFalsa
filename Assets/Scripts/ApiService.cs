using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class ApiService : MonoBehaviour
{
    // Reemplaza por tu propio endpoint generado en el paso 1
    private const string BASE_URL = "https://my-json-server.typicode.com/ThomasHyCr/ApiFalsa";
    private const string RANDOM_USER_URL = "https://randomuser.me/api/?results=1";

    // ---------- API FALSA: obtener todas las cartas ----------
    public void ObtenerCartas(Action<List<Carta>> onSuccess, Action<string> onError)
    {
        StartCoroutine(GetRequest($"{BASE_URL}/cartas", onSuccess, onError));
    }

    // ---------- API FALSA: obtener todos los usuarios ----------
    public void ObtenerUsuarios(Action<List<Usuario>> onSuccess, Action<string> onError)
    {
        StartCoroutine(GetRequest($"{BASE_URL}/usuarios", onSuccess, onError));
    }

    // ---------- API DE TERCEROS: usuario aleatorio ----------
    public void ObtenerUsuarioAleatorio(Action<RandomUserResult> onSuccess, Action<string> onError)
    {
        StartCoroutine(GetRequestSingle<RandomUserResponse>(RANDOM_USER_URL, response =>
        {
            if (response.results != null && response.results.Count > 0)
                onSuccess?.Invoke(response.results[0]);
            else
                onError?.Invoke("Respuesta vacía de randomuser.me");
        }, onError));
    }

    // ---------- Corrutina genérica para listas ----------
    private IEnumerator GetRequest<T>(string url, Action<List<T>> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke($"Error consultando {url}: {request.error}");
            }
            else
            {
                try
                {
                    List<T> data = JsonConvert.DeserializeObject<List<T>>(request.downloadHandler.text);
                    onSuccess?.Invoke(data);
                }
                catch (Exception e)
                {
                    onError?.Invoke($"Error parseando JSON: {e.Message}");
                }
            }
        }
    }

    // ---------- Corrutina genérica para objetos únicos ----------
    private IEnumerator GetRequestSingle<T>(string url, Action<T> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke($"Error consultando {url}: {request.error}");
            }
            else
            {
                try
                {
                    T data = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
                    onSuccess?.Invoke(data);
                }
                catch (Exception e)
                {
                    onError?.Invoke($"Error parseando JSON: {e.Message}");
                }
            }
        }
    }
}