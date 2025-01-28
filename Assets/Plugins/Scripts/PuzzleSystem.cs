using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Tools.Tools;

public class Puzzle {
  public Vector2 startPos;
  public GameObject part;
}

public class PuzzleSystem : MonoBehaviour
{ 

    public Vector2Int tileSize;
    public Animator levelTransition;
    public GameObject mouse, targetTrigger, Camera, nextButton, spriteRoot, puzzleSource;
    Dictionary<Vector3, GameObject> startParts = new Dictionary<Vector3, GameObject>();
    public GameObject example, curPart, nextPart;
   
    bool selectNew=true, trigger=true, selected=false, firstLoop = true;
    public GameObject[,] grid;
    public List<GameObject> parts;
    public int startX, startY, endX, endY, rightCase;
    public int rows, columns;
    int i = 0, state=0;
    public float scaleX, scaleY;
    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
    public Material material;

    public void InitializePuzzle() {
        int num = 0;
        parts = new List<GameObject>();
       
        int range = rows*columns; 
        Vector3 tempGO;
        grid = new GameObject[rows, columns];

        Sprite sprite =  Resources.Load<Sprite>(PlayerPrefs.GetString("puzzleSprite"));
        Debug.Log("ARMY: " + Resources.Load(PlayerPrefs.GetString("puzzleSprite")) + " fuck: " + sprite);
        rows = (int)PlayerPrefs.GetInt("puzzleSize")/10;
        columns = PlayerPrefs.GetInt("puzzleSize")%10;

        var source = new Texture2D( (int)sprite.rect.width, (int)sprite.rect.height );
        var pixels = sprite.texture.GetPixels(  (int)sprite.textureRect.x, 
                                                (int)sprite.textureRect.y, 
                                                (int)sprite.textureRect.width, 
                                                (int)sprite.textureRect.height );
        source.SetPixels(pixels);
        source.Apply();

        for(int i = 0; i < rows; i++) {
            for(int j = 0; j < columns; j++) {
                Sprite newSprite = Sprite.Create(source, new Rect(i*1920/rows, j*1080/columns, 1920/rows, 1080/columns), new Vector2(0f, 0f));
                GameObject n = new GameObject();
                n.AddComponent<itemGrabTrigger>();
                n.GetComponent<itemGrabTrigger>().startToTrigger = true;
                n.GetComponent<itemGrabTrigger>().name = targetTrigger.name;
                SpriteRenderer sr = n.AddComponent<SpriteRenderer>();
                sr.material = material;sr.sprite = newSprite;
                Vector2 S = n.GetComponent<SpriteRenderer>().sprite.bounds.size;
                n.AddComponent<BoxCollider2D>().size = S;
                n.GetComponent<BoxCollider2D>().offset = new Vector2 ((S.x/2), S.y/2);
                n.GetComponent<BoxCollider2D>().isTrigger = true;

                n.transform.position = new Vector3(i*(1920/(float)rows)/100f, j*(1080/(float)columns)/100f, 0);
                n.transform.parent = spriteRoot.transform;

                startParts.Add(n.transform.position, n);
                parts.Add(n); 
               
            }
        }
        
        foreach (GameObject part in parts) {
            int number = Random.Range(0, rows*columns);
            tempGO = parts[number].transform.position;
            parts[number].transform.position = part.transform.position; 
            part.transform.position = tempGO;
        }
    }
    
    void Start()
    {
        levelTransition.Play("levelAppearTransition");

        InitializePuzzle();
    }
    
    void Update()
    {
        if (state==0) {
            foreach (GameObject part in parts) {
                if (mouse.GetComponent<InputManager>().fastTapped && part.GetComponent<itemGrabTrigger>().triggered) {
                    curPart = part;
                    state = 1;
                    break;
                }
            }
        }
  
        if (state==1) {
            if (curPart && !nextPart) {
                foreach (GameObject part in parts) {
                    bool pieceTriggered = mouse.GetComponent<InputManager>().fastTapped && part.GetComponent<itemGrabTrigger>().triggered ? true : false;
                    
                    if(pieceTriggered && part != curPart) {
           
                        nextPart = part;
                        var copy=curPart.transform.position;

                        curPart.transform.position=nextPart.transform.position;
                        nextPart.transform.position=copy;
                        curPart = null;
                        state = 0;
                        nextPart = null;
                        break;
                    }
                }
            }
        }
        
        if (puzzleDone()) {   
            foreach (GameObject part in parts){
                part.GetComponent<itemGrabTrigger>().startToTrigger = false;
                Return();
            }
        }
        
        }

    

    IEnumerator UpdateArray(int index)
    {
        while(true)
        {
            yield return new WaitForEndOfFrame();
        
            UpdateArray(index);
        
            index = (index < parts.Count) ? index + 1 : 0;
        }
    }

    void Return() {
        levelTransition.GetComponent<Animator>().Play("cameraSuckIn");

        StartCoroutine(animFinished(levelTransition.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length, animFinish => {
            if(animFinish) {
                FindObjectOfType<ReloadAdditiveScene>().reload = true;
                SceneManager.UnloadScene("puzzleScene");
            }
        }));
    }

    public bool puzzleDone() {
        foreach (GameObject part in parts) {
            foreach(KeyValuePair<Vector3,GameObject> startPart in startParts){
                if(part == startPart.Value && part.transform.position != startPart.Key){
                    return false;
                }
            }
        }
        return true;
        
    }
}

