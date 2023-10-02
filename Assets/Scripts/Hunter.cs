using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    FSM _fsm;

    private void Start()
    {
        _fsm = new FSM();

        _fsm.CreateState("Idle", new Idle(_fsm));
        _fsm.CreateState("Patrol", new Patrol(_fsm));

        _fsm.ChangeState("Idle");
    }

    private void Update()
    {
        _fsm.Execute();
    }
}
