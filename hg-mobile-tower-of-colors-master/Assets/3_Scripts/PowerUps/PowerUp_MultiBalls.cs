using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerUp_MultiBalls : PowerUp
{
    public override void Launch()
    {
        base.Launch();
        GameManager.Instance.GetBallShooter().ShootMultiTargets(RemoteConfig.POWER_UP_MULTIBALLS_AMOUNT);
        GameManager.Instance.GetBallShooter().OnMultiBallShot += Finish;
        StartSlowMode();
    }

    private void StartSlowMode()
    {
        GameManager.Instance.SlowMoGame();
        // slow mo is finished direclty in gamemanager but it could be here to the miror imlementation
    }

}
