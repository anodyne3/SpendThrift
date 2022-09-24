// Unused Tool
using System.Globalization;
using TMPro;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InputTextSlider : Slider
{
    public UnityAction<float> sliderChange, inputChange;

    [SerializeField] protected TMP_InputField inputField;

    protected override void Awake()
    {
        base.Awake();

        if (!inputField)
            return;

        inputField.contentType = TMP_InputField.ContentType.DecimalNumber;

        inputField.onValueChanged.AddListener(UpdateSlider);
        onValueChanged.AddListener(UpdateInputField);
    }

    protected virtual void UpdateSlider(string inputFieldText)
    {
        if (float.TryParse(inputFieldText, NumberStyles.Float, CultureInfo.CurrentCulture, out var inputFieldFloat))
            SetValueWithoutNotify(inputFieldFloat);

        sliderChange?.Invoke(inputFieldFloat);
    }

    protected virtual void UpdateInputField(float sliderValue)
    {
        inputChange?.Invoke(sliderValue);
        inputField.SetTextWithoutNotify(sliderValue.ToString("N2"));
    }
}

[CustomEditor(typeof(InputTextSlider))]
public class InputTextSliderEditor : SliderEditor
{
    private SerializedProperty inputField;

    protected override void OnEnable()
    {
        base.OnEnable();

        inputField = serializedObject.FindProperty("inputField");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(inputField);
        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}
