using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float diskHeight = 110f;

    // приватный список — правильная практика
    private List<Disk> disks = new List<Disk>();

    // публичный доступ только для чтения
    public int DiskCount => disks.Count;
    public List<Disk> Disks => disks;

    private void Start()
    {
        Debug.Log("Башня " + name + " инициализирована");
    }

    public void AddDisk(Disk d)
    {
        disks.Add(d);
        d.SetTower(this);
        UpdateStack();
    }

    public void RemoveDisk(Disk d)
    {
        disks.Remove(d);
        UpdateStack();
    }

    public Disk GetTopDisk()
    {
        if (disks.Count == 0)
            return null;

        return disks[disks.Count - 1];
    }

    public void UpdateStack()
    {
        RectTransform towerRT = GetComponent<RectTransform>();

        for (int i = 0; i < disks.Count; i++)
        {
            RectTransform dr = disks[i].GetComponent<RectTransform>();

            dr.anchoredPosition = new Vector2(
                towerRT.anchoredPosition.x,
                towerRT.anchoredPosition.y + diskHeight * i
            );
        }
    }

    // ПРОВЕРКА ПРАВИЛЬНОСТИ СОРТИРОВКИ
    public bool IsCorrectlySorted()
    {
        if (disks.Count == 0)
            return false;

        // нижний диск должен быть самым большим
        for (int i = 0; i < disks.Count - 1; i++)
        {
            if (disks[i].size < disks[i + 1].size)
                return false;
        }

        return true;
    }

}
