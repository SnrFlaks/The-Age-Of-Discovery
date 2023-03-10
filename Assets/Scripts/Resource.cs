using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cysharp.Threading.Tasks;

public class Resource : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Pipes pipes;
    private TileBase[] directionTile;
    private TileBase[][] turnPipes = new TileBase[4][];
    private TileBase[] teePipes = new TileBase[4];
    private TileBase crossPipes;
    private Vector3 targetPosition;
    private Collider2D startPipe;
    private Collider2D colliderPipe;
    private Collider2D Collider
    {
        get { return colliderPipe; }
        set { colliderPipe = value; }
    }
    private int lastSide;
    private Rigidbody2D rb;
    private CancellationTokenSource cancellationTokenSource;

    private async void Awake()
    {
        pipes = transform.parent.parent.GetChild(6).GetComponent<Pipes>();
        directionTile = pipes.DirectionTile;
        turnPipes[0] = pipes.TurnPipesUp;
        turnPipes[1] = pipes.TurnPipesRight;
        turnPipes[2] = pipes.TurnPipesDown;
        turnPipes[3] = pipes.TurnPipesLeft;
        teePipes = pipes.TeePipes;
        crossPipes = pipes.CrossPipes;
        rb = GetComponent<Rigidbody2D>();
        cancellationTokenSource = new CancellationTokenSource();
        await ResourceTask(cancellationTokenSource.Token);
    }
    private async UniTask ResourceTask(CancellationToken cancellationToken)
    {
        while (true)
        {
            if (this != null && Collider != null && Collider.CompareTag("Pipe"))
            {
                if (startPipe == null) startPipe = Collider;
                Pipe currentPipe = Collider.GetComponent<Pipe>();
                Transform[] pipe = currentPipe.GetNeighborPipes(0, 0);
                int number = 0;
                int neighbors = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (currentPipe.PipesOutput[i]) number++;
                    if (pipe[i]) neighbors++;
                }
                if (number == 4) for (int i = 0; i < 4; i++) currentPipe.PipesOutput[i] = false;
                if (Vector3.Distance(transform.position, targetPosition) < 0.05f || targetPosition == new Vector3())
                {
                    Vector3 targetPositionBefore = targetPosition;
                    for (int i = 0; i < directionTile.Length; i++)
                    {
                        if (currentPipe.currentTile == directionTile[i])
                        {
                            targetPosition = pipe[i].transform.position;
                            lastSide = i;
                            break;
                        }
                        else if (i == 3 && currentPipe.currentTile != directionTile[i])
                        {
                            if (lastSide == 0)
                            {
                                for (int j = 0; j < turnPipes[0].Length; j++)
                                {
                                    if (currentPipe.currentTile == turnPipes[0][j] && j % 2 == 0)
                                    {
                                        targetPosition = pipe[3].transform.position;
                                        lastSide = 3;
                                    }
                                    else if (currentPipe.currentTile == turnPipes[0][j] && j % 2 == 1)
                                    {
                                        targetPosition = pipe[1].transform.position;
                                        lastSide = 1;
                                    }
                                }
                            }
                            if (lastSide == 1)
                            {
                                for (int j = 0; j < turnPipes[1].Length; j++)
                                {
                                    if (currentPipe.currentTile == turnPipes[1][j] && j % 2 == 0)
                                    {
                                        targetPosition = pipe[2].transform.position;
                                        lastSide = 2;
                                    }
                                    else if (currentPipe.currentTile == turnPipes[1][j] && j % 2 == 1)
                                    {
                                        targetPosition = pipe[0].transform.position;
                                        lastSide = 0;
                                    }
                                }
                            }
                            if (lastSide == 2)
                            {
                                for (int j = 0; j < turnPipes[2].Length; j++)
                                {
                                    if (currentPipe.currentTile == turnPipes[2][j] && j % 2 == 0)
                                    {
                                        targetPosition = pipe[3].transform.position;
                                        lastSide = 3;
                                    }
                                    else if (currentPipe.currentTile == turnPipes[2][j] && j % 2 == 1)
                                    {
                                        targetPosition = pipe[1].transform.position;
                                        lastSide = 1;
                                    }
                                }
                            }
                            if (lastSide == 3)
                            {
                                for (int j = 0; j < turnPipes[3].Length; j++)
                                {
                                    if (currentPipe.currentTile == turnPipes[3][j] && j % 2 == 0)
                                    {
                                        targetPosition = pipe[2].transform.position;
                                        lastSide = 2;
                                    }
                                    else if (currentPipe.currentTile == turnPipes[3][j] && j % 2 == 1)
                                    {
                                        targetPosition = pipe[0].transform.position;
                                        lastSide = 0;
                                    }
                                }
                            }
                        }
                        else if (i == 3 && currentPipe.currentTile != directionTile[i] && targetPosition == targetPositionBefore)
                        {
                            if (lastSide == 0)
                            {

                            }
                        }
                    }
                }
                if (Vector3.Distance(transform.position, targetPosition) > 0.05f || (neighbors != 1 || Collider == startPipe))
                {
                    if (rb.bodyType == RigidbodyType2D.Static) rb.bodyType = RigidbodyType2D.Kinematic;
                    MoveToCenter(targetPosition);
                }
                else rb.bodyType = RigidbodyType2D.Static;
            }
            await UniTask.DelayFrame(5);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Pipe")) Collider = collider;
    }
    private void MoveToCenter(Vector3 targetPosition)
    {
        if (targetPosition != Vector3.zero) transform.position += Vector3.Normalize(targetPosition - transform.position) * speed * Time.deltaTime;
    }
}
