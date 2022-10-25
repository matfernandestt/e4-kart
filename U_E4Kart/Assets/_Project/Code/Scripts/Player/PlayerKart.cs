using Photon;

public class PlayerKart : PunBehaviour
{
    public PhotonPlayer photonPlayer;

    public bool isMine => photonView.isMine;

    private void Awake()
    {
        photonPlayer = photonView.owner;
    }
}
