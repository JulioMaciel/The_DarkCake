using System;

namespace AussieCake.Attempt
{
	public class AttemptVM
	{
		public int Id { get; private set; }

		public int IdQuestion { get; set; }

		public int Score { get; set; }
		public DateTime When { get; set; }

        public Model Type { get; private set; }

        public AttemptVM(int idQuestion, int score, DateTime when, Model type)
        {
            IdQuestion = idQuestion;
            Score = score;
            When = when;
            Type = type;
        }

        public AttemptVM(int id, int idQuestion, int score, DateTime when, Model type) 
            : this (idQuestion, score, when, type)
        {
            Id = id;
        }

    }
}
