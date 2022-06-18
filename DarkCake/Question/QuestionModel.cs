namespace AussieCake.Question
{
    public abstract class QuestionModel
    {
        public int Id { get; protected set; }

        public string Text { get; protected set; }

        public int IsActive { get; protected set; }

        protected bool IsReal { get; set; } = false;

        protected QuestionModel(int id, string text, int isActive)
            : this(text, isActive)
        {
            Id = id;

            IsReal = true;
        }

        protected QuestionModel(string text, int isActive)
        {
            IsActive = isActive;
            Text = text.Trim(); // can't set ToLower here       
        }

    }
}
