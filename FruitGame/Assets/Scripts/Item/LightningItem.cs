using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningItem : BaseItem
{
    public override void OnDrop()
    {
        // ���⿡ ����� ����
        // �Ʒ��� ������Ʈ�� Ȯ���ؼ� �ı���Ű��

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 30, LayerMask.GetMask("TrickleOnLand"));
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 30, Color.blue, 3);
        if (hit.collider != null && hit.collider.CompareTag("Trickle"))
        {
            Destroy(hit.collider.gameObject);
        }

        Debug.Log("Drop");
        base.OnDrop();
    }

    public override void OnHighlight()
    {
    }

    public override void OnLand()
    {
    }

    public override void OnReady()
    {
    }

    public override void OnSpawn()
    {
    }
}
