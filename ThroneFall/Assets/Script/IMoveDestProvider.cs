using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveDestProvider
{
    void SetMoveDest(Vector3 dest);
    void SimpleMove();
    void NotifyStopMove();
    void NotifyPauseMove();
    void NotifyResumeMove();

}
