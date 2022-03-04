using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
  private const float PIPE_WIDTH = 1f;
  private const float SCENE_HEIGHT = 20f;
  private const float PIPE_DESTROY_POSITION_X = -20f;
  private const float PIPE_SPAWN_POSITION_X = 20f;
  private const float PIPE_MOVE_SPEED_MAX = 20f;
  private const float PIPE_MOVE_SPEED_MIN = 1f;
  private const float PIPE_SPAWN_INTERVAL_DISTANCE = 10f;
  private const float GAP_SIZE_MAX = 20f;
  private const float GAP_SIZE_MIN = 10f;
  private const float TIME_TO_HIGHEST_LEVEL = 100f;

  private List<Transform> pipePairList;
  private float pipeMoveSpeed;
  private float gapSize;
  private int pipeSpawnedCount;
  private float gameRanTime;
  private float pipeMovedDistanceFromLastFrame;

  private void PipeMovement()
  {
    for (int i = 0; i < pipePairList.Count; i++)
    {
      pipePairList[i].position += new Vector3(-1, 0, 0) * pipeMoveSpeed * Time.deltaTime;

      if (pipePairList[i].position[0] < PIPE_DESTROY_POSITION_X)
      {
        Destroy(pipePairList[i].gameObject);
        pipePairList.Remove(pipePairList[i]);

        i--;
      }
    }
  }

  private void CreateGapPipes(float gapSize, float gapPositionY, float positionX)
  {
    Transform pipePairTransform = new GameObject("pipePair").transform;

    (Transform bottomPipeBody, Transform bottomPipeHead) = CreatePipe(SCENE_HEIGHT / 2 + gapPositionY - gapSize / 2, true);
    (Transform topPipeBody, Transform topPipeHead) = CreatePipe(SCENE_HEIGHT / 2 - gapPositionY - gapSize / 2, false);

    bottomPipeBody.parent = pipePairTransform;
    bottomPipeHead.parent = pipePairTransform;
    topPipeBody.parent = pipePairTransform;
    topPipeHead.parent = pipePairTransform;

    pipePairTransform.position = new Vector3(positionX, 0, 0);

    pipePairList.Add(pipePairTransform);

    // Add pipe spawned count
    pipeSpawnedCount++;
  }

  private (Transform, Transform) CreatePipe(float height, bool onBottom)
  {
    // Make pipe body
    Transform pipeBody = Instantiate(GameAssets.GetInstance().pf_pipe_body);

    float pipeBodyPositionY;

    if (onBottom)
    {
      pipeBodyPositionY = -SCENE_HEIGHT / 2;
    }
    else
    {
      pipeBody.transform.localScale = new Vector3(1, -1, 1);
      pipeBodyPositionY = SCENE_HEIGHT / 2;
    }

    pipeBody.position = new Vector2(0, pipeBodyPositionY);
    pipeBody.GetComponent<SpriteRenderer>().size = new Vector2(PIPE_WIDTH, height);
    pipeBody.GetComponent<BoxCollider2D>().offset = new Vector2(0, height / 2);
    pipeBody.GetComponent<BoxCollider2D>().size = new Vector2(PIPE_WIDTH, height);

    // Make pipe head
    Transform pipeHead = Instantiate(GameAssets.GetInstance().pf_pipe_head);

    float pipeHeadPositionY;

    if (onBottom)
    {
      pipeHeadPositionY = pipeBody.position[1] + height;
    }
    else
    {
      pipeHeadPositionY = pipeBody.position[1] - height;
    }

    pipeHead.position = new Vector2(0, pipeHeadPositionY);

    return (pipeBody, pipeHead);
  }

  private void Awake()
  {
    pipePairList = new List<Transform>();
    pipeMoveSpeed = PIPE_MOVE_SPEED_MIN;
    gapSize = GAP_SIZE_MAX;
    pipeSpawnedCount = 0;
    gameRanTime = 0;
  }

  private void Start()
  {
  }

  private void Update()
  {
    PipeMovement();

    gameRanTime += float.IsInfinity(gameRanTime + Time.deltaTime) ? 0 : Time.deltaTime;
    pipeMovedDistanceFromLastFrame += pipeMoveSpeed * Time.deltaTime;

    // Adjustment for level
    if (gameRanTime >= TIME_TO_HIGHEST_LEVEL)
    {
      pipeMoveSpeed = PIPE_MOVE_SPEED_MAX;
      gapSize = GAP_SIZE_MIN;
    }
    else
    {
      pipeMoveSpeed = PIPE_MOVE_SPEED_MIN + (PIPE_MOVE_SPEED_MAX-PIPE_MOVE_SPEED_MIN) * (gameRanTime/TIME_TO_HIGHEST_LEVEL);
      gapSize = GAP_SIZE_MAX - (GAP_SIZE_MAX-GAP_SIZE_MIN) * (gameRanTime/TIME_TO_HIGHEST_LEVEL);
    }

    // Spawn pipe
    if (pipeMovedDistanceFromLastFrame >= PIPE_SPAWN_INTERVAL_DISTANCE)
    {
      CreateGapPipes(gapSize, Random.Range(-10 + gapSize / 2, 10 - gapSize / 2), PIPE_SPAWN_POSITION_X);

      pipeMovedDistanceFromLastFrame -= PIPE_SPAWN_INTERVAL_DISTANCE;
    }
  }
}
