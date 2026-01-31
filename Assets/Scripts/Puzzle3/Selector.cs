using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    [SerializeField]
    private Button up;
    [SerializeField]
    private Button down;
    [TextArea]
    [SerializeField]
    private string selection;
    [SerializeField]
    private List<char> selector;
    private int current_index = -1;
    [SerializeField]
    private TextMeshProUGUI option;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ParseString();
    }
    public void ParseString()
    {
        selector = new List<char>();
        foreach (char s in selection)
        {

            selector.Add(s);
        }
        current_index = 0;
    }

    public void UpDisplay()
    {
        Debug.Log("Up");
        if (selector.Count != 0)
        {
            if (current_index + 1 < selector.Count)
            {
                current_index += 1;

            }
            else
            {
                current_index = 0;
            }
            option.text = selector[current_index].ToString();
        }
    }

    public char ReturnSelection()
    {
        if (current_index >= 0)
            return selector[current_index];
        return ' ';
    }
    public void DownDisplay()
    {

        if (selector.Count != 0)
        {
            if (current_index - 1 >= 0)
            {
                current_index -= 1;

            }
            else
            {
                current_index = selector.Count - 1;
            }
            option.text = selector[current_index].ToString();
            Debug.Log("down: " + current_index);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (current_index >= 0 && current_index < selector.Count)
        {
            option.text = selector[current_index].ToString();
        }
    }
}
