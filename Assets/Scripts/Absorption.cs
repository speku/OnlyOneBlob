using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Absorption : MonoBehaviour {

    public float tinyScale = 0.01f;
    public float absorbPercentage = 1;

    Rigidbody2D rb;
    CircleCollider2D cc;
    SpriteRenderer sr;
    SettingsManager sm;
    EventManager em;
    GameObject player;
    LayerMask lm;

    bool isPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        lm = LayerMask.NameToLayer("Absorption");
        isPlayer = GetComponent<PlayerMovement>() != null;
    }


    void Start () {
        sm = FindObjectOfType<SettingsManager>();
        em = FindObjectOfType<EventManager>();
        player = GameObject.FindGameObjectWithTag("Player");

        // event handling
        em.AreaChanged += OnAreaChanged;
        OnAreaChanged();
	}
	
	void Update () {
		
	}

    public void OnAreaChanged()
    {
        if (sr == null) return;
        if (!isPlayer) sr.color = player.GetComponent<Absorption>().Area() > Area() ? sm.absorbableColor : sm.unabsorbableColor;
    }


    public Absorption Create(Vector2 pos, float rotation, float area)
    {
        var a = Instantiate(this, pos, Quaternion.Euler(Vector3.forward * rotation));
        a.Scale(area);
        return a;
    }


    public void Slice(Vector2 leftPos, float leftRotation, Vector2 rightPos, float rightRotation, float ratio = 0.5f)
    {
        var a = Area();
        var leftArea = a * ratio;
        var rightArea = a - leftArea;

        Destroy(gameObject);
        if (isPlayer) OnAreaChanged();

        Create(leftPos, leftRotation, leftArea);
        Create(rightPos, rightRotation, rightArea);
    }


    public List<Absorption> Explode(int parts, float passThroughDuration)
    {
        var a = Area();
        Util.Delay(() => Physics2D.IgnoreLayerCollision(lm, lm, true), passThroughDuration, () => Physics.IgnoreLayerCollision(lm, lm, false));
        var newParts = Enumerable.Range(1, parts).Select((int _) => Create(transform.position, transform.rotation.z, a / parts)).ToList();
        DestroyProper();
        return newParts;
    }



    public void Scale(float newArea)
    {
        transform.localScale = transform.localScale * (newArea / Area());
        if (isPlayer)
        {
            em.RaiseAreaChanged();
        } else
        {
            OnAreaChanged();
        }
    }

    public void DestroyTiny()
    {
        if (transform.localScale.x < tinyScale) Destroy(gameObject);
    }

    private void DestroyProper()
    {
        em.AreaChanged -= OnAreaChanged;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        //em.AreaChanged -= OnAreaChanged;
    }

    public void Absorb(Absorption other, float percentage)
    {
        if (!(Area() >= other.Area())) return;
        
        var otherNewArea = other.Area() * (1 - percentage);
        var myNewArea = Area() + other.Area() * percentage;

        Scale(myNewArea);
        other.Scale(otherNewArea);
        other.DestroyTiny();
        DestroyTiny();
    }

    public float Area()
    {
        if (cc == null) return 0;
        return Mathf.Pow(cc.bounds.extents.x, 2) * Mathf.PI;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        var a = c.gameObject.GetComponent<Absorption>();
        if (a == null) return;
        Absorb(a, absorbPercentage);
    }
}
