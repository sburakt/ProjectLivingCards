using UnityEngine;

namespace TCG.View
{
    public class GameDisplay : MonoBehaviour
    {
        [SerializeField] SideDisplay[] sideDisplays;
        
        public void Display(GameDisplayData gameDisplayData)
        {
            for (int i = 0; i < 2; i++)
            {
                sideDisplays[i].Display(gameDisplayData.Sides[i]);
            }   
        }   
    }
}
