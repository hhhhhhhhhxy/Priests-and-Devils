using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCtrl : ClickAction
{
    private Boat boatModel;
    private IUserAction userAction;

    public BoatCtrl() {
        userAction = SSDirector.GetInstance().CurrentSceneController as IUserAction;
    }

    public void CreateBoat(Vector3 position) {
        if (boatModel != null) {
            Object.DestroyImmediate(boatModel.boat);
        }
        boatModel = new Boat(position);
        boatModel.boat.GetComponent<Click>().setClickAction(this);
    }

    public Boat GetBoatModel() {
        return boatModel;
    }

    public Vector3 AddRole(Role roleModel) {
        int index = -1;
        if (boatModel.roles.Count < 2) {
            boatModel.roles.Add(roleModel);
            roleModel.inBoat = true;
            roleModel.role.transform.parent = boatModel.boat.transform;
            if (roleModel.isPriest) boatModel.priestCount++;
            else boatModel.devilCount++;
            return Position.role_boat[boatModel.roles.Count - 1];
        }
        return roleModel.role.transform.localPosition;
    }

    public void RemoveRole(Role roleModel) {
        boatModel.roles.Remove(roleModel);
        if (roleModel.isPriest) boatModel.priestCount--;
        else boatModel.devilCount--;
    }

    public void DealClick() {
        if (boatModel.roles.Count > 0) {
            userAction.MoveBoat();
        }
    }

    public void Reset()
    {
        boatModel.roles.Clear();
        boatModel.priestCount = 0;
        boatModel.devilCount = 0;
        boatModel.isRight = false;
        boatModel.boat.transform.position = Position.left_boat;
    }
}