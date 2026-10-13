
namespace FirstProject.Meta.Multiplayer
{
    public interface IMultiplayerService
    {
        bool InRoom {  get; }
        bool IsMasterClient { get; }
        bool IsConnectedAndReady { get; }

        void SetReadyState(bool isReady);
        void LeaveRoom();
    }
}