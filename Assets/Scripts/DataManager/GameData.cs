using System;

[Serializable]

public class Data
{
    // member variable //
    private static int ItemSize = 30;

    // data //
    public bool[] CurrentHaveItem = new bool[ItemSize];

    // interface //
    public int GetItemVariableNum() { return ItemSize; }
}
