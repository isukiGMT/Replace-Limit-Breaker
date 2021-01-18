using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STMText : MonoBehaviour
{
    private PlayerSet.Player PlayerScript;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSet.Player>();
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Stamina: "+Mathf.Round(PlayerScript.Stamina) + "/" + PlayerScript.MaxStamina;
    }
}
