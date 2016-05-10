using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Mycanvas : MonoBehaviour {
    public SerialHandler serialHandler;
    static Canvas _canvas;
    string[] menuName, sceneName;
    int menuLook;

    Color nomalColor, highlightColor;

    private static float[] _fingerVal = new float[4];
    float[] finger_prev = new float[4];
    string change_name; 

    // Use this for initialization
    void Start () {
        _canvas = GetComponent<Canvas>();
        menuName = new string[3] { "Button_dataglove", "Button_Unitychan", "Button_hiratest" };
        sceneName = new string[3] {"dataglove", "unitychan", "hiratest" };
        menuLook = 0;

        nomalColor = Color.white;
        highlightColor = Color.green;

        SetColor(menuName[0], highlightColor);

        serialHandler.OnDataReceived += OnDataReceived;

        StartCoroutine(reload());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDataReceived(string message)
    {
        var data = message.Split(
                new string[] { "\t" }, System.StringSplitOptions.None);

        if (data.Length != 10) return;

        try
        {
            //_fingerVal[0] = float.Parse(data[0]);
            _fingerVal[1] = float.Parse(data[2]);
            _fingerVal[2] = float.Parse(data[1]);
            _fingerVal[3] = float.Parse(data[0]);
          }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    private IEnumerator reload()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (_fingerVal[1] > 30 && _fingerVal[2] > 30 && _fingerVal[3] > 30)
            {
                serialHandler.changeScene();
                Application.LoadLevel(sceneName[menuLook]);
            }
            else if (_fingerVal[3] > 30)
            {
                //Debug.Log("prev1 " + finger_prev[1]);
                SetColor(menuName[menuLook], nomalColor);
                menuLook--;
                if (menuLook < 0) menuLook = menuName.Length - 1;
                SetColor(menuName[menuLook], highlightColor);
            }
            else if (_fingerVal[1] > 30)
            {
                //Debug.Log("prev2 " + finger_prev[3]);
                SetColor(menuName[menuLook], nomalColor);
                menuLook = (menuLook + 1) % menuName.Length;
                SetColor(menuName[menuLook], highlightColor);
            }


        }
    }

    public static void SetColor(string name, Color color)
    {
        foreach (Transform child in _canvas.transform)
        {
            // 子の要素をたどる
            if (child.name == name)
            {
                Button btn = child.GetComponent<Button>();
                btn.image.color = color;
                return;
            }
        }
        // 指定したオブジェクト名が見つからなかった
        Debug.LogWarning("Not found objname:" + name);
    }
}
