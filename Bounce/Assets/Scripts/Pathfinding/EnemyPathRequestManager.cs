using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyPathRequestManager : MonoBehaviour
{
    Queue<PathRequest> requestQueue = new Queue<PathRequest>();
    PathRequest currentRequest;

    static EnemyPathRequestManager instance;
    AStarPathfinding pathfinder;

    bool isProcessing;

    private void Awake()
    {
        instance = this;
        pathfinder = GetComponent<AStarPathfinding>();
    }

    public static void RequestPath(Vector3 start, Vector3 end, Action<Vector3[], bool> callback)
    {
        PathRequest newReq = new PathRequest(start, end, callback);
        instance.requestQueue.Enqueue(newReq);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if(!isProcessing && requestQueue.Count > 0)
        {
            currentRequest = requestQueue.Dequeue();
            isProcessing = true;
            pathfinder.StartFindPath(currentRequest.pathStart, currentRequest.pathEnd);
        }
    }

    public void FinishedProcessing(Vector3[] path, bool success)
    {
        currentRequest.callback(path, success);
        isProcessing = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
