using UnityEngine;

namespace PxlDev
{
    [CreateAssetMenu(fileName = "Dialog", menuName = "Create New Dialog", order = 0)]
    public class Dialog : ScriptableObject
    {
        [TextArea(3, 10)]
        public string[] Lines; 
        public string Name;
        public AudioClip Voice;
    }
}