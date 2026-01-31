using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KeyPuzzle : MonoBehaviour
{
    [SerializeField]
    private bool get_key = false;
    [SerializeField] private GameObject seif_close_up_panel;
    [SerializeField] private GameObject seif;
    [SerializeField]
    private GameObject door;
    [SerializeField] private Button try_open;
    [SerializeField] private List<Selector> option;
    [TextArea][SerializeField] private string key_code;
    [SerializeField] private string code = "";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (seif_close_up_panel != null)
        {
            seif_close_up_panel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (get_key)
        {
            UnlockDoor();
        }
    }

    public void TryOpenSafe()
    {
        string code = "";
        if (option.Count != 0)
        {
            foreach (var op in option)
            {
                Debug.Log("current code:" + code + " add value " + op.ReturnSelection());
                code += op.ReturnSelection();

            }
        }
        if (key_code == code)
        {
            get_key = true;
            seif_close_up_panel.SetActive(false);
            seif.GetComponent<Collider2D>().enabled = false;
        }

    }
    void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Seif"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {

                if (!seif_close_up_panel.activeSelf)
                {
                    seif_close_up_panel.SetActive(true);
                }
                else
                {

                    seif_close_up_panel.SetActive(false);
                }
            }
        }
    }


    private void UnlockDoor()
    {
        if (door != null)
        {
            door.GetComponent<SpriteRenderer>().enabled = false;
            door.GetComponent<Collider2D>().enabled = false;
        }
    }
}
