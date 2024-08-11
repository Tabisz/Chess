using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileOccupier
{
    void OnMyTileSelected();
    void OnMyTileSecondarySelected();

    void OnMyTileKillCommand();
}
