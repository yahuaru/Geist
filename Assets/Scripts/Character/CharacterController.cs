using UnityEngine;
using System.Collections;
using InControl;

[RequireComponent(typeof(Character))]
public class CharacterController : MonoBehaviour
{
    private Character character;


    // Use this for initialization
    void Start()
    {
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        var controller = InputManager.ActiveDevice;

        if (controller.Action1.WasPressed)
        {
            character.Jump();
            
        }

        if (controller.Action3.WasPressed)
        {
            character.SwapColors(perc: controller.LeftStickX);
        }

        character.HorizontalMovement(controller.LeftStickX);
    }
}
