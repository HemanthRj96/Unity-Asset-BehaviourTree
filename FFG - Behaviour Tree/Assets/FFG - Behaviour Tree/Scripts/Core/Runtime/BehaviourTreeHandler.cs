using UnityEngine;


namespace FFG
{
    public class BehaviourTreeHandler : MonoBehaviour
    {
        // Behaviour tree asset reference
        public BehaviourTreeAsset tree = null;

        // Common components and subsystems that is shared between nodes
        private Metadata metadata;

        private void Awake()
        {
            metadata = Metadata.CreateMetaDataFromGameobject(gameObject);
            tree = tree.CreateTreeImage();
            tree.BindMetadataToTree(metadata);
        }

        private void Update()
        {
            tree.Execute();
        }
    }
}
