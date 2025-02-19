using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
/*        public float speed = 5;*/
        float distanceTravelled;

        public readonly float waitTime = 3f;

        public Transform AssignedDebrees;
        public Transform Dumper;

        private float waitTimer = 0;

        private bool trashCollected = false;

        private Transform COllectedBebrees;

        public bool gameOver = false;

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }

            COllectedBebrees = gameObject.transform.Find("Sphere");
            COllectedBebrees.gameObject.SetActive(false);
        }

        void Update()
        {
            if(Vector3.Distance(AssignedDebrees.position, transform.position) < 8 && !trashCollected)
            {
                waitTimer += Time.deltaTime;
                if(waitTimer >= waitTime)
                {
                    trashCollected = true;
                    GameStore.Instance.UpdateMoney(GameStore.Instance.moneyIncrementOnDebree);
                    COllectedBebrees.gameObject.SetActive(trashCollected);
                    Vector3 newScale = AssignedDebrees.localScale - GameStore.Instance.debreeDecrement;
                    AssignedDebrees.localScale = Vector3.Max(newScale, Vector3.zero);
                    GameStore.Instance.UpdateDebreeScaling();
                    // Compare with a small threshold (epsilon) instead of checking for exact zero
                    if (AssignedDebrees.localScale.sqrMagnitude < 0.001f)
                    {
                        AssignedDebrees.localScale = Vector3.zero; // Ensure it's completely zero if it's close
                        gameOver = true;
                    }
                }
                else
                {
                    return;
                }
            }
            if (pathCreator != null)
            {
                distanceTravelled += GameStore.Instance.mainGameData.speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }


            if (Vector3.Distance(Dumper.position, transform.position) < 5)
            {
                trashCollected = false;
                COllectedBebrees.gameObject.SetActive(trashCollected);
                waitTimer = 0;

                if (gameOver)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}