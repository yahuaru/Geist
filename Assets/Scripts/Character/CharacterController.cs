using UnityEngine;
using System.Collections;
using InControl;

[RequireComponent(typeof(Character))]
public class CharacterController : MonoBehaviour
{
    private Character character;
    private bool prevAction1 = false;
    private bool prevAction3 = false;


    // Use this for initialization
    void Start()
    {
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        var controller = InputManager.ActiveDevice;

        if (controller.Action1 && !prevAction1)
        {
            character.Jump();
            
        }
        prevAction1 = controller.Action1;

        if (controller.Action3 && !prevAction3)
        {
            character.SwapColors();
        }
        prevAction3 = controller.Action3;


        character.HorizontalMovement(controller.LeftStickX);
    }
}
