using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float PIPE_WIDTH = 1f;
    private const float SCENE_HEIGHT = 20f;

    private void CreateGapPipes(float gapSize, float gapPositionY, float positionX) {
        CreatePipe(SCENE_HEIGHT/2 + gapPositionY - gapSize/2, positionX, true);
        CreatePipe(SCENE_HEIGHT/2 - gapPositionY - gapSize/2, positionX, false);
    }

    private void CreatePipe(float height, float xposition, bool onBottom) {
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
    }

    void Start()
    {
        CreateGapPipes(3f, 5f, 6f);
    }

    void Update()
    {
        
    }
}
