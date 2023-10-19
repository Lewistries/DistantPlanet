using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DialogBox : MonoBehaviour, IPointerDownHandler
{

    public string[] messages;
    public string button;
    public int startIndex;
    public string scene;

    private TextMeshProUGUI text;

    private static int index = 0;
    private bool hidden = true;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        transform.parent.Translate(Vector3.left * 10000);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hidden && Input.GetButtonDown(button)) {
            index += 1;
            if (scene != null && scene != "") {
                SceneManager.LoadScene(scene, LoadSceneMode.Single);
            }
        }

        bool inRange = startIndex <= index && index < startIndex + messages.Length;
        if (inRange) {
            text.SetText(messages[index - startIndex]);
        }
        if (inRange && hidden) {
            transform.parent.Translate(Vector3.right * 10000);
        } else if (!inRange && !hidden) {
            transform.parent.Translate(Vector3.left * 10000);
        }
        hidden = !inRange;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!hidden && eventData.button == 0) {
            index += 1;
            if (scene != null && scene != "") {
                SceneManager.LoadScene(scene, LoadSceneMode.Single);
            }
        }
    }
}
