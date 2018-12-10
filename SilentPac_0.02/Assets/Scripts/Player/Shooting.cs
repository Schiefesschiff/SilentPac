using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private LineRenderer lRend;
    public Transform transformPointA;
    public Transform transformPointB;
    public float length = 5f;
    private readonly int pointsCount = 5;
    private readonly int half = 2;
    private float randomness;
    private Vector3[] points;

    private readonly int pointIndexA = 0;
    private readonly int pointIndexB = 1;
    private readonly int pointIndexC = 2;
    private readonly int pointIndexD = 3;
    private readonly int pointIndexE = 4;

    private readonly string mainTexture = "_MainTex";
    private Vector2 mainTextureScale = Vector2.one;
    private Vector2 mainTextureOffset = Vector2.one;

    private float timer;
    private float timerTimeOut = 0.05f;

    public bool isShooting;

    private void Start()
    {
        lRend = GetComponent<LineRenderer>();
        points = new Vector3[pointsCount];
        lRend.positionCount = pointsCount;
    }

    private void Update()
    {
        if (isShooting)
        {
            StartShooting();
        }

    }
    
    public void IsShooting()        // control for animation event
    {
        isShooting = !isShooting;
        if (isShooting)
        {
            lRend.enabled = true;
        }
        else
        {
            lRend.enabled = false;
        }
    }


    public void StartShooting()
    {
        RaycastHit hit;

        if (Physics.Raycast(transformPointA.position, transformPointA.TransformDirection(Vector3.forward), out hit, length))
        {
            transformPointB.position = hit.point;
            Debug.DrawRay(transformPointA.position, hit.point, Color.yellow);
        }
        else
        {
            Debug.DrawRay(transformPointA.position, hit.point, Color.yellow);
            print("There is something in front of the object!");
            //transformPointB.position = new Vector3 (transformPointA.localPosition.x , transformPointA.localPosition.y, transformPointA.localPosition.z + length);
        }

        CalculatePoints();
    }

    private void CalculatePoints()
    {
        timer += Time.deltaTime;

        if (timer > timerTimeOut)
        {
            timer = 0;

            points[pointIndexA] = transformPointA.position;
            points[pointIndexE] = transformPointB.position;
            points[pointIndexC] = GetCenter(points[pointIndexA], points[pointIndexE]);
            points[pointIndexB] = GetCenter(points[pointIndexA], points[pointIndexC]);
            points[pointIndexD] = GetCenter(points[pointIndexC], points[pointIndexE]);

            float distance = Vector3.Distance(transformPointA.position, transformPointB.position) / points.Length;
            mainTextureScale.x = distance;
            mainTextureOffset.x = Random.Range(-randomness, randomness);
            lRend.material.SetTextureScale(mainTexture, mainTextureScale);
            lRend.material.SetTextureOffset(mainTexture, mainTextureOffset);

            randomness = distance / (pointsCount * half);

            SetRandomness();

            lRend.SetPositions(points);
        }
    }

    private void SetRandomness()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (i != pointIndexA && i != pointIndexE)
            {
                points[i].x += Random.Range(-randomness, randomness);
                points[i].y += Random.Range(-randomness, randomness);
                points[i].z += Random.Range(-randomness, randomness);
            }
        }
    }

    private Vector3 GetCenter(Vector3 a, Vector3 b)
    {
        return (a + b) / half;
    }
}
