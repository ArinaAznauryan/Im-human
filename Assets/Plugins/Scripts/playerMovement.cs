using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine.AI;
using static InputManager;
using static Tools.Tools;

public class playerMovement : MonoBehaviour
{

    [System.Serializable]
    public struct animSprite
    {
        [SerializeField] public Sprite[] Img;
    }

    public animSprite[] animSprites;

    public bool fixedMoving = false;
    
    public Vector2 animSpriteSize = new Vector2(2f, 8f);

    public Camera camera;
    public int animIndex = 0;
    public SpriteRenderer playerSprite;
    public Sprite[] sprites;
    public float speed, distance;
    joyStickSystem joystickSystem;
    public int goToStandPosMode;
    public LayerMask boundsMask;
    public bool collided = false, goToStandFinished = false, doOnce = true, animatable = false;
    public Tilemap tilemap;
    public GameObject player, animal;
    public Tile grass;
    public Vector2 dir;
    float endX, endY;

    Vector2[] dirs = {new Vector2(1f, 1f), new Vector2(-1f, 1f), new Vector2(-1f, -1f), 
    new Vector2(1f, -1f), new Vector2(1f, 0f), new Vector2(-1f, 0f)};

    public string mode = "joystick"; //joystick, manual, goToAnimalStandPos

    public int animationSpeed = 0;
    public int animDirection = 0;
    int frames = 0;

    Vector3 playerPos, endPos, startPos;
    
    void Start()
    {
        joystickSystem = FindObjectOfType<joyStickSystem>();
    }

    void goToStandPos() {
        if (animal && doOnce){
            doOnce = false;

            if (goToStandPosMode == 0) dir = dirs[Random.Range(0, dirs.Length)];
            
        }
        if (goToStandPosMode == 0) {
            if (animal && Vector2.Distance(player.transform.position, animal.transform.position) < 10f) {
             
                goToStandFinished = false;
            }
            else {
                goToStandFinished = true;
                doOnce = true;
                animal = null;
                mode = "joystick"; 
            }
        }

        else {
            if (Vector2.Distance(player.transform.position, animal.transform.position) > 1f)
                player.transform.position = Vector2.MoveTowards(player.transform.position, animal.transform.position, speed);
           
            else {
                goToStandFinished = true;
                doOnce = true;
                animal = null;
                mode = "joystick";
            }
        }
        
    }

    void FixedUpdate()
    {
       player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -1f);
       
