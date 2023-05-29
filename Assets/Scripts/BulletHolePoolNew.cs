using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletHolePoolNew : MonoBehaviour
{
    ObjectPool<GameObject> bulletHolePool;
    [SerializeField] GameObject bulletHole;
    public void Awake()
    {
        bulletHolePool = new ObjectPool<GameObject>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy, true, 30, 1000);
    }

    public void CreateBulletHole(Vector3 holePosition, GameObject hitObj, Vector3 hitNormal)
    {
        GameObject hole = bulletHolePool.Get();
        hole.transform.position = holePosition;
        hole.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hitNormal);
        hole.transform.SetParent(hitObj.transform);
    }

    private void ActionOnDestroy(GameObject obj)
    {
        Destroy(obj.gameObject);
    }

    private void ActionOnRelease(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    private GameObject CreateFunc()
    {
        var hole = Instantiate(bulletHole, transform);
        return hole;

    }
}
