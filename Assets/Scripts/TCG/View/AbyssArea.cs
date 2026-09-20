using UnityEngine;

namespace TCG.View
{
    public class AbyssAreaView : MonoBehaviour
    {
        [SerializeField] public AbyssCellView[] abyssCellsMid;
        [SerializeField] public AbyssCellView[] abyssCellsSide0;
        [SerializeField] public AbyssCellView[] abyssCellsSide1;
        public AbyssCellView[][] AbyssCellsSide 
        { 
            get 
            { 
                return new AbyssCellView[][] { abyssCellsSide0, abyssCellsSide1 }; 
            } 
        } 
    }
}
