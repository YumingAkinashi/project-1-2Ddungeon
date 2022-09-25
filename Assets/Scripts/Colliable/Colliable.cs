using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliable : MonoBehaviour
{
    public ContactFilter2D filter; // ����Collider�����I��
    private BoxCollider2D boxCollider; // �x�s���o�Ӹ}�������⪺Collider��T���ܶ�
    private Collider2D[] hits = new Collider2D[10]; // �s��I����T��array

    protected virtual void Start()
    {

        boxCollider = GetComponent<BoxCollider2D>(); // ���o���o�Ӹ}�������⪺Collider��T

    }
    protected virtual void Update()
    {

        // collision work �����M�o�Ө���I����Colliders���x�s��T
        boxCollider.OverlapCollider(filter, hits);

        for (int i = 0; i < hits.Length; i++)
        {

            if (hits[i] == null) // hits[i]�N�O�C������I�����t�~�@�Ө���
                continue;

            OnCollide(hits[i]);
            
            // clean up the array
            hits[i] = null;

        }

    }
    protected virtual void OnCollide(Collider2D coll)
    {

        Debug.Log("OnCollide was not implemented on " + this.name);
    }

}
