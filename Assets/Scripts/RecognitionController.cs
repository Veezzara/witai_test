using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RecognitionController : MonoBehaviour
{
    [SerializeField] private WavRecorder recorder;
    [SerializeField] private WitRecognizer recognizer;

    [Header("UI")]
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Button button;
    
    [Header("Events")]
    [SerializeField] private UnityEvent<string> onRecognized;
    [SerializeField] private UnityEvent onRecordStart;
    [SerializeField] private UnityEvent onRecordEnd;

    private TMP_Text _buttonText;

    private void Start()
    {
        statusText.text = "Статус: Готово";
        _buttonText = button.GetComponentInChildren<TMP_Text>();

        button.onClick.AddListener(() =>
        {
            if (recorder.Recording)
            {
                var recording = recorder.StopRecording();
                RecognizeAndSetResultAsync(recording);
                onRecordEnd.Invoke();
            }
            else
            {
                recorder.StartRecording();
                onRecordStart.Invoke();
            }
        });
        
        onRecognized.AddListener(OnRecognized);
        onRecordStart.AddListener(OnRecordStart);
        onRecordEnd.AddListener(OnRecordEnd);
    }

    private void OnRecognized(string result)
    {
        statusText.text = "Статус: Готово";
        resultText.text = $"Результат: {result}";
        _buttonText.text = "Начать запись";
        button.interactable = true;
    }

    private void OnRecordStart()
    {
        _buttonText.text = "Остановить запись";
        statusText.text = "Статус: Запись...";
    }
    
    private void OnRecordEnd()
    {
        button.interactable = false;
    }

    private async void RecognizeAndSetResultAsync(byte[] data)
    {
        statusText.text = "Статус: Распознавание...";
        var res = await recognizer.GetRecognitionResultAsync(data);
        onRecognized.Invoke(res);
    }
}