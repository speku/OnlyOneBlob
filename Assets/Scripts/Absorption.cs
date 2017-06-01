using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Absorption : MonoBehaviour {

    public float tinyScale = 0.01f;
    public float absorbPercentage = 1;
    public float minimumRadius = 0.15f;
    public bool npcMerge = true;
    public float checkGameStateDelay = 1;

    Rigidbody2D rb;
    CircleCollider2D cc;
    SpriteRenderer sr;
    SettingsManager sm;
    EventManager em;
    PlayerMovement player;
    LayerMask lm;

    bool isPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        lm = LayerMask.NameToLayer("Absorption");
    }


    public void Setup()
    {
        player = FindObjectOfType<PlayerMovement>();
        isPlayer = GetComponent<PlayerMovement>() != null;
        sm = FindObjectOfType<SettingsManager>();
        em = FindObjectOfType<EventManager>();
    }


    void Start () {
        Setup();

        // event handling
        em.AreaChanged += OnAreaChanged;
        OnAreaChanged();
    }

	
	void Update () {
		
	}

    public void OnAreaChanged()
    {
        if (sr == null) return;
        if (!isPlayer) sr.color = player.GetComponent<Absorption>().Area() >= Area() ? sm.absorbableColor : sm.unabsorbableColor;
    }


    public Absorption Create(Vector2 pos, float rotation, float area)
    {
        var a = Instantiate(this, pos, Quaternion.Euler(Vector3.forward * rotation));
        a.Setup();
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
        var newParts = new List<Absorption>();
        var a = Area() / parts;
        if (Radius(a) >= minimumRadius)
        {
            Util.Delay(() => Physics2D.IgnoreLayerCollision(lm, lm, true), passThroughDuration, () => Physics2D.IgnoreLayerCollision(lm, lm, false));
            newParts = Enumerable.Range(1, parts).Select((int _) => Create(transform.position, transform.rotation.z, a)).ToList();
            newParts.ForEach(ab => { ab.transform.Rotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 180))); ab.transform.Translate(ab.transform.up * ab.Radius()); });
            DestroyProper();
            return newParts;
        }
        newParts.Add(this);
        return newParts;
    }

    public float Radius(float area)
    {
        return Mathf.Sqrt(area / Mathf.PI);
    }



    public void Scale(float newArea)
    {
        var a = Area();
        if (a == 0) return;
        transform.localScale = transform.localScale * (newArea / a);
        if (isPlayer)
        {
            em.RaiseAreaChanged();
            player.powerUps.ForEach(p => p.ResetScale());
            player.mousePointer.AdjustScale();
        } else
        {
            OnAreaChanged();
        }
    }

    public void DestroyTiny()
    {
        if (transform.localScale.x < tinyScale) DestroyProper(); // Destroy(gameObject);
    }

    private void DestroyProper()
    {
        em.AreaChanged -= OnAreaChanged;
        CheckGameState();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        //em.AreaChanged -= OnAreaChanged;
    }

    void CheckGameState()
    {
       
            if (isPlayer)
            {
                em.RaiseGameStateChanged(GameManager.GameState.Lost);
            } else if (FindObjectsOfType<Absorption>().Length == 2)
            {
                em.RaiseGameStateChanged(GameManager.GameState.Won);
            }
    }


    //void CheckGameState()
    //{
    //    Util.Delay(null, checkGameStateDelay, () =>
    //     {
    //         if (FindObjectsOfType<PlayerMovement>().Length == 0)
    //         {
    //             em.RaiseGameStateChanged(GameManager.GameState.Lost);
    //         } else if (FindObjectsOfType<Absorption>().Length == 1)
    //         {
    //             em.RaiseGameStateChanged(GameManager.GameState.Won);
    //         }
    //     });
    //}

    public void Absorb(Absorption other, float percentage)
    {
        if (!npcMerge && !(isPlayer || other.isPlayer)) return;

        var area = Area();
        var otherArea = other.Area();

        if (area < otherArea) return;

        float otherNewArea;
        float myNewArea;
        if (area == otherArea && other.isPlayer)
        {
            otherNewArea = Area() + other.Area() * percentage;
            myNewArea = other.Area() * (1 - percentage);
        }
        else
        {
            otherNewArea = other.Area() * (1 - percentage);
            myNewArea = Area() + other.Area() * percentage;
        }
        Scale(myNewArea);
        other.Scale(otherNewArea);
        other.DestroyTiny();
        DestroyTiny();
        //CheckGameState();
    }

    public float Radius()
    {
        return cc.bounds.extents.x;
    }

    public float Area()
    {
        if (cc == null) return 0;
        return Mathf.Pow(Radius(), 2) * Mathf.PI;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        var a = c.gameObject.GetComponent<Absorption>();
        if (a == null) return;
        Absorb(a, absorbPercentage);
    }
}
