using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class EnemyPathRequestManager : MonoBehaviour
{
    //Queue<PathRequest> requestQueue = new Queue<PathRequest>();
    //PathRequest currentRequest;
    Queue<PathResult> results = new Queue<PathResult>();

    static EnemyPathRequestManager instance;
    AStarPathfinding pathfinder;

    //bool isProcessing;

    private void Awake()
    {
        instance = this;
        pathfinder = GetComponent<AStarPathfinding>();
    }

    //public static void RequestPath(Vector3 start, Vector3 end, Action<Vector3[], bool> callback)
    //{
    //    //PathRequest newReq = new PathRequest(start, end, callback);
    //    //instance.requestQueue.Enqueue(newReq);
    //    //instance.TryProcessNext();
    //}
    public static void RequestPath(PathRequest request)
    {
        ThreadStart threadStart = delegate
        {
            instance.pathfinder.FindPath(request, instance.FinishedProcessing);
        };
        threadStart.Invoke();
    }

    private void Update()
    {
        if(results.Count > 0)
        {
            int itemsInQueue = results.Count;
            lock(results)
            {
                for(int i = 0; i < itemsInQueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success);
                }
            }
        }
    }

    //void TryProcessNext()
    //{
    //    if(!isProcessing && requestQueue.Count > 0)
    //    {
    //        currentRequest = requestQueue.Dequeue();
    //        isProcessing = true;
    //        pathfinder.StartFindPath(currentRequest.pathStart, currentRequest.pathEnd);
    //    }
    //}

    public void FinishedProcessing(PathResult result)
    {
        lock(results)
        {
            results.Enqueue(result);
        }


        //currentRequest.callback(path, success);
        //isProcessing = false;
        //TryProcessNext();
    }
}

public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;

    public PathResult(Vector3[] _path, bool _success, Action<Vector3[], bool> _callback)
    {
        path = _path;
        success = _success;
        callback = _callback;
    }
}

public struct PathRequest
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
