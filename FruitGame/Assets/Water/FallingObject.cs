using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    Rigidbody2D _rigid;
    public Rigidbody2D Rigid { get { return _rigid; } }

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }
}
