using UnityEngine;
using System.Collections;

namespace Strategy {
    public class DroneBop : 
        MonoBehaviour, IManeuverable { 
        
        public void Maneuver(Drone drone) {
            StartCoroutine(Bop(drone));
        }

        IEnumerator Bop(Drone drone)
        {
            var isReverse = false;
            var speed = drone.speed;
            var startPosition = drone.transform.position;
            var endPosition = startPosition;
            endPosition.y = drone.maxHeight;

            while (true) {
                float time = 0;
                var start = drone.transform.position;
                var end = 
                    (isReverse) ? startPosition : endPosition;

                while (time < speed) {
                    drone.transform.position = 
                        Vector3.Lerp(start, end, time / speed);
                    time += Time.deltaTime;
                    yield return null;
                }

                yield return new WaitForSeconds(1);
                isReverse = !isReverse;
            }
        }
    }
}