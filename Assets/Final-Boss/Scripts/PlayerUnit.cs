using UnityEngine;

namespace Final_Boss
{
    public class PlayerUnit : MonoBehaviour
    {
        [SerializeField] private int health = 30;
        [SerializeField] private int mana = 3;
        [SerializeField] private int manaPerTurn = 1;
    }
}