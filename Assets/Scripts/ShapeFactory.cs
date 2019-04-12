﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
    [SerializeField]
    private Shape[] _prefabs;
    [SerializeField]
    private Material[] _materials;
    [SerializeField]
    private bool recycle;
    private List<Shape>[] _pools;
    private Scene _poolScene;

    public Shape Get(int shapeId = 0, int materialId = 0)
    {
        Shape instance;
        if (recycle)
        {
            if (_pools == null)
            {
                CreatePools();
            }
            List<Shape> pool = _pools[shapeId];
            int lastIndex = pool.Count - 1;
            if (lastIndex >= 0)
            {
                instance = pool[lastIndex];
                instance.gameObject.SetActive(true);
                pool.RemoveAt(lastIndex);
            }
            else
            {
                instance = Instantiate(_prefabs[shapeId]);
                instance.ShapeId = shapeId;
                SceneManager.MoveGameObjectToScene(instance.gameObject, _poolScene);
            }
        }
        else
        {
            instance = Instantiate(_prefabs[shapeId]);
            instance.ShapeId = shapeId;
        }
        instance.SetMaterial(_materials[materialId],materialId);
        return instance;
    }

    public void Reclaim (Shape shapeToRecycle)
    {
        if (recycle)
        {
            if (_pools == null)
            {
                CreatePools();
            }
            _pools[shapeToRecycle.ShapeId].Add(shapeToRecycle);
            shapeToRecycle.gameObject.SetActive(false);
        }
        else
        {
            Destroy(shapeToRecycle.gameObject);
        }
    }

    public Shape GetRandom()
    {
        return Get(
            Random.Range(0, _prefabs.Length),
            Random.Range(0,_materials.Length));
    }

    private void CreatePools()
    {
        _pools = new List<Shape>[_prefabs.Length];
        for (int i = 0; i < _pools.Length; i++)
        {
            _pools[i] = new List<Shape>();
        }

        if (Application.isEditor)
        {
            _poolScene = SceneManager.GetSceneByName(name);
            if (_poolScene.isLoaded)
            {
                GameObject[] rootObjects = _poolScene.GetRootGameObjects();
                for (int i = 0; i < rootObjects.Length; i++)
                {
                    Shape pooledShape = rootObjects[i].GetComponent<Shape>();
                    if (!pooledShape.gameObject.activeSelf)
                    {
                        _pools[pooledShape.ShapeId].Add(pooledShape);
                    }
                }
                return;
            }
        }

        _poolScene = SceneManager.CreateScene(name);
    }
   
}
