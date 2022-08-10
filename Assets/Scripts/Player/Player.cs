using UnityEngine;

namespace PxlDev
{
    public class Player : MonoBehaviour
    {
        public PlayerMovement Movement;
        public PlayerVisuals Visuals;
        public PlayerInteractions Interactions;

        public static Player Instance;

        void Awake()
        {
            if(Instance == null)
                Instance = this;

            Movement = GetComponent<PlayerMovement>();
            Visuals = GetComponent<PlayerVisuals>();
            Interactions = GetComponent<PlayerInteractions>();
        }
    }
}
