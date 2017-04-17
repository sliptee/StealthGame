using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Highscore
{
    int maxlist;

    List<HSItem> HSList = new List<HSItem>();
    public Highscore(int MaxInList)
    {
        maxlist = MaxInList;
    }

    public void Add(string name, int points)
    {
        HSItem item = new HSItem();
        item.Points = points;
        item.Name = name;
        HSList.Add(item);
        Sort();
    }
    public void Sort()
    {
        HSList = HSList.OrderByDescending(a => a.Points).ToList();
        while (HSList.Count > maxlist)
        {
            HSList.RemoveAt(HSList.Count - 1);
        }
    }
    public void Print()
    {
        foreach (HSItem item in HSList)
        {
            Console.WriteLine(item.Name + "                 " + item.Points);
        }
    }
}
public class HSItem
{
    public string Name { get; set; }
    public int Points { get; set; }
}



