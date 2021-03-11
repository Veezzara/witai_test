using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;

public class WitRecognizer : MonoBehaviour
{
    [SerializeField] private string token;

    private const string Uri = "https://api.wit.ai/speech";

    /// <summary>
    /// Отправляет wav и ждет ответа. Возвращает HttpResponseMessage.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private async Task<HttpResponseMessage> GetRecognitionHttpResponseAsync(byte[] data)
    {
        var httpClient = new HttpClient();
        var request = new HttpRequestMessage(new HttpMethod("POST"), Uri);
        request.Headers.Add("Authorization", $"Bearer {token}");
        request.Content = new ByteArrayContent(data);
        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/wav");
        return await httpClient.SendAsync(request);
    }

    /// <summary>
    /// Отправляет wav и ждет ответа. Возвращает распознанную строчку.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public async Task<string> GetRecognitionResultAsync(byte[] data)
    {
        var response = await GetRecognitionHttpResponseAsync(data);
        var content = await response.Content.ReadAsStringAsync();
        return JsonUtility.FromJson<Response>(content).text;
    }
}

public class Response
{
    public string text;
}