using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float PIPE_WIDTH = 1f;
    private const float SCENE_HEIGHT = 20f;
    private const float PIPE_MOVE_SPEED = 3f;
    private const float PIPE_DESTROY_POSITION_X = -20f;

    private List<Transform> pipePairList;

    private void PipeMovement() {
        for (int i = 0; i < pipePairList.Count; i++) {
            pipePairList[i].position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;

            if (pipePairList[i].position[0] < PIPE_DESTROY_POSITION_X) {
                Destroy(pipePairList[i].gameObject);
                Debug.Log("wow");
                pipePairList.Remove(pipePairList[i]);
                i--;
            }
        }
    }

    private void CreateGapPipes(float gapSize, float gapPositionY, float positionX) {
        GameObject pipePair = new GameObject("pipePair");

        (Transform bottomPipeBody, Transform bottomPipeHead) = CreatePipe(SCENE_HEIGHT/2 + gapPositionY - gapSize/2, positionX, true);
        (Transform topPipeBody, Transform topPipeHead) = CreatePipe(SCENE_HEIGHT/2 - gapPositionY - gapSize/2, positionX, false);

        bottomPipeBody.parent = pipePair.transform;
        bottomPipeHead.parent = pipePair.transform;
        topPipeBody.parent = pipePair.transform;
        topPipeHead.parent = pipePair.transform;

        pipePairList.Add(pipePair.transform);
    }

    private (Transform, Transform) CreatePipe(float height, float xposition, bool onBottom) {
        // Make pipe body
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pf_pipe_body);

        float pipeBodyPositionY;

        if (onBottom) {
            pipeBodyPositionY = -SCENE_HEIGHT/2;
        } else {
            pipeBody.transform.localScale = new Vector3(1, -1, 1);
            pipeBodyPositionY = SCENE_HEIGHT/2;
        }

        pipeBody.position = new Vector2(xposition, pipeBodyPositionY);
        pipeBody.GetComponent<SpriteRenderer>().size = new Vector2(PIPE_WIDTH, height);
        pipeBody.GetComponent<BoxCollider2D>().offset = new Vector2(0, height/2);
        pipeBody.GetComponent<BoxCollider2D>().size = new Vector2(PIPE_WIDTH, height);

        // Make pipe head
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pf_pipe_head);

        float pipeHeadPositionY;

        if (onBottom) {
            pipeHeadPositionY = pipeBody.position[1] + height;
        } else {
            pipeHeadPositionY = pipeBody.position[1] - height;
        }

        pipeHead.position = new Vector2(xposition, pipeHeadPositionY);

        return (pipeBody, pipeHead);
    }

    private void Awake() {
        pipePairList = new List<Transform>();
    }

    private void Start()
    {
        CreateGapPipes(3f, 5f, 6f);
    }

    private void Update()
    {
        PipeMovement();
    }
}
