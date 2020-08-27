using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : Element
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) app.logic.InputFilter(KeyCode.W);

        if (Input.GetKeyDown(KeyCode.S)) app.logic.InputFilter(KeyCode.S);

        if (Input.GetKeyDown(KeyCode.A)) app.logic.InputFilter(KeyCode.A);

        if (Input.GetKeyDown(KeyCode.D)) app.logic.InputFilter(KeyCode.D);

        if (Input.GetKeyDown(KeyCode.Space)) app.logic.InputFilter(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.Alpha1)) app.logic.InputFilter(KeyCode.Alpha1);

        if (Input.GetKeyDown(KeyCode.Alpha2)) app.logic.InputFilter(KeyCode.Alpha2);

        if (Input.GetKeyDown(KeyCode.Alpha3)) app.logic.InputFilter(KeyCode.Alpha3);
    }
}
