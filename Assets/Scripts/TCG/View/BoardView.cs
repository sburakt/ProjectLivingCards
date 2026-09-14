using UnityEngine;
using UnityEngine.Serialization;

namespace TCG.View
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] public SideView[] sideViews;
        [SerializeField] public AbyssAreaView abyssAreaView;
    }
}
