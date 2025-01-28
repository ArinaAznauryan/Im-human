using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Tools.Tools;

public class CartManager : MonoBehaviour
{
    public GameObject player, railSpawnPoint;
    public Tilemap tilemap;
    public Tile[] tiles;
    public Sprite[] tileSprites;
    public TileBase ruleTile;
    public Tile autoTile;
    public Vector3Int coord, railSpawnPointPos;
    public playerMovement playerMovement;
    public float speed;
    bool runIsFalling = false, fall = false;
    public Animator cartAnimator;

    int crossRoadType = 0; //0 - rightDirectCross; 1 - leftDirectCross

    TileBase emptyTileBase;

    public Vector2 centrePos;

    void Start() {
        railSpawnPoint.transform.position = tilemap.CellToWorld(railSpawnPointPos);
    }

    void Update() {
        var dir = playerMovement.dir;

        Vector3Int curTile = tilemap.WorldToCell(player.transform.position);
        curTile = new Vector3Int(curTile.x, curTile.y, 0);

        Vector3Int nextTile = new Vector3Int(curTile.x, curTile.y, 0);


        if (dir.x == dir.y) crossRoadType = 1;
        else crossRoadType = 0;

        if (isRail(tileSprites, curTile)) {
            
            if (dir.x == dir.y) { //Right cross direction - increasing function - (1, 1), (-1, -1)
                //crossRoadType = 0;
                nextTile = new Vector3Int(curTile.x+(int)dir.x, curTile.y, 0);
            }

            else {  //Left cross direction - decreasing function - (-1, 1), (1, -1)
                //crossRoadType = 1;
                nextTile = new Vector3Int(curTile.x, curTile.y+(int)dir.y, 0);
            }

            var tarTile = tilemap.GetTile(nextTile);

            if (isRail(tileSprites, nextTile)) {
                runIsFalling = false;
                
                
                
                var worldNextTile = tilemap.CellToWorld(nextTile);

                Vector2 nearTile = tilemap.CellToWorld(new Vector3Int(nextTile.x+1, nextTile.y+1, 0));

                centrePos = new Vector2((worldNextTile.x+nearTile.x)/2, (worldNextTile.y+nearTile.y)/2);
    
                player.transform.position = Vector2.MoveTowards(player.transform.position, centrePos, speed);
            }

            else { 
                runIsFalling = true;
            }
            
        }

        else {
            if (dir.x == dir.y) { //Right cross direction - increasing function - (1, 1), (-1, -1)
                crossRoadType = 0;
            }

            else {  //Left cross direction - decreasing function - (-1, 1), (1, -1)
                crossRoadType = 1;
            }
        }

        if (dir == new Vector2(0f, 0f)) {
            runIsFalling =  false;
            crossRoadType = 2;
        }

        if (fall) FindObjectOfType<dieAndRevive>().play = true;
    }

    public void fallOffCart() {
        if (crossRoadType == 0) cartAnimator.Play("runRight");
        else if (crossRoadType == 1) cartAnimator.Play("runLeft");
        
        if (cartAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "cartFallEnd") {
            StartCoroutine(animFinished(cartAnimator.GetCurrentAnimatorStateInfo(0).length, animFinish => {
                if(animFinish) {
                    fall = false;
                    player.transform.position = centreTilePos(railSpawnPoint.transform.position);
                }
            }));
        }
    }

    Vector2 centreTilePos(Vector2 worldTilePos) {
        Vector3Int tilePos = tilemap.WorldToCell(worldTilePos);
        Vector2 nearTile = tilemap.CellToWorld(new Vector3Int(tilePos.x+1, tilePos.y+1, 0));
        Vector2 centrePos = new Vector2((worldTilePos.x+nearTile.x)/2, (worldTilePos.y+nearTile.y)/2);
        return centrePos;
    }
    

        bool isRail(Sprite[] tiles, Vector3Int curTilePos/*, TileBase ruleTile = null*/) {
            Sprite curTileSprite = tilemap.GetSprite(curTilePos);

            foreach (Sprite tileSprite in tileSprites) {
                if (curTileSprite == tileSprite) {
                    return true;
                }
            }
            return false;
        }

    IEnumerator isFalling(Vector3Int tarTilePos) {
        if (runIsFalling) { 
            yield return new WaitForSeconds(20f/**Time.deltaTime*/);
            if(!isRail(tileSprites, tarTilePos)) {
                fall = true;
                runIsFalling = false;
            }
        }
    }

}
