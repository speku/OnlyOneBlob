using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Absorption : MonoBehaviour {

    public float tinyScale = 0.01f;

    [HideInInspector]
    public Rigidbody2D rb;
    CircleCollider2D cc;
    SpriteRenderer sr;
    SettingsManager sm;
    EventManager em;
    GameObject player;
    LayerMask lm;

    bool isPlayer;



	void Start () {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        sm = FindObjectOfType<SettingsManager>();
        em = FindObjectOfType<EventManager>();
        lm = LayerMask.NameToLayer("Absorption");
        player = GameObject.FindGameObjectWithTag("Player");

        
        isPlayer = GetComponent<PlayerMovement>() != null;

        // event handling
        em.AreaChanged += OnAreaChanged;
	}
	
	void Update () {
		
	}

    public void OnAreaChanged(Absorption a)
    {
        if (!isPlayer) sr.color = a.Area() > Area() ? sm.absorbableColor : sm.unabsorbableColor;
    }

    public Absorption Create(Vector2 pos, float rotation, float area)
    {
        var a = Instantiate(this, pos, Quaternion.Euler(Vector3.forward * rotation));
        OnAreaChanged(player.GetComponent<Absorption>());
        return a;
    }


    public void Slice(Vector2 leftPos, float leftRotation, Vector2 rightPos, float rightRotation, float ratio = 0.5f)
    {
        var a = Area();
        var leftArea = a * ratio;
        var rightArea = a - leftArea;

        Destroy(gameObject);
        if (isPlayer) OnAreaChanged(this);

        Create(leftPos, leftRotation, leftArea);
        Create(rightPos, rightRotation, rightArea);
    }


    public List<Absorption> Explode(int parts, float force, float passThroughDuration)
    {
        var a = Area();
        Util.Delay(() => Physics.IgnoreLayerCollision(lm, lm, true), passThroughDuration, () => Physics.IgnoreLayerCollision(lm, lm, false));
        return Enumerable.Range(1, parts).Select((int _) => Create(transform.position, transform.rotation.z, (a / parts) / a)).ToList();
    }



    public void Scale(float newArea)
    {
        transform.localScale = transform.localScale * (newArea / Area());
        if (isPlayer) OnAreaChanged(this);
    }

    public void DestroyTiny()
    {
        if (transform.localScale.x < tinyScale) Destroy(gameObject);
    }

    public void Absorb(Absorption other, float percentage)
    {
        if (!(Area() > other.Area())) return;
        
        var otherNewArea = other.Area() * (1 - percentage);
        var myNewArea = Area() + other.Area() * percentage;

        Scale(myNewArea);
        other.Scale(otherNewArea);
        other.DestroyTiny();
    }

    public float Area()
    {
        
        return Mathf.Pow(cc.radius, 2) * Mathf.PI;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        var absorption = c.gameObject.GetComponent<Absorption>();
        if (absorption == null) return; 
    }
}
