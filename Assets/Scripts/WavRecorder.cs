using System.IO;
using UnityEngine;

/// <summary>
/// Записывает AudioClip с микрофона и конвертирует его в wav-файл.
/// </summary>
public class WavRecorder : MonoBehaviour
{
    [SerializeField] private int recordingTime = 10;
    [SerializeField] private int samplingRate = 44100;
    [SerializeField] private string wavFilename = "wav.wav";

    private bool _recording;
    private AudioClip _clip;

    public bool Recording => _recording;

    /// <summary>
    /// Начать запись.
    /// </summary>
    public void StartRecording()
    {
        _clip = Microphone.Start(Microphone.devices[0], true, recordingTime, samplingRate);
        _recording = true;
    }

    /// <summary>
    /// Остановить запись. Возвращает wav-файл массивов байтов. 
    /// </summary>
    public byte[] StopRecording()
    {
        SavWav.Save(wavFilename, _clip);
        _recording = false;
        var data = GetWavData();
        return data;
    }

    /// <summary>
    /// Читает записанный файл и возвращает массив байтов.
    /// </summary>
    private byte[] GetWavData()
    {
        var savedWavPath = Path.Combine(Application.persistentDataPath, wavFilename);
        return File.ReadAllBytes(savedWavPath);
    }
}