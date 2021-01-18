using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualHUD : MonoBehaviour
{
    public Image HPBAr;
    public Image StaminaBAr;
    private PlayerSet.Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSet.Player>();
    }

    // Update is called once per frame
    void Update()
    {
        HPBAr.fillAmount = _player.CurrentHP / _player.MaximumHP;
        StaminaBAr.fillAmount = _player.Stamina / _player.MaxStamina;
    }
}
