using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecognitionController : MonoBehaviour
{
    [SerializeField] private WavRecorder recorder;
    [SerializeField] private WitRecognizer recognizer;

    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text statusText;

    [SerializeField] private Button button;
    private TMP_Text _buttonText;

    private void Start()
    {
        statusText.text = "Статус: Готово";
        _buttonText = button.GetComponentInChildren<TMP_Text>();

        button.onClick.AddListener(() =>
        {
            if (recorder.Recording)
            {
                button.interactable = false;
                var recording = recorder.StopRecording();
                RecognizeAndSetResultAsync(recording);
            }
            else
            {
                _buttonText.text = "Остановить запись";
                recorder.StartRecording();
                statusText.text = "Статус: Запись...";
            }
        });
    }

    private async void RecognizeAndSetResultAsync(byte[] data)
    {
        statusText.text = "Статус: Распознавание...";
        var res = await recognizer.GetRecognitionResultAsync(data);
        statusText.text = "Статус: Готово";

        resultText.text = $"Результат: {res}";

        _buttonText.text = "Начать запись";
        button.interactable = true;
    }
}