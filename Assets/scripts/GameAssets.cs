using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
  private static GameAssets instance;

  public static GameAssets GetInstance() {
    return instance;
  }
  
  private void Awake() {
    instance = this;
  }

  public Sprite pipeHeadSprite;
  public Transform pf_pipe_head;
  public Transform pf_pipe_body;
}
