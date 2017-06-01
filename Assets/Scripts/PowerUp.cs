using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PowerUp : MonoBehaviour {

    public SpriteRenderer symbol;
    public SpriteRenderer background;
    Mouse mousePointer;
    PlayerMovement player;
    [HideInInspector]
    public Type type;
    bool pickedUp = false;
    Color unselectedColor;
    public Color selectedColor;
    bool animating = false;
    public float animationSpeed = 0.5f;
    public float animationDelay = 0.7f;


    private void Start()
    {
        type = GetComponent<GravityBomb>() != null ? Type.GravityBomb : GetComponent<Explosion>() != null ? Type.Explosion : Type.Pull;
        background = GetComponent<SpriteRenderer>();
        mousePointer = FindObjectOfType<Mouse>();
        player = FindObjectOfType<PlayerMovement>();
        unselectedColor = symbol.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var p = collision.GetComponent<PlayerMovement>();
        if (p != null && !pickedUp)
        {
            pickedUp = true;
            PickUp(p);
        }
    }

    public void PickUp(PlayerMovement player)
    {
        Show(true, false);
        GetComponent<Collider2D>().enabled = false;
        player.powerUps.Add(this);
        switch (type)
        {
            case Type.GravityBomb:
                transform.SetParent(player.gravityBombPosition.transform);
                player.gravity = GetComponent<GravityBomb>();
                break;
            case Type.Explosion:
                transform.SetParent(player.explosionPosition.transform);
                player.explosion = GetComponent<Explosion>();
                break;
            case Type.Pull:
                transform.SetParent(player.pullPosition.transform);
                player.pull = GetComponent<Pull>();
                break;
        }
        transform.localPosition = Vector3.zero;
    }

    void Show(bool s, bool b)
    {
        symbol.enabled = s;
        background.enabled = b;
    }

        

    public void Select()
    {
        switch(type){
            case Type.GravityBomb | Type.Explosion:
                break;
            case Type.Pull:
                break;
        }
        symbol.color = selectedColor;
    }

    public void Unselect()
    {
        switch (type)
        {
            case Type.GravityBomb | Type.Explosion:
                break;
            case Type.Pull:
                break;
        }
        symbol.color = unselectedColor;
    }

    public void Place()
    {
        switch (type)
        {
            case Type.GravityBomb:
                Instantiate(GetComponent<GravityBomb>(), mousePointer.transform.position, Quaternion.identity).Arm();
                break;
            case Type.Explosion:
                 Instantiate(GetComponent<Explosion>(), mousePointer.transform.position, Quaternion.identity).Arm();
                break;
            case Type.Pull:
                player.cone.Show(true);
                break;

        }
    }

    public void Animate(float duration)
    {
        var until = Time.time + duration;
        Util.While(() => Time.time <= until, () => Util.Lerp(symbol, selectedColor, Color.white, animationSpeed, () => Util.Lerp(symbol, Color.white, selectedColor, animationSpeed)), animationSpeed * 2);
    }


    public enum Type
    {
        GravityBomb,
        Explosion,
        Pull,
        None
    }

    public void ResetScale()
    {
        var parent = transform.parent;
        transform.SetParent(null);
        transform.localScale = Vector3.one;
        transform.SetParent(parent);
    }
}
