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
                break;
            case Type.Explosion:
                transform.SetParent(player.explosionPosition.transform);
                break;
            case Type.Pull:
                transform.SetParent(player.pullPosition.transform);
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
                player.cone.Show(true);
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
                player.cone.Show(false);
                break;
        }
        symbol.color = unselectedColor;
    }

    //void Animate(bool start)
    //{
    //    animating = start;
    //    if (start) StartCoroutine(Animation());
    //}

    //IEnumerator Animation()
    //{
    //    while (animating)
    //    {
    //        Util.Alpha(symbol, 1);
    //        Util.Fade(symbol, animationSpeed);
    //        yield return new WaitForSeconds(animationDelay);
    //    }
    //    Util.Alpha(symbol, 1);
    //    yield break;
    //}

    public enum Type
    {
        GravityBomb,
        Explosion,
        Pull,
        None
    }


}
