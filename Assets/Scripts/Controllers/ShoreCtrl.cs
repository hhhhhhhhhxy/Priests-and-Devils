using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoreCtrl
{
    private Shore shoreModel;

    public void CreateShore(Vector3 position)
    {
        if (shoreModel == null) {
            shoreModel = new Shore(position);
        }
    }

    public Shore GetShore()
    {
        return shoreModel;
    }

    public Vector3 AddRole(Role roleModel)
    {
        roleModel.role.transform.parent = shoreModel.shore.transform;
        roleModel.inBoat = false;
        shoreModel.roles.Add(roleModel);
        if (roleModel.isPriest) shoreModel.priestCount++;
        else shoreModel.devilCount++;
        return Position.role_shore[roleModel.id];
    }

    public void RemoveRole(Role roleModel)
    {
        shoreModel.roles.Remove(roleModel);
        if (roleModel.isPriest) shoreModel.priestCount--;
        else shoreModel.devilCount--;
    }

    public void Reset()
    {
        shoreModel.roles.Clear();
        shoreModel.priestCount = 0;
        shoreModel.devilCount = 0;
    }
}