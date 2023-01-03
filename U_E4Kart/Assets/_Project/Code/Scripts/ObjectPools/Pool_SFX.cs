using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pool_SFX : ObjectPoolBase
{
    [SerializeField] private AudioSource prefabBase;

    [SerializeField] private List<AudioSource> objectsSpawned = new List<AudioSource>();

    public static Pool_SFX Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void SpawnObjects()
    {
        foreach (var obj in objectsSpawned)
        {
            DestroyImmediate(obj.gameObject);
        }
        objectsSpawned.Clear();

        for (var i = 0; i < spawnQuantity; i++)
        {
            var spawn = Spawn();
            objectsSpawned.Add(spawn);
            spawn.gameObject.SetActive(false);
        }
    }
    
    private AudioSource Spawn()
    {
        AudioSource spawn = null;
        if (Application.isPlaying)
        {
            spawn = Instantiate(prefabBase, transform);
        }
#if UNITY_EDITOR
        if(!Application.isPlaying)
        {
            spawn = (AudioSource) PrefabUtility.InstantiatePrefab(prefabBase, transform);
            EditorUtility.SetDirty(transform);
        }
#endif
        return spawn;
    }

    public AudioSource GetInstance(Vector3 position)
    {
        AudioSource instance = null;
        if(objectsSpawned.Count > 0)
            instance = objectsSpawned[0];
        else
        {
            var spawn = Spawn();
            objectsSpawned.Add(spawn);
            spawn.gameObject.SetActive(false);
            instance = spawn;
        }
        objectsSpawned.Remove(instance);
        instance.transform.position = position;
        instance.gameObject.SetActive(true);

        return instance;
    }

    public void ReturnInstance(AudioSource returnedObject)
    {
        objectsSpawned.Add(returnedObject);
        returnedObject.gameObject.SetActive(false);
        returnedObject.transform.SetParent(transform);
        returnedObject.transform.localPosition = Vector3.zero;
        returnedObject.pitch = 1f;
    }

    public void ReturnInstanceWhenConcludePlaying(AudioSource instance)
    {
        StartCoroutine(WhilePlaying(instance));
    }

    private IEnumerator WhilePlaying(AudioSource instance)
    {
        yield return new WaitWhile(() => instance.isPlaying);
        ReturnInstance(instance);
    }
}