        switch(mode) {
        
            case "joystick":
                dir = joystickSystem.direction;
                
                bool right = Physics2D.Raycast(player.transform.position, Vector2.right, distance, boundsMask);
                bool left = Physics2D.Raycast(player.transform.position, Vector2.left, distance, boundsMask);
                bool up = Physics2D.Raycast(player.transform.position, Vector2.up, distance, boundsMask);
                bool down = Physics2D.Raycast(player.transform.position, Vector2.down, distance, boundsMask);
                bool right1 = Physics2D.Raycast(player.transform.position, new Vector2(1, 1), distance, boundsMask);
                bool right_1 = Physics2D.Raycast(player.transform.position, new Vector2(1, -1), distance, boundsMask);
                bool left1 = Physics2D.Raycast(player.transform.position, new Vector2(-1, 1), distance, boundsMask);
                bool left_1 = Physics2D.Raycast(player.transform.position, new Vector2(-1, -1), distance, boundsMask);
                
                if (!right && joystickSystem.direction == Vector2.right) dir = Vector2.right;
                else if (right && joystickSystem.direction == Vector2.right) dir = Vector2.zero;

                if (!left && joystickSystem.direction == Vector2.left) dir = Vector2.left;
                else if (left && joystickSystem.direction == Vector2.left) dir = Vector2.zero;

                if (!up && joystickSystem.direction == Vector2.up) dir = Vector2.up;
                else if (up && joystickSystem.direction == Vector2.up) dir = Vector2.zero;

                if (!down && joystickSystem.direction == Vector2.down) dir = Vector2.down;
                else if (down && joystickSystem.direction == Vector2.down) dir = Vector2.zero;

                if (!right1 && joystickSystem.direction == new Vector2(1, 1)) dir = new Vector2(1, 1);
                else if (right1 && joystickSystem.direction == new Vector2(1, 1))dir = Vector2.zero;

                if (!right_1 && joystickSystem.direction == new Vector2(1, -1)) dir = new Vector2(1, -1);
                else if (right_1 && joystickSystem.direction == new Vector2(1, -1)) dir = Vector2.zero;

                if (!left1 && joystickSystem.direction == new Vector2(-1, 1)) dir = new Vector2(-1, 1);
                else if (left1 && joystickSystem.direction == new Vector2(-1, 1)) dir = Vector2.zero;

                if (!left_1 && joystickSystem.direction == new Vector2(-1, -1)) dir = new Vector2(-1, -1);
                else if (left_1 && joystickSystem.direction == new Vector2(-1, -1)) dir = Vector2.zero;

                break;

            case "goToAnimalStandPos": 
                goToStandPos();
                break;
        }
            changeSpriteDirect(dir, playerSprite, sprites, animSprites);
       

    }
        
        
    void changeSpriteDirect(Vector2 dir, SpriteRenderer sprite, Sprite[] Sprites, animSprite[] frameSprites){
        if (animatable) {
            
            frames += animDirection;
            if (animDirection >= 0 && frames < animationSpeed) {
                animDirection = 1;
                animIndex = 1;
            }
            else {
                animIndex = 0;
                animDirection = frames != 0 ? -1 : 0;
            }
        }

        if(dir == new Vector2(0, 0)) {
            sprite.sprite = animatable ? frameSprites[0].Img[0] : Sprites[0];
        }

        if (!fixedMoving) {
            if (dir.x == 0) {
                player.transform.position += new Vector3(0, dir.y*7f/speed*Time.deltaTime, 0);
            }

            else player.transform.position += new Vector3(dir.x*7f/speed*Time.deltaTime, dir.y*4f/speed*Time.deltaTime, 0);
        }

        if(dir == new Vector2(1, 1)) {
            sprite.sprite = animatable ? frameSprites[1].Img[animIndex] : Sprites[1];
        }
        if(dir == new Vector2(1, -1)) {
            sprite.sprite = animatable ? frameSprites[2].Img[animIndex] : Sprites[2];
            //player.transform.position += new Vector3(7f/speed*Time.fixedDeltaTime, -4f/speed*Time.fixedDeltaTime, 0);
        }
        if(dir == new Vector2(-1, 1)) {
            sprite.sprite = animatable ? frameSprites[3].Img[animIndex] : Sprites[3];
            //player.transform.position += new Vector3(-7f/speed*Time.fixedDeltaTime, 4f/speed*Time.fixedDeltaTime, 0);
        }
        if(dir == new Vector2(-1, -1)) {
            sprite.sprite = animatable ? frameSprites[4].Img[animIndex] : Sprites[4];
            //player.transform.position += new Vector3(-7f/speed*Time.fixedDeltaTime, -4f/speed*Time.fixedDeltaTime, 0);
        }
        if(dir == new Vector2(0, 1)) {
            sprite.sprite = animatable ? frameSprites[5].Img[animIndex] : Sprites[5];
            //player.transform.position += new Vector3(0, 7f/speed*Time.fixedDeltaTime, 0);
        }
        if(dir == new Vector2(0, -1)) {
            sprite.sprite = animatable ? frameSprites[6].Img[animIndex] : Sprites[6];
            //player.transform.position += new Vector3(0, -7f/speed*Time.fixedDeltaTime, 0);
        }
        if(dir == new Vector2(-1, 0)) {
            sprite.sprite = animatable ? frameSprites[7].Img[animIndex] : Sprites[7];
            //player.transform.position += new Vector3(-7f/speed*Time.fixedDeltaTime, 0, 0);
        }
        if(dir == new Vector2(1, 0)) {
            sprite.sprite = animatable ? frameSprites[8].Img[animIndex] : Sprites[8];
            //player.transform.position += new Vector3(7f/speed*Time.fixedDeltaTime, 0, 0);
        }
    }
}