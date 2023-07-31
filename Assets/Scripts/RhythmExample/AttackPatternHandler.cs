using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPatternHandler : MonoBehaviour
{
    public float time; // Please manipulate this with TimeKeeper

    public float startBeat; // What beat is the relative 'zero' (for starting patterns on custom times)
    public TimeKeeper timeKeeper;

    [Header("Key Data")]
    public AttackPattern attackPattern;
    public GameObject keyPrefab;
    public List<GameObject> keyObjects;
    public GameObject barPrefab;
    public List<GameObject> barObjects;
    public float lerpHitbox; // What area of lerp the hitbox can be hit 

    [Header("Object Anchors")]
    public Transform spawner;
    public Transform target;
    public float spawnTargetBeats;
    public float beatOffset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time = timeKeeper.beatsAnalogue;
        UpdateKeyObjects(time);
    }

    public void DeleteAllObjects()
    {
        print("DeleteAllObjects");

        foreach (GameObject objectToDestroy in keyObjects)
        {
            Destroy(objectToDestroy);
        }
        keyObjects.Clear();

        foreach (GameObject objectToDestroy in barObjects)
        {
            Destroy(objectToDestroy);
        }
        barObjects.Clear();

    }

    // Spawns keys (+barPrefab) in their positions relative to the timekeeper whenever you run the functioni
    public void Activate()
    {

        DeleteAllObjects();

    }
    
    // Function that updates the keyObjects (+ barPrefabs)
    public void UpdateKeyObjects(float timeBeats)
    {
        // Update keyObjects
        for (int i = 0; i < keyObjects.Count; i++)
        {
            KeyObject keyObject = keyObjects[i].GetComponent<KeyObject>();
            keyObject.UpdatePosition(timeBeats);
        }

        // Update barObjects
        for (int i = 0; i < barObjects.Count; i++)
        {
            KeyObject barObject = barObjects[i].GetComponent<KeyObject>();
            barObject.UpdatePosition(timeBeats);
        }


    }

    public void CreatePatternOnBeat(AttackPattern attackPattern, float beat)
    {
        // add beats to list of beats and increase their times by beat
        for (int i = 0; i < attackPattern.beats.Count; i++)
        {
            // Create a KeyObject object and fill in the data, add it to a list
            GameObject spawnedKeyPrefab = Instantiate(keyPrefab);
            keyObjects.Add(spawnedKeyPrefab);

            // Add the variables to the spawned KeyObjects to help it calculate it's position when feeding in time
            KeyObject keyObject = spawnedKeyPrefab.GetComponent<KeyObject>();

            keyObject.isKeyObject = true;
            keyObject.beatOfThisKey = attackPattern.beats[i] + beat;
            keyObject.key = attackPattern.keys[i];
            keyObject.spawner = spawner;
            keyObject.target = target;
            keyObject.spawnTargetBeats = spawnTargetBeats;
            keyObject.lerpHitbox = lerpHitbox;
            Vector2.LerpUnclamped(spawner.position, target.position, keyObject.beatOfThisKey);
        }

        // Find the last beat of the attack and round up to find the total number of bars that need to be displayed
        int barCount = (int)Mathf.Ceil(attackPattern.beats[attackPattern.beats.Count - 1]) + 1;
        for (int i = 0; i < barCount; i++)
        {
            GameObject spawnedBarPrefab = Instantiate(barPrefab);
            barObjects.Add(spawnedBarPrefab);

            // Add the variables to the spawned BarObjects to help it calculate it's position when feeding in the beatCount of the music
            KeyObject barObject = spawnedBarPrefab.GetComponent<KeyObject>();

            barObject.isKeyObject = false;
            barObject.beatOfThisKey = i + beatOffset + beat;
            barObject.spawner = spawner;
            barObject.target = target;
            barObject.spawnTargetBeats = spawnTargetBeats;
            Vector2.LerpUnclamped(spawner.position, target.position, barObject.beatOfThisKey);
        }
    }


    public void CreateAttackPatternOnNextBar(AttackPattern attackPattern, int barsAheadToPlace)
    {
        CreatePatternOnBeat(attackPattern, timeKeeper.beatsPerBar * (timeKeeper.currentBar + barsAheadToPlace));
    }


    public void ClearKeyObjects()
    {
        foreach (GameObject keyObject in keyObjects)
        {
            Destroy(keyObject);
        }
        keyObjects.Clear();
        
        foreach (GameObject barObject in barObjects)
        {
            Destroy(barObject);
        }
        barObjects.Clear();
    }





}
