using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerMovement : MonoBehaviour {

    public float forwardSpeed = 10;
    public float rotationSpeed = 10;
    public Cone cone;
    public Transform gravityBombPosition;
    public Transform pullPosition;
    public Transform explosionPosition;

    PowerUp.Type powerUp = PowerUp.Type.None;
    [HideInInspector]
    public Pull pull;
    [HideInInspector]
    public GravityBomb gravity;
    [HideInInspector]
    public Explosion explosion;
    public List<PowerUp> powerUps = new List<PowerUp>();

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tag = "Player";
    }

    void Start () {
        
	}
	
	void Update () {
        rb.AddForce(Input.GetAxis("Vertical") * transform.up * forwardSpeed * Time.deltaTime, ForceMode2D.Force);
        transform.Rotate(Vector3.back * Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Equip(PowerUp.Type.Pull);
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Equip(PowerUp.Type.Explosion);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Equip(PowerUp.Type.GravityBomb);
        }
        switch (powerUp)
        {
            case PowerUp.Type.Pull:
                if (Input.GetMouseButton(1)) pull.PullIn();
                break;
            case PowerUp.Type.Explosion | PowerUp.Type.GravityBomb:
                explosion.GetComponent<PowerUp>().Place();
                break;
        }

    }

    void Equip(PowerUp.Type type)
    {
        if (powerUp == type)
        {
            powerUp = PowerUp.Type.None;
            powerUps.ForEach(p => p.Unselect());
        } else
        {
            powerUps.Where(p => p.type == type).ToList().ForEach(p => { p.Select(); powerUp = type; });
            powerUps.Where(p => p.type != type).ToList().ForEach(p => p.Unselect());
        }
    }


}
