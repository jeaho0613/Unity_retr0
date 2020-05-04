using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGenerator : MonoBehaviour
{
    public GameObject[] propPrefabs; // 프리팹
    public int count = 100; // 생성 수

    private BoxCollider area; // 생성 될 범위
    private List<GameObject> props = new List<GameObject>(); // 프롭을 출력하기 위한 리스트

    private void Start()
    {
        area = GetComponent<BoxCollider>(); // 초기화

        for (int i = 0; i < count; i++)
        {
            Spawn();
        }

        area.enabled = false; // 최초 1번 생성 후 비활성화
    }

    private void Spawn()
    {
        int selection = Random.Range(0, propPrefabs.Length);

        GameObject selectedPrefab = propPrefabs[selection];

        Vector3 spawnPos = GetRandomPosition();

        GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        props.Add(instance);
    }

    // 랜덤한 생성 위치
    private Vector3 GetRandomPosition()
    {
        Vector3 basePosition = transform.position; // 오브젝트 원지점
        Vector3 size = area.size; // 박스 콜라이더 사이즈

        // x,y,z의 랜덤한 위치를 각각 구한 후 생성
        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);
        float posZ = basePosition.z + Random.Range(-size.z / 2f, size.z / 2f);

        Vector3 spawnPos = new Vector3(posX, posY, posZ);

        return spawnPos;
    }

    public void Reset()
    {
        for (int i = 0; i < props.Count; i++)
        {
            props[i].transform.position = GetRandomPosition();
            props[i].SetActive(true);
        }
    }
}
