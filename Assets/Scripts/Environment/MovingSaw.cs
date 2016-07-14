using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NotFixSaw : MonoBehaviour
{
    public enum FollowType
    {
        MoveTowards,
        Lerp
    }

    public FollowType Type = FollowType.MoveTowards;
    public PathDefinition Path;
    public float Speed = 1.0f;
    public float MinDistance = 0.1f;
    public Vector3 Velocity = Vector3.zero;

    private IEnumerator<Transform> _currentPoint;

    // Use this for initialization
    void Awake()
    {
        if (Path == null)
        {
            Debug.LogError("Path can not be null ", gameObject);
        }

        _currentPoint = Path.GetPathEnumerator();
        _currentPoint.MoveNext();

        if (_currentPoint.Current == null)
            return;

        transform.position = _currentPoint.Current.position;
    }

    void Update()
    {
        if (_currentPoint == null || _currentPoint.Current == null)
            return;
        if (Type == FollowType.MoveTowards)
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentPoint.Current.position, Time.deltaTime * Speed);
            Velocity = (_currentPoint.Current.position - transform.position).normalized * Speed;
        }
        else if (Type == FollowType.Lerp)
        {
            transform.position = Vector3.Lerp(transform.position, _currentPoint.Current.position, Time.deltaTime * Speed);
            Velocity = (_currentPoint.Current.position - transform.position).normalized * Speed;
        }

        float distanceSquard = (transform.position - _currentPoint.Current.position).sqrMagnitude;
        if (distanceSquard < MinDistance * MinDistance)
        {
            _currentPoint.MoveNext();
        }
    }

}
