using UnityEngine;
using UnityEngine.AI;


namespace Lacobus.BehaviourTree
{
    public class Metadata
    {
        // Fields

        public GameObject gameObject;
        public Transform transform;
        public Rigidbody2D rigidbody2D;
        public Rigidbody rigidbody;
        public Animator animator;
        public NavMeshAgent agent;
        public BoxCollider2D boxCollider2D;
        public BoxCollider boxCollider;
        public CapsuleCollider2D capsuleCollider2D;
        public CapsuleCollider capsuleCollider;
        public CharacterController characterController;


        // Public methods

        /// <summary>
        /// Create a new meta data from gameobject
        /// </summary>
        /// <param name="gameobject">Target gameobject from which meta data is being created</param>
        public static Metadata CreateMetaDataFromGameobject(GameObject gameobject)
        {
            Metadata metaData = new Metadata();
            metaData.gameObject = gameobject;
            metaData.transform = gameobject.transform;

            // AI
            metaData.agent = gameobject.GetComponent<NavMeshAgent>();

            // Physics
            metaData.rigidbody2D = gameobject.GetComponent<Rigidbody2D>();
            metaData.rigidbody = gameobject.GetComponent<Rigidbody>();

            // Colliders
            metaData.boxCollider2D = gameobject.GetComponent<BoxCollider2D>();
            metaData.boxCollider = gameobject.GetComponent<BoxCollider>();
            metaData.capsuleCollider2D = gameobject.GetComponent<CapsuleCollider2D>();
            metaData.capsuleCollider = gameobject.GetComponent<CapsuleCollider>();
            metaData.characterController = gameobject.GetComponent<CharacterController>();

            return metaData;
        }
    }
}
