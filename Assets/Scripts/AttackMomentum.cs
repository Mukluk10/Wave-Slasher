using UnityEngine;

public class AttackMomentum : MonoBehaviour
{
    [SerializeField] PlayerAttackScript attakScript;

    private void Awake()
    {
        attakScript = GetComponentInParent<PlayerAttackScript>();
    }

    public void ApplyMomentum()
    {
        attakScript.ApplyMomentum();
    }
}
