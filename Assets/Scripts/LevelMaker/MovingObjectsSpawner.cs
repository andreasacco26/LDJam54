using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingObjectsSpawner: MonoBehaviour {

    public float streetWidth = 4;
    public int maxNumberOfItems = 4;
    public float speed = 5;
    public float minSpawnTime = 2;
    public float maxSpawnTime = 4;
    public GameObject[] itemsToSpawn;

    private float currentSpawnTime = 2;
    private readonly List<Transform> itemsToMove = new();

    void Start() {
    }

    void Update() {
        MoveItems();
        currentSpawnTime -= Time.deltaTime;
        if (currentSpawnTime <= 0) {
            Spawn();
            currentSpawnTime = 2;
        }
    }

    private void Spawn() {
        var numberOfItemsToRemove = Random.Range(0, maxNumberOfItems);
        var availablePositions = Enumerable.Range(0, maxNumberOfItems).ToList();

        while (numberOfItemsToRemove > 0) {
            var toRemove = Random.Range(0, availablePositions.Count - 1);
            availablePositions.RemoveAt(toRemove);
            numberOfItemsToRemove--;
        }

        foreach (int position in availablePositions) {
            var itemPosition = positionFromIndex(position);
            var item = itemsToSpawn[Random.Range(0, itemsToSpawn.Length - 1)];
            var instantiatedItem = Instantiate(item, itemPosition, Quaternion.identity);
            itemsToMove.Add(instantiatedItem.transform);
        }
    }

    private Vector3 positionFromIndex(int index) {
        var position = transform.position;
        position.x = (streetWidth / maxNumberOfItems * index) - (position.x + streetWidth * 0.5f);
        position.x += streetWidth / maxNumberOfItems * 0.5f;
        return position;
    }

    private void MoveItems() {
        foreach (Transform t in itemsToMove) {
            if (t == null) continue;
            t.Translate(0, 0, -speed * Time.deltaTime);
        }
        CleanItemsToMove();
    }

    private void CleanItemsToMove() {
        itemsToMove.Remove(null);
    }
}
