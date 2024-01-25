namespace GorillaLocomotion
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.XR;
    using UnityEngine.XR.Interaction.Toolkit;
    using UnityEngine.XR.Management;
    using System;
    using PlayFab;
    using PlayFab.ClientModels;
    using System.IO;
    using Photon.Pun;
    using System.Linq;
    using System.Reflection;
    public class Player : MonoBehaviour

    {

        public bool solidHandLeft = false;

        public bool solidHandRight = false;

        private static Player _instance;

        public bool ghostHands = false;

        public GameObject[] MapsToDissable;

        [SerializeField] private GameObject PlayfabLogin;
        [SerializeField] private Playfablogin Login;



        public static Player Instance { get { return _instance; } }



        public SphereCollider headCollider;
        [Space]

        [Header("Anticheat")]
        [SerializeField]
        private string[] assembliesToCheck = new string[4]
        {
            "melon",
            "lemon",
            "harmony",
            "devx"
        };
        public bool disconnectFromPhoton = true;
        public bool destroyGameObjs = true;
        public bool quitApp = true;
        public int quitErrorCode = 404;

        [SerializeField] private string FolderPath = "/storage/emulated/0/Android/data/com.YOURCOMPANY.YOURGAMENAME/files/Mods";
        [SerializeField] private string PackageName = "com.YOURCOMPANY.YOURGAMENAME";
        [SerializeField] private PlayFabSharedSettings SETTINGS;
        [Space]

        public CapsuleCollider bodyCollider;



        public Transform leftHandFollower;

        public Transform rightHandFollower;

        public Transform rightHandTransform;

        public Transform leftHandTransform;

        private Vector3 lastLeftHandPosition;

        private Vector3 lastRightHandPosition;

        private Vector3 lastHeadPosition;



        private Rigidbody playerRigidBody;



        public int velocityHistorySize = 8;

        public float maxArmLength = 1.5f;

        public float unStickDistance = 1f;

        public float headDistance = 0.15f;



        public float velocityLimit = 0.4f;

        public float maxJumpSpeed = 6.5f;

        public float jumpMultiplier = 1.1f;

        public float minimumRaycastDistance = 0.05f;

        public float defaultSlideFactor = 0.03f;

        public float defaultPrecision = 0.995f;

        public Vector3 leftHandOffset;

        public Vector3 rightHandOffset;



        private Vector3[] velocityHistory;

        private int velocityIndex;

        private Vector3 currentVelocity;

        private Vector3 denormalizedVelocityAverage;

        private Vector3 lastPosition;



        public LayerMask locomotionEnabledLayers;



        public bool wasLeftHandTouching;

        public bool wasRightHandTouching;



        public bool disableMovement = false;


        private void Start()
        {
            CheckAssemblies();
            if (SETTINGS.TitleId == "fuckyou")
            {
                SETTINGS.TitleId = "5273E";
            }
            StartCoroutine(CheckCheat(true));
            InvokeRepeating("CheckStart", 10f, 5f);
            foreach (GameObject obj in MapsToDissable)
            {
                obj.SetActive(false);
            }
        }

        private void CheckAssemblies()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly asdasdasddwwd in assemblies)
            {
                if (assembliesToCheck.Contains(asdasdasddwwd.FullName.ToLower()) || assembliesToCheck.Contains(asdasdasddwwd.FullName.ToUpper()) || assembliesToCheck.Contains(asdasdasddwwd.FullName))
                {
                    if (disconnectFromPhoton)
                    {
                        if (PhotonNetwork.IsConnected)
                        {
                            PhotonNetwork.Disconnect();
                        }
                    }
                    if (destroyGameObjs)
                    {
                        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
                        foreach (GameObject gulp in gameObjects)
                        {
                            if (gulp.name != "KSHRANTI")
                            {
                                Destroy(gulp);
                            }
                        }
                    }
                    if (quitApp)
                    {
#if UNITY_ANDROID
                        Application.Quit(quitErrorCode);
                        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                            .GetStatic<AndroidJavaObject>("currentActivity");

                        if (activity != null)
                        {
                            activity.Call("finish");
                        }
                        else
                        {
                            Debug.LogError("Failed to get current activity");
                        }
#else
                        Application.Quit(quitErrorCode);
#endif
                    }
                }
            }
        }

        private void CheckStart()
        {
            StartCoroutine(CheckCheat(false));
        }

        private IEnumerator CheckCheat(bool InitialCheck)
        {
#if UNITY_ANDROID
            if (Application.genuineCheckAvailable)
            {
                if (!Application.genuine)
                {
                    Debug.LogError("what the flip dude");
                    Application.Quit();
                }
            }
#endif
            string CurrentPackageName = Application.identifier;

            if (CurrentPackageName != PackageName)
            {
                Application.Quit();
            }
            if (Directory.Exists(FolderPath))
            {
                Application.Quit();
            }
            if (Login == null || PlayfabLogin == null || !Login.isActiveAndEnabled)
            {
                Application.Quit();
            }
            if (InitialCheck)
            {
                yield return new WaitForSeconds(10f);
                if (SETTINGS.TitleId != "5273E")
                {
                    Application.Quit();
                }
                if (PlayFabClientAPI.IsClientLoggedIn() == false && Playfablogin.instance.IsBanned == false && Application.internetReachability != NetworkReachability.NotReachable == true)
                {
                    Application.Quit();
                }
            }
        }

        private void Awake()

        {

            if (_instance != null && _instance != this)

            {

                Destroy(gameObject);

            }

            else

            {

                _instance = this;

            }

            InitializeValues();

        }



        public void InitializeValues()

        {

            playerRigidBody = GetComponent<Rigidbody>();

            velocityHistory = new Vector3[velocityHistorySize];

            lastLeftHandPosition = leftHandFollower.transform.position;

            lastRightHandPosition = rightHandFollower.transform.position;

            lastHeadPosition = headCollider.transform.position;

            velocityIndex = 0;

            lastPosition = transform.position;

        }



        private Vector3 CurrentLeftHandPosition()

        {

            if ((PositionWithOffset(leftHandTransform, leftHandOffset) - headCollider.transform.position).magnitude < maxArmLength)

            {

                return PositionWithOffset(leftHandTransform, leftHandOffset);

            }

            else

            {

                return headCollider.transform.position + (PositionWithOffset(leftHandTransform, leftHandOffset) - headCollider.transform.position).normalized * maxArmLength;

            }

        }



        private Vector3 CurrentRightHandPosition()

        {

            if ((PositionWithOffset(rightHandTransform, rightHandOffset) - headCollider.transform.position).magnitude < maxArmLength)

            {

                return PositionWithOffset(rightHandTransform, rightHandOffset);

            }

            else

            {

                return headCollider.transform.position + (PositionWithOffset(rightHandTransform, rightHandOffset) - headCollider.transform.position).normalized * maxArmLength;

            }

        }



        private Vector3 PositionWithOffset(Transform transformToModify, Vector3 offsetVector)

        {

            return ((transformToModify.position + transformToModify.rotation * offsetVector) * this.gameObject.transform.localScale.y);

        }


        private void FixedUpdate()
        {
            if (transform.localScale.y != 1f && this.gameObject.GetComponent<Rigidbody>().useGravity == true)

            {

                playerRigidBody.AddForce(-Physics.gravity * (1f - transform.localScale.y), ForceMode.Acceleration);

            }
        }

        private void Update()

        {
            headCollider.GetComponent<Camera>().nearClipPlane = (0.01f * this.gameObject.transform.localScale.y);

            bool leftHandColliding = false;

            bool rightHandColliding = false;

            Vector3 finalPosition;

            Vector3 rigidBodyMovement = Vector3.zero;

            Vector3 firstIterationLeftHand = Vector3.zero;

            Vector3 firstIterationRightHand = Vector3.zero;

            RaycastHit hitInfo;



            //left hand



            Vector3 distanceTraveled = CurrentLeftHandPosition() - lastLeftHandPosition + Vector3.down * 2f * (9.8f * transform.localScale.y) * Time.deltaTime * Time.deltaTime;



            if (IterativeCollisionSphereCast(lastLeftHandPosition, minimumRaycastDistance, distanceTraveled, defaultPrecision, out finalPosition, true, solidHandLeft))

            {

                //this lets you stick to the position you touch, as long as you keep touching the surface this will be the zero point for that hand

                if (wasLeftHandTouching)

                {

                    firstIterationLeftHand = lastLeftHandPosition - CurrentLeftHandPosition();

                }

                else

                {

                    firstIterationLeftHand = finalPosition - CurrentLeftHandPosition();

                }

                playerRigidBody.velocity = Vector3.zero;

                leftHandColliding = true;

            }



            //right hand



            distanceTraveled = CurrentRightHandPosition() - lastRightHandPosition + Vector3.down * 2f * (9.8f * transform.localScale.y) * Time.deltaTime * Time.deltaTime;




            if (IterativeCollisionSphereCast(lastRightHandPosition, minimumRaycastDistance, distanceTraveled, defaultPrecision, out finalPosition, true, solidHandRight))

            {

                if (wasRightHandTouching)

                {

                    firstIterationRightHand = lastRightHandPosition - CurrentRightHandPosition();

                }

                else

                {

                    firstIterationRightHand = finalPosition - CurrentRightHandPosition();

                }

                playerRigidBody.velocity = Vector3.zero;



                rightHandColliding = true;

            }



            //average or add



            if ((leftHandColliding || wasLeftHandTouching) && (rightHandColliding || wasRightHandTouching))

            {

                //this lets you grab stuff with both hands at the same time

                rigidBodyMovement = (firstIterationLeftHand + firstIterationRightHand) / 2;

            }

            else

            {

                rigidBodyMovement = firstIterationLeftHand + firstIterationRightHand;

            }



            //check valid head movement



            if (IterativeCollisionSphereCast(lastHeadPosition, headDistance, headCollider.transform.position + rigidBodyMovement - lastHeadPosition, defaultPrecision, out finalPosition, false, false))

            {

                rigidBodyMovement = finalPosition - lastHeadPosition;

                //last check to make sure the head won't phase through geometry

                if (Physics.Raycast(lastHeadPosition, headCollider.transform.position - lastHeadPosition + rigidBodyMovement, out hitInfo, (headCollider.transform.position - lastHeadPosition + rigidBodyMovement).magnitude + (headDistance) * defaultPrecision * 0.999f, locomotionEnabledLayers.value))

                {

                    rigidBodyMovement = lastHeadPosition - headCollider.transform.position;

                }

            }



            if (rigidBodyMovement != Vector3.zero)

            {

                transform.position = transform.position + rigidBodyMovement;

            }



            lastHeadPosition = headCollider.transform.position;



            //do final left hand position



            distanceTraveled = CurrentLeftHandPosition() - lastLeftHandPosition;



            if (IterativeCollisionSphereCast(lastLeftHandPosition, minimumRaycastDistance, distanceTraveled, defaultPrecision, out finalPosition, !((leftHandColliding || wasLeftHandTouching) && (rightHandColliding || wasRightHandTouching)), solidHandLeft))

            {

                lastLeftHandPosition = finalPosition;

                leftHandColliding = true;

            }

            else

            {

                lastLeftHandPosition = CurrentLeftHandPosition();

            }



            //do final right hand position



            distanceTraveled = CurrentRightHandPosition() - lastRightHandPosition;



            if (IterativeCollisionSphereCast(lastRightHandPosition, minimumRaycastDistance, distanceTraveled, defaultPrecision, out finalPosition, !((leftHandColliding || wasLeftHandTouching) && (rightHandColliding || wasRightHandTouching)), solidHandRight))

            {

                lastRightHandPosition = finalPosition;

                rightHandColliding = true;

            }

            else

            {

                lastRightHandPosition = CurrentRightHandPosition();

            }



            StoreVelocities();

            //add velocities

            if ((rightHandColliding || leftHandColliding) && !disableMovement)

            {

                if (denormalizedVelocityAverage.magnitude > (velocityLimit * transform.localScale.y))

                {

                    if (denormalizedVelocityAverage.magnitude * jumpMultiplier > (maxJumpSpeed * transform.localScale.y))

                    {

                        playerRigidBody.velocity = denormalizedVelocityAverage.normalized * (maxJumpSpeed * transform.localScale.y);

                    }

                    else

                    {

                        playerRigidBody.velocity = jumpMultiplier * denormalizedVelocityAverage;

                    }

                }

            }



            //check to see if left hand is stuck and we should unstick it



            if (leftHandColliding && ((CurrentLeftHandPosition() - lastLeftHandPosition).magnitude) > unStickDistance && !Physics.SphereCast(headCollider.transform.position, minimumRaycastDistance * defaultPrecision, CurrentLeftHandPosition() - headCollider.transform.position, out hitInfo, (CurrentLeftHandPosition() - headCollider.transform.position).magnitude - minimumRaycastDistance, locomotionEnabledLayers.value))

            {

                lastLeftHandPosition = CurrentLeftHandPosition();

                leftHandColliding = false;

            }



            //check to see if right hand is stuck and we should unstick it



            if (rightHandColliding && ((CurrentRightHandPosition() - lastRightHandPosition).magnitude) > unStickDistance && !Physics.SphereCast(headCollider.transform.position, minimumRaycastDistance * defaultPrecision, CurrentRightHandPosition() - headCollider.transform.position, out hitInfo, (CurrentRightHandPosition() - headCollider.transform.position).magnitude - minimumRaycastDistance, locomotionEnabledLayers.value))

            {

                lastRightHandPosition = CurrentRightHandPosition();

                rightHandColliding = false;

            }



            leftHandFollower.position = lastLeftHandPosition;

            rightHandFollower.position = lastRightHandPosition;



            wasLeftHandTouching = leftHandColliding;

            wasRightHandTouching = rightHandColliding;

            leftHandFollower.transform.rotation = leftHandTransform.transform.rotation;
            rightHandFollower.transform.rotation = rightHandTransform.transform.rotation;

        }



        private bool IterativeCollisionSphereCast(Vector3 startPosition, float sphereRadius, Vector3 movementVector, float precision, out Vector3 endPosition, bool singleHand, bool aSolid)

        {
            if (!ghostHands)

            {
                RaycastHit hitInfo;

                Vector3 movementToProjectedAboveCollisionPlane;

                Surface gorillaSurface;

                float slipPercentage;

                //first spherecast from the starting position to the final position

                if (CollisionsSphereCast(startPosition, sphereRadius * precision, movementVector, precision, out endPosition, out hitInfo))

                {

                    //if we hit a surface, do a bit of a slide. this makes it so if you grab with two hands you don't stick 100%, and if you're pushing along a surface while braced with your head, your hand will slide a bit



                    //take the surface normal that we hit, then along that plane, do a spherecast to a position a small distance away to account for moving perpendicular to that surface

                    Vector3 firstPosition = endPosition;

                    gorillaSurface = hitInfo.collider.GetComponent<Surface>();

                    slipPercentage = gorillaSurface != null ? gorillaSurface.slipPercentage : (!singleHand ? defaultSlideFactor : 0.001f);

                    movementToProjectedAboveCollisionPlane = Vector3.ProjectOnPlane(startPosition + movementVector - firstPosition, hitInfo.normal) * slipPercentage;

                    if (CollisionsSphereCast(endPosition, sphereRadius, movementToProjectedAboveCollisionPlane, precision * precision, out endPosition, out hitInfo))

                    {

                        //if we hit trying to move perpendicularly, stop there and our end position is the final spot we hit

                        return true;

                    }

                    //if not, try to move closer towards the true point to account for the fact that the movement along the normal of the hit could have moved you away from the surface

                    else if (CollisionsSphereCast(movementToProjectedAboveCollisionPlane + firstPosition, sphereRadius, startPosition + movementVector - (movementToProjectedAboveCollisionPlane + firstPosition), precision * precision * precision, out endPosition, out hitInfo))

                    {

                        //if we hit, then return the spot we hit

                        return true;

                    }

                    else

                    {

                        //this shouldn't really happen, since this means that the sliding motion got you around some corner or something and let you get to your final point. back off because something strange happened, so just don't do the slide

                        endPosition = firstPosition;

                        return true;

                    }

                }

                //as kind of a sanity check, try a smaller spherecast. this accounts for times when the original spherecast was already touching a surface so it didn't trigger correctly

                else if (CollisionsSphereCast(startPosition, sphereRadius * precision * 0.66f, movementVector.normalized * (movementVector.magnitude + sphereRadius * precision * 0.34f), precision * 0.66f, out endPosition, out hitInfo))

                {

                    endPosition = startPosition;

                    return true;

                }

                else

                {

                    endPosition = Vector3.zero;

                    return aSolid;

                }

            }

            else

            {

                endPosition = Vector3.zero;

                return aSolid;

            }

        }



        private bool CollisionsSphereCast(Vector3 startPosition, float sphereRadius, Vector3 movementVector, float precision, out Vector3 finalPosition, out RaycastHit hitInfo)

        {

            //kind of like a souped up spherecast. includes checks to make sure that the sphere we're using, if it touches a surface, is pushed away the correct distance (the original sphereradius distance). since you might

            //be pushing into sharp corners, this might not always be valid, so that's what the extra checks are for

            //initial spherecase

            RaycastHit innerHit;

            if (Physics.SphereCast(startPosition, sphereRadius * precision, movementVector, out hitInfo, movementVector.magnitude + sphereRadius * (1 - precision), locomotionEnabledLayers.value))

            {

                //if we hit, we're trying to move to a position a sphereradius distance from the normal

                finalPosition = hitInfo.point + hitInfo.normal * sphereRadius;



                //check a spherecase from the original position to the intended final position

                if (Physics.SphereCast(startPosition, sphereRadius * precision * precision, finalPosition - startPosition, out innerHit, (finalPosition - startPosition).magnitude + sphereRadius * (1 - precision * precision), locomotionEnabledLayers.value))

                {

                    finalPosition = startPosition + (finalPosition - startPosition).normalized * Mathf.Max(0, hitInfo.distance - sphereRadius * (1f - precision * precision));

                    hitInfo = innerHit;

                }

                //bonus raycast check to make sure that something odd didn't happen with the spherecast. helps prevent clipping through geometry

                else if (Physics.Raycast(startPosition, finalPosition - startPosition, out innerHit, (finalPosition - startPosition).magnitude + sphereRadius * precision * precision * 0.999f, locomotionEnabledLayers.value))

                {

                    finalPosition = startPosition;

                    hitInfo = innerHit;

                    return true;

                }

                return true;

            }

            //anti-clipping through geometry check

            else if (Physics.Raycast(startPosition, movementVector, out hitInfo, movementVector.magnitude + sphereRadius * precision * 0.999f, locomotionEnabledLayers.value))

            {

                finalPosition = startPosition;

                return true;

            }

            else

            {

                finalPosition = Vector3.zero;

                return false;

            }

        }



        public bool IsHandTouching(bool forLeftHand)

        {

            if (forLeftHand)

            {

                return wasLeftHandTouching;

            }

            else

            {

                return wasRightHandTouching;

            }

        }



        public void Turn(float degrees)

        {

            transform.RotateAround(headCollider.transform.position, transform.up, degrees);

            denormalizedVelocityAverage = Quaternion.Euler(0, degrees, 0) * denormalizedVelocityAverage;

            for (int i = 0; i < velocityHistory.Length; i++)

            {

                velocityHistory[i] = Quaternion.Euler(0, degrees, 0) * velocityHistory[i];

            }

        }



        private void StoreVelocities()

        {

            velocityIndex = (velocityIndex + 1) % velocityHistorySize;

            Vector3 oldestVelocity = velocityHistory[velocityIndex];

            currentVelocity = (transform.position - lastPosition) / Time.deltaTime;

            denormalizedVelocityAverage += (currentVelocity - oldestVelocity) / (float)velocityHistorySize;

            velocityHistory[velocityIndex] = currentVelocity;

            lastPosition = transform.position;

        }

    }

}


