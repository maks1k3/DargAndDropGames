using UnityEngine;

public class RandomStart : MonoBehaviour
{
    void Start()
    {
        Tower[] towers = FindObjectsOfType<Tower>();
        Disk[] disks = FindObjectsOfType<Disk>();

        Shuffle(disks);

        foreach (Disk d in disks)
        {
            Tower t = towers[Random.Range(0, towers.Length)];
            t.AddDisk(d);
        }
    }

    void Shuffle(Disk[] a)
    {
        for (int i = a.Length - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            (a[i], a[j]) = (a[j], a[i]);
        }
    }
}
