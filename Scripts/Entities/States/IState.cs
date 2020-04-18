public interface IState
{
    void Start(Character target);
    void Process(Character target, float delta);
    void End(Character target);

    string GetDebugInfo();
}
