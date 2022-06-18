namespace AussieCake.Verb
{
    public class VerbModel
	{
		public string Infinitive { get; set; }
		public string Past { get; set; }
		public string PastParticiple { get; set; }
		public string Gerund { get; set; } // Present Participle
		public string Person { get; set; }

		public VerbModel()
		{
		}

		public VerbModel(string infinitive, string past, string pastParticiple, string gerund, string person)
		{
			Infinitive = infinitive;
			Past = past;
			PastParticiple = pastParticiple;
			Gerund = gerund;
			Person = person;
		}
	}
}
