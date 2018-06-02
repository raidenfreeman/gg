using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using InControl;

public class PlayerController : MonoBehaviour
{

    public Text hAxis;
    public Text vAxis;
    public Text hAxis2;
    public Text vAxis2;

    [SerializeField]
    public float speed = 3f;

    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField]
    private Transform model;

    private Vector3 movement;

    void Awake()
    {
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    }

    void FixedUpdate()
    {
        // Move the player around the scene.
        Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void Move(float h, float v)
    {
        var mv = new Vector3(h, 0, v).normalized;
        _rigidbody.velocity = mv * speed;// movement;

        if (mv != Vector3.zero)
        {
            model.rotation = Quaternion.LookRotation(mv, Vector3.up);
        }
    }
}
