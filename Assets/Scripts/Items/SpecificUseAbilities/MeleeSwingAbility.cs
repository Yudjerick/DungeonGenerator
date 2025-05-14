using UnityEngine;

public class MeleeSwingAbility : OnUseAbility
{
    [SerializeField] PlayerAnimationController animController;

    private void Start()
    {
        animController = transform.root.GetComponentInChildren<PlayerAnimationController>();
    }
    public override void OnUseButtonDownServer()
    {
        print("OnUseButtonDownServer");
    }

    public override void OnUseButtonUpServer()
    {
        print("OnUseButtonUpServer");
    }

    public override void OnUseButtonDownCLient()
    {
        animController.SwordUp();
    }

    public override void OnUseButtonUpClient()
    {
        animController.SwordDown();
    }
}
