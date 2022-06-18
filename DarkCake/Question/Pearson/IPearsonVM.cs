namespace AussieCake.Question
{
    public interface IPearsonVM
    {
        PearsonType PearsonType { get; }
        string Text { get; }

        string GetUpdatedRealChance();
    }
}
