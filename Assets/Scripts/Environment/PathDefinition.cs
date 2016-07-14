using UnityEngine;
using System.Collections.Generic;
using System;

public class PathDefinition : MonoBehaviour
{
    public Transform[] Points;
    

    public IEnumerator<Transform> GetPathEnumerator()
    {
        if (Points == null || Points.Length < 1)
            yield break;

        int direction = 1;
        int index = 0;

        while (true)
        {
            yield return Points[index];

            if (Points.Length == 1)
                continue;

            if (index <= 0)
                direction = 1;
            else if (index >= Points.Length - 1)
                direction = -1;

            index = index + direction;
        }
    }


   


    public Transform GetMaxXPoint()
    {
        Transform max = Points[0];
        for (int i = 0; i < Points.Length; i++)
        {
            if (Points[i].position.x > max.position.x)
            {
                max = Points[i];
            }
        }
        return max;
    }

    public Transform GetMinXPoint()
    {
        Transform min = Points[0];
        for (int i = 0; i < Points.Length; i++)
        {
            if (Points[i].position.x < min.position.x)
            {
                min = Points[i];
            }
        }
        return min;
    }

    public void OnDrawGizmos()
    {
        if (Points == null)
            return;

        if (Points.Length < 2)
            return;

        for (var i = 1; i < Points.Length; i++)
        {
            Gizmos.DrawLine(Points[i - 1].position, Points[i].position);
        }
    }
}
