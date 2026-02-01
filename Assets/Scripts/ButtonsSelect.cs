using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsSelect : MonoBehaviour
{
    [SerializeField] private Color _handleColor;
    [SerializeField] private Color _normalColor;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private float _inputDelay = 0.3f;
    [SerializeField] private float _scaleDuration = 0.2f;
    [SerializeField] private Vector3 _selectedScale = new Vector3(1.1f, 1.1f, 1f);
    [SerializeField] private Vector3 _normalScale = Vector3.one;

    private int _selectedButtonIndex = 0;
    private float _nextInputTime = 0f;

    void Start()
    {
        HighlightButton(_selectedButtonIndex);
    }

    void Update()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (Time.time >= _nextInputTime)
        {
            if (verticalInput > 0)
            {
                ButtonUp();
                _nextInputTime = Time.time + _inputDelay;
            }
            else if (verticalInput < 0)
            {
                ButtonDown();
                _nextInputTime = Time.time + _inputDelay;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || (JoystickController.Instance != null && JoystickController.Instance.IsSouthButtonPressed()))
        {
            _buttons[_selectedButtonIndex].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void ButtonDown()
    {
        _selectedButtonIndex = (_selectedButtonIndex + 1) % _buttons.Length;
        HighlightButton(_selectedButtonIndex);
    }

    private void ButtonUp()
    {
        _selectedButtonIndex = (_selectedButtonIndex - 1 + _buttons.Length) % _buttons.Length;
        HighlightButton(_selectedButtonIndex);
    }

    private void HighlightButton(int index)
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            Image buttonImage = _buttons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = _normalColor;
            }
            StartCoroutine(ScaleButton(_buttons[i].transform, _normalScale));
        }

        Image selectedButtonImage = _buttons[index].GetComponent<Image>();
        if (selectedButtonImage != null)
        {
            selectedButtonImage.color = _handleColor;
            StartCoroutine(ScaleButton(_buttons[index].transform, _selectedScale));
        }

    }

    private IEnumerator ScaleButton(Transform buttonTransform, Vector3 targetScale)
    {
        Vector3 initialScale = buttonTransform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < _scaleDuration)
        {
            buttonTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / _scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        buttonTransform.localScale = targetScale;
    }
}
