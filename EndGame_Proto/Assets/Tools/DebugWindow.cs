using System.Collections.Generic;
using UnityEngine;

public class DebugWindow : MonoBehaviour
{
    private static DebugWindow _instance;

    public static void AddPrintTask(PrintTaskFunc printTask)
    {
        if (_instance != null)
            _instance.RegisterPrintTask(printTask);
    }

    [Header("Window")]
    [SerializeField] private float _width;
    [SerializeField] private float _margin;
    [SerializeField] private GUIStyle _windowStyle;
    // ---------------------------------------------
    [Header("Element")]
    [SerializeField] private float _areaPadding;
    [SerializeField] private float _elementSpacing;
    [SerializeField] private GUIStyle _elementStyle;

    //                  DISLAY
    // ---------------------------------------------
    private Rect _windowRect;
    private GUIContent _content;

    //               FUNCTIONALITY
    // ---------------------------------------------
    public delegate string PrintTaskFunc();
    private List<PrintTaskFunc> _printTasks;

    private void Awake()
    {
        _instance = this;
        _printTasks = new List<PrintTaskFunc>(20);
    }

    private void Start()
    {
        _windowRect.position = new Vector2(Screen.width - _width - _margin, _margin);
        _windowRect.size = new Vector2(_width, Screen.height - _margin * 2);

        _content = new GUIContent();
    }

    private void OnGUI()
    {
        _elementStyle.fixedWidth = (_width - _areaPadding);
        _elementStyle.contentOffset = Vector2.one * ((_width - (_width - _areaPadding)) / 2f);

        GUILayout.BeginArea(_windowRect, _windowStyle);
        for (int i = 0; i < _printTasks.Count; i++)
        {
            _content.text = _printTasks[i]();
            GUILayout.Label(_content, _elementStyle);
            GUILayout.Space(_elementSpacing);
        }
        GUILayout.EndArea();
    }

    private void RegisterPrintTask(PrintTaskFunc func)
    {
        _printTasks.Add(func);
    }
}