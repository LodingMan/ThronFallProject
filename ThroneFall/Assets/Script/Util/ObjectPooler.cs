using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    private List<PoolObject> Pool= new();
    public List<PoolObject> RegistedObjects;

    static public ObjectPooler instance;
    
    private void Start()
    {
        instance = this.GetComponent<ObjectPooler>();
    }

    public PoolObject CreateObject(PoolObject poolObj, Vector3 position)
    {
        foreach (var registedObject in RegistedObjects)
        {
            if (registedObject.objectName == poolObj.objectName)
            {
                //var obj = parent == null ? Instantiate(poolObj) : Instantiate(poolObj,parent);
                var obj = Instantiate(registedObject,position,Quaternion.identity);
                Pool.Add(obj);
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        return null;
    }
    public PoolObject CreateObject(string name, Vector3 position)
    {
        foreach (var registedObject in RegistedObjects)
        {
            if (registedObject.objectName == name)
            {
                var obj = Instantiate(registedObject,position,Quaternion.identity);
                //var obj = parent == null ? Instantiate(registedObject) : Instantiate(registedObject,parent);
                Pool.Add(obj);
                obj.transform.SetParent(this.transform);
                return obj;
            }
        }

        return null;
    }

    
    public PoolObject GetObjectPool(PoolObject obj,Vector3 position, Transform parent = null)
    { 
        PoolObject poolObject;

        if (Pool.Count == 0)
        {
            poolObject = CreateObject(obj,position);
        }
        else
        {
            poolObject = FindPool(obj,position);
        }
        poolObject.gameObject.SetActive(true);
        if (parent != null)
        {
            poolObject.transform.SetParent(parent);
        }
        poolObject.isOn = true;
        return poolObject;
    }
    public PoolObject GetObjectPool(string poolID,Vector3 position, Transform parent = null)
    {
        PoolObject poolObject;
        if (Pool.Count == 0)
        {
            poolObject = CreateObject(poolID,position);
        }
        else
        {
            poolObject = FindPool(poolID,position);
        }
        
        poolObject.gameObject.SetActive(true);
        if (parent != null)
        {
            poolObject.transform.SetParent(parent);
        }
        poolObject.isOn = true;
        return poolObject;
    }

    public PoolObject FindPool(PoolObject obj,Vector3 position)
    {
        var findObj = Pool.Find(p =>
        {
            if (p.objectName == obj.objectName && p.isOn == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        });

        if (findObj != null)
        {
            findObj.transform.position = position;
            return findObj;
        }
        else
        {
            return CreateObject(obj,position);
        }
        
    }
    
    public PoolObject FindPool(string name,Vector3 position)
    {
        var findObj = Pool.Find(p =>
        {
            if (p.objectName == name && p.isOn == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        });

        if (findObj != null)
        {
            findObj.transform.position = position;
            return findObj;
        }
        else
        {
            return CreateObject(name,position);
        }
        
    }

    public static void ReturnPool(PoolObject poolObject)
    {
        poolObject.isOn = false;
        poolObject.gameObject.SetActive(false);
        poolObject.transform.position = Vector3.zero;
        poolObject.transform.SetParent(instance.transform);
    }
}
