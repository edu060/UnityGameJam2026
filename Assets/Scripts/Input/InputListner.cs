using UnityEngine;
using UnityEngine.Windows;

public class InputListner : MonoBehaviour
{
    public static InputListner Instance;

    private InputMaster _input;

    public InputMaster Input => _input;

    public void InitializeControllers()
    {
        _input = new InputMaster();
        _input.Enable();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        InitializeControllers();
    }

}
