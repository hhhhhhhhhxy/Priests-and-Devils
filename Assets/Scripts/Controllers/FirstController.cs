using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction {
    public CCActionManager actionManager;
    public ShoreCtrl leftShoreController, rightShoreController;
    public River river;
    public BoatCtrl boatController;
    public RoleCtrl[] roleControllers;
    public bool isRunning;
    public float time;

    public void JudgeCallback(bool isRunning, string message){
        this.gameObject.GetComponent<UserGUI>().gameMessage = message;
        this.gameObject.GetComponent<UserGUI>().time = (int)time;
        this.isRunning = isRunning;
        this.gameObject.GetComponent<UserGUI>().SetGameOver(!isRunning);
    }

    public void LoadResources() {
        //role
        roleControllers = new RoleCtrl[6];
        for (int i = 0; i < 6; ++i) {
            roleControllers[i] = new RoleCtrl();
            roleControllers[i].CreateRole(Position.role_shore[i], i < 3 ? true : false, i);
        }

        //shore
        leftShoreController = new ShoreCtrl();
        leftShoreController.CreateShore(Position.left_shore);
        leftShoreController.GetShore().shore.name = "left_shore";
        rightShoreController = new ShoreCtrl();
        rightShoreController.CreateShore(Position.right_shore);
        rightShoreController.GetShore().shore.name = "right_shore";

        //将人物添加并定位至左岸  
        foreach (RoleCtrl roleController in roleControllers)
        {
            roleController.GetRoleModel().role.transform.localPosition = leftShoreController.AddRole(roleController.GetRoleModel());
        }
        //boat
        boatController = new BoatCtrl();
        boatController.CreateBoat(Position.left_boat);

        //river
        river = new River(Position.river);

        isRunning = true;
        time = 60;
    }

    public void MoveBoat() {
        if (isRunning == false || actionManager.IsMoving()) return;
        
        Vector3 destination = boatController.GetBoatModel().isRight ? Position.left_boat : Position.right_boat;
        actionManager.MoveBoat(boatController.GetBoatModel().boat, destination, 5);
        
        boatController.GetBoatModel().isRight = !boatController.GetBoatModel().isRight;
    }

    public void MoveRole(Role roleModel) {
        if (isRunning == false || actionManager.IsMoving()) return;
        Vector3 destination, mid_destination;
        if (roleModel.inBoat)
        {
            if (boatController.GetBoatModel().isRight)
                destination = rightShoreController.AddRole(roleModel);
            else
                destination = leftShoreController.AddRole(roleModel);
            if (roleModel.role.transform.localPosition.y > destination.y)
                mid_destination = new Vector3(destination.x, roleModel.role.transform.localPosition.y, destination.z);
            else
                mid_destination = new Vector3(roleModel.role.transform.localPosition.x, destination.y, destination.z);
            actionManager.MoveRole(roleModel.role, mid_destination, destination, 5);
            roleModel.onRight = boatController.GetBoatModel().isRight;
            boatController.RemoveRole(roleModel);
        }
        else
        {
            if (boatController.GetBoatModel().isRight == roleModel.onRight)
            {
                if (roleModel.onRight)
                {
                    rightShoreController.RemoveRole(roleModel);
                }
                else
                {
                    leftShoreController.RemoveRole(roleModel);
                }
                destination = boatController.AddRole(roleModel);
                if (roleModel.role.transform.localPosition.y > destination.y)
                    mid_destination = new Vector3(destination.x, roleModel.role.transform.localPosition.y, destination.z);
                else
                    mid_destination = new Vector3(roleModel.role.transform.localPosition.x, destination.y, destination.z);
                actionManager.MoveRole(roleModel.role, mid_destination, destination, 5);
            }
        }
    }

    public void Check() {
        // This method is now empty as the check logic is moved to JudgeController
    }

    void Awake() {
        SSDirector.GetInstance().CurrentSceneController = this;
        LoadResources();
        this.gameObject.AddComponent<UserGUI>();
        this.gameObject.AddComponent<CCActionManager>();
        this.gameObject.AddComponent<JudgeController>();
    }

    void Update() {
        if (isRunning)
        {
            time -= Time.deltaTime;
            this.gameObject.GetComponent<UserGUI>().time = (int)time;
        }
    }

    public void Restart() {
        // Reset game state
        isRunning = true;
        time = 60;

        // Reset shore and boat controllers
        leftShoreController.Reset();
        rightShoreController.Reset();
        boatController.Reset();

        // Reset roles to initial positions
        for (int i = 0; i < 6; ++i) {
            Role roleModel = roleControllers[i].GetRoleModel();
            roleModel.role.transform.localPosition = Position.role_shore[i];
            roleModel.inBoat = false;
            roleModel.onRight = false;
            leftShoreController.AddRole(roleModel);
        }

        // Reset boat position
        boatController.GetBoatModel().boat.transform.position = Position.left_boat;
        boatController.GetBoatModel().isRight = false;

        // Reset left shore roles
        Shore leftShore = leftShoreController.GetShore();
        leftShore.priestCount = 3;
        leftShore.devilCount = 3;

        // Reset right shore roles
        Shore rightShore = rightShoreController.GetShore();
        rightShore.priestCount = 0;
        rightShore.devilCount = 0;

        // Reset boat roles
        boatController.GetBoatModel().priestCount = 0;
        boatController.GetBoatModel().devilCount = 0;
    }
}