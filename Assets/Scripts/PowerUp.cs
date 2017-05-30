using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PowerUp : MonoBehaviour {

    List<SpriteRenderer> renderers;
    Mouse mousePointer;
    Type type;


    private void Start()
    {
        type = GetComponent<GravityBomb>() != null ? Type.GravityBomb : GetComponent<Explosion>() != null ? Type.Explosion : Type.Pull;
        renderers = GetComponentsInParent<SpriteRenderer>().ToList();
        mousePointer = FindObjectOfType<Mouse>();
    }

    public void PickUp(PlayerMovement player)
    {
        transform.SetParent(player.transform);
        Show(false);
    }

    void Show(bool show)
    {
        renderers.ForEach(r => r.enabled = show);
    }

        

    public void Select()
    {
        switch(type){
            case Type.GravityBomb:
                transform.SetParent(mousePointer.transform);
                break;
            case Type.Explosion:
                break;
                transform.SetParent(mousePointer.transform);
            case Type.Pull:
                GetComponent<Pull>().ShowCone();
                break;
        }
    }

    public enum Type
    {
        GravityBomb,
        Explosion,
        Pull
    }


}
