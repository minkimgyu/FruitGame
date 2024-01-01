using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseDropObject : MonoBehaviour
{
    public abstract void SpawnRequested();

    public abstract void MergeRequested();

    public abstract void DropRequested();

    public abstract void DestroyRequested(); // ������ ����� ���� �ı�
}
