using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject pipePrefab;
    public GameObject marioPerfab;
    public float spawnRatepipes = 1f;
    public float minpipesHeight = - 1f;
    public float maxpipesHeight = 1f;

    public float spawnmarioRate = 1f;
    public float minmarioHeight = - 1f;
    public float maxmarioHeight = 1f;

    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawnpipes), spawnRatepipes, spawnRatepipes);
        InvokeRepeating(nameof(Spawnmario), spawnmarioRate, spawnmarioRate);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawnpipes));
        CancelInvoke(nameof(Spawnmario));
    }


    private void Spawnmario()
    {
        GameObject marios = Instantiate(marioPerfab, transform.position, Quaternion.identity);
        marios.transform.position += Vector3.up * Random.Range(minmarioHeight, maxmarioHeight);
    }

    private void Spawnpipes()
    {
        GameObject pipes = Instantiate(pipePrefab, transform.position, Quaternion.identity);
        pipes.transform.position += Vector3.left * 5;
        pipes.transform.position += Vector3.up * Random.Range(minpipesHeight, maxpipesHeight);
    }

//     private void Spawnmario()
//     {
//         GameObject marios = Instantiate(marioPerfab, transform.position, Quaternion.identity);
//         marios.transform.position += Vector3.up * Random.Range(minmarioHeight, maxmarioHeight);
//     }
}
