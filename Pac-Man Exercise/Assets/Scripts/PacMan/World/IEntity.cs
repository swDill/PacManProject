namespace PacMan.Entities
{
    /*
     * Base interface for interactable objects in the scene. This game just requires a "OnConsumed" action for when it is eaten.
     */
    public interface IEntity
    {
        void OnConsumed(IEntity consumer);
    }
}