using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Tools {
    public class Tools : MonoBehaviour
    {

        public static void RemoveAt<T>(ref T[] arr, int index)
        {
            for (int a = index; a < arr.Length - 1; a++)
            {
                arr[a] = arr[a + 1];
            }
            System.Array.Resize(ref arr, arr.Length - 1);
        }


        public static Vector2 direction(Vector2 position1, Vector2 position2) {
            var dir = (position1 - position2).normalized;
            if (dir.x > 0 & dir.y > 0) dir = new Vector2(1f, 1f);
            else if (dir.x < 0 & dir.y < 0) dir = new Vector2(-1f, -1f);
            else if (dir.x < 0 & dir.y > 0) dir = new Vector2(-1f, 1f);
            else if (dir.x > 0 & dir.y < 0) dir = new Vector2(1f, -1f);

            else if (dir.x == 0 && dir.y != 0) {
                if (dir.y < 0) new Vector2(0f, -1f);
                else new Vector2(0f, 1f);
            }

            else if (dir.y == 0 && dir.x != 0) {
                if (dir.x < 0) dir = new Vector2(-1f, 0f);
                else dir = new Vector2(1f, 0f);
            }

            return dir;
        }

        public static float normalizeDirect(Vector2 pos, Vector2 pos1) {
            if (((pos - pos1).normalized).x > 0) {
                return 1;
            }
            else {
                return -1;
            }
        }

        public static int GetClosestObjectIndex(GameObject[] array, GameObject target) {
            List<float> distances = new List<float>();
            int closestObject = 0;
            for (int i = 0; i<array.Length; i++) {
                distances.Add(Vector2.Distance(target.transform.position, array[i].transform.position));
                float minVal = distances.Min();
                closestObject = distances.IndexOf(minVal);

                if (i==(array.Length-1)) break;
                
            }

            return closestObject;
        }

        public static int GetClosestObjectIndex<T>(ref T[] array, GameObject target) where T : MonoBehaviour {
            List<float> distances = new List<float>();
            int closestObject = 0;
            for (int i = 0; i<array.Length; i++) {
                distances.Add(Vector2.Distance(target.transform.position, array[i].gameObject.transform.position));
                
                float minVal = distances.Min();
                closestObject = distances.IndexOf(minVal);

                if (i==(array.Length-1)) break;
                
            }

            return closestObject;
        }

        public static IEnumerator animFinished(float delay, System.Action<bool> callback) {
            yield return new WaitForSeconds(delay);
            callback(true);
           
        }

        public static List<GameObject> GetGameObjectInChildren(GameObject parent, GameObject excludedTarget = null) {
            List<GameObject> list = new List<Transform>(parent.GetComponentsInChildren<Transform>()).ConvertAll<GameObject>(delegate(Transform p_it) { return p_it.gameObject; });
            list.RemoveAt(0);
            if (excludedTarget) list.Remove(excludedTarget);
            return list;
        }

        public static float randomChoice(float bound1, float bound2) {
            var result = Random.Range(-1f, 1f) > 0 ? bound2 : bound1;
            return result;
        }

        public static int randomChoice(int bound1, int bound2) {
            var result = Random.Range(-1f, 1f) > 0 ? bound2 : bound1;
            return result;
        }


        public static void allignMessage(GameObject scene, GameObject animal, GameObject player, GameObject animalMessage) {
            //________________MESSAGE POSITIONING_______________//
            var messagePointer = animal.transform.transform.Find(animal.name+"Message");

            animalMessage.transform.position = Camera.main.WorldToScreenPoint(messagePointer.transform.position);
            animalMessage.transform.transform.localScale = new Vector3(Mathf.Abs(animalMessage.transform.transform.localScale.x), animalMessage.transform.transform.localScale.y, animalMessage.transform.transform.localScale.z);
            animalMessage.transform.transform.localScale = Vector3.Scale(animalMessage.transform.localScale, new Vector3(normalizeDirect(player.transform.position, Camera.main.ScreenToWorldPoint(animalMessage.transform.position)), 1f, 1f));
            
            if (FindObject(scene, "yesNo")) {
                FindObject(scene, "yesNo").transform.localScale = animalMessage.transform.transform.localScale;
            }
            
            FindObject(scene, "animalUI").transform.position = Camera.main.WorldToScreenPoint(messagePointer.transform.position);

            Vector2 messageDir = animalMessage.transform.localScale;
            Vector2 textDir = FindObject(scene, "animalMessageDescription").transform.localScale;
            FindObject(scene, "animalMessageDescription").transform.localScale = messageDir.x > 0 ? new Vector2(1f, 1f) : new Vector2(-1f, 1f);
            //________________MESSAGE POSITIONING_______________//
                
        }

        public static GameObject FindObject(GameObject parent, string name) {
            Transform[] trs= parent.GetComponentsInChildren<Transform>(true);
            foreach(Transform t in trs){
                if(t.name == name) return t.gameObject;
            }
            return null;
        }
    }
}
