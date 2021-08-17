using PacMan.Systems;

namespace PacMan.UI
{
    /*
     * Button operation to invoke matchmaking
     */
    public class StartMatchmakingButtonOperation : ButtonOperation
    {
        protected override void OnClicked()
        {
            NetworkController.StartMatchmaking();
        }
    }
}