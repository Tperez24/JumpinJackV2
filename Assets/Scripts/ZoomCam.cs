using PlayerComponents;
using UnityEngine;

public class ZoomCam : MonoBehaviour
{
    private Transform[] _players;
    public float minDistanceZ = 7.5f;
    public float minDistanceX = -2f;
    public float maxDistanceZ = 13.5f;
    public float maxDistanceX = 2f;

    private float _xMin, _xMax;

    public void Initialize()
    {
        var allPlayers = FindObjectsOfType<Player>();
        _players = new Transform[allPlayers.Length];

        for (var i = 0; i < allPlayers.Length; i++) _players[i] = allPlayers[i].gameObject.transform;
    }

    private void LateUpdate()
    {
        if (_players.Length == 0) return;

        _xMin = _xMax = _players[0].position.x;

        foreach (var t in _players)
        {
            if (t.position.x < _xMin) _xMin = t.position.x;
            if (t.position.x > _xMax) _xMax = t.position.x;
        }

        var xMiddle = (_xMin + _xMax) / 2;

        var distance = Mathf.Abs(Vector2.Distance(_players[0].position, _players[1].position));
        if (distance < minDistanceZ) distance = minDistanceZ;
        if (distance > maxDistanceZ) distance = maxDistanceZ;

        if (xMiddle < minDistanceX) xMiddle = minDistanceX;
        if (xMiddle > maxDistanceX) xMiddle = maxDistanceX;

        transform.position = new Vector3(xMiddle, 4, distance);
    }
}
