using UnityEngine;

namespace TCG.View
{
    public class GameDisplay : MonoBehaviour
    {
        [SerializeField] SideDisplay[] sideDisplays;
        
        public void Display(BoardSnapshot boardSnapshot)
        {
            for (int i = 0; i < 2; i++)
            {
                sideDisplays[i].Display(boardSnapshot.Sides[i], boardSnapshot);
            }   
        }   
    }
}
