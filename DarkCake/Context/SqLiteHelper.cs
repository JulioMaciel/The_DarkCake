using AussieCake.Attempt;
using AussieCake.Question;
using AussieCake.Util;
using AussieCake.Verb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AussieCake.Context
{

    public class SqLiteHelper : SqLiteCommands
    {
        private static readonly string InsertSQL = "insert into {0} values(NULL, ";
        private static readonly string UpdateSQL = "update {0} set {1} where Id = {2}";
        private static readonly string RemoveSQL = "delete from {0} where Id = {1}";

        private static readonly string Null = "NULL";

        protected static List<VocVM> Vocabularies { get; private set; }
        protected static List<AttemptVM> VocabularyAttempts { get; private set; }
        protected static List<VerbModel> Verbs { get; private set; }
        protected static List<AttemptVM> EssayAttempts { get; private set; }
        protected static List<AttemptVM> SumRetellAttempts { get; private set; }
        protected static List<AttemptVM> DescImgAttempts { get; private set; }
        protected static List<PronVM> Pronunciations { get; private set; }
        protected static List<SpellVM> Spellings { get; private set; }
        protected static List<AttemptVM> SpellingAttempts { get; private set; }

        protected static List<P_FIB_VM> PearsonFIBs { get; private set; }
        protected static List<AttemptVM> PearsonFIBAttempts { get; private set; }
        protected static List<P_Dit_VM> PearsonDits { get; private set; }
        protected static List<AttemptVM> PearsonDitAttempts { get; private set; }

        public static void InitializeDB()
        {
            if (!CreateIfEmptyDB())
                return;
        }

        #region GetDB

        protected static void GetVocabulariesDB()
        {
            var dataset = GetTable(Model.Voc);

            Vocabularies = new List<VocVM>();

            var tables = dataset.Tables[0];
            var enumerable = tables.AsEnumerable();
            var selected = enumerable.Select(GetDatarowVocabulary());

            foreach (var model in selected)
                Vocabularies.Add(model.ToVM());
        }

        protected static void GetPronunciationsDB()
        {
            var dataset = GetTable(Model.Pron);

            Pronunciations = new List<PronVM>();

            var tables = dataset.Tables[0];
            var enumerable = tables.AsEnumerable();
            var selected = enumerable.Select(GetDatarowPronunciation());

            foreach (var model in selected)
                Pronunciations.Add(model.ToVM());
        }

        protected static void GetSpellingsDB()
        {
            var dataset = GetTable(Model.Spell);

            Spellings = new List<SpellVM>();

            var tables = dataset.Tables[0];
            var enumerable = tables.AsEnumerable();
            var selected = enumerable.Select(GetDatarowSpelling());

            foreach (var model in selected)
                Spellings.Add(model.ToVM());
        }

        protected static void GetP_FIB_DB()
        {
            var dataset = GetTable(Model.P_FIB);

            PearsonFIBs = new List<P_FIB_VM>();

            var tables = dataset.Tables[0];
            var enumerable = tables.AsEnumerable();
            var selected = enumerable.Select(GetDatarowP_FIB());

            foreach (var model in selected)
                PearsonFIBs.Add(model.ToVM());
        }

        protected static void GetP_Dit_DB()
        {
            var dataset = GetTable(Model.P_Dit);

            PearsonDits = new List<P_Dit_VM>();

            var tables = dataset.Tables[0];
            var enumerable = tables.AsEnumerable();
            var selected = enumerable.Select(GetDatarowP_Dit());

            foreach (var model in selected)
                PearsonDits.Add(model.ToVM());
        }

        protected static void GetVerbsDB()
        {
            var dataset = GetTable(Model.Verb);

            Verbs = dataset.Tables[0].AsEnumerable().Select(GetDatarowVerbs()).ToList();
        }

        protected static void GetVocAttemptsDB()
        {
            var dataset = GetTable(GetDBAttemptName(Model.Voc));
            VocabularyAttempts = dataset.Tables[0].AsEnumerable().Select(GetDatarowAttempts(Model.Voc)).ToList();
        }

        protected static void GetEssayAttemptsDB()
        {
            var dataset = GetTable(GetDBAttemptName(Model.Essay));
            EssayAttempts = dataset.Tables[0].AsEnumerable().Select(GetDatarowAttempts(Model.Essay)).ToList();
        }

        protected static void GetSumRetellAttemptsDB()
        {
            var dataset = GetTable(GetDBAttemptName(Model.SumRetell));
            SumRetellAttempts = dataset.Tables[0].AsEnumerable().Select(GetDatarowAttempts(Model.SumRetell)).ToList();
        }

        protected static void GetDescImgAttemptsDB()
        {
            var dataset = GetTable(GetDBAttemptName(Model.DescImg));
            DescImgAttempts = dataset.Tables[0].AsEnumerable().Select(GetDatarowAttempts(Model.DescImg)).ToList();
        }

        protected static void GetSpellAttemptsDB()
        {
            var dataset = GetTable(GetDBAttemptName(Model.Spell));
            SpellingAttempts = dataset.Tables[0].AsEnumerable().Select(GetDatarowAttempts(Model.Spell)).ToList();
        }

        protected static void GetPearsonFIBAttemptsDB()
        {
            var dataset = GetTable(GetDBAttemptName(Model.P_FIB));
            PearsonFIBAttempts = dataset.Tables[0].AsEnumerable().Select(GetDatarowAttempts(Model.P_FIB)).ToList();
        }

        protected static void GetPearsonDitAttemptsDB()
        {
            var dataset = GetTable(GetDBAttemptName(Model.P_Dit));
            PearsonDitAttempts = dataset.Tables[0].AsEnumerable().Select(GetDatarowAttempts(Model.P_Dit)).ToList();
        }

        #endregion

        #region Get

        protected static List<string> GetSentenceWhichContains(List<string> whichContains)
        {
            var sens = GetFromDB(Model.P_Dit.ToDesc(), "Text", whichContains);

            if (!sens.Any())
                sens = GetFromDB(Model.Sen.ToDesc(), "Text", whichContains);

            return sens;
        }

        #endregion

        #region Inserts

        protected static bool InsertVocabulary(VocModel Voc)
        {
            string query = string.Format(InsertSQL + "'{1}', '{2}', {3}, '{4}', '{5}')",
                                        Model.Voc.ToDesc(),
                                        Voc.Text, Voc.Answer,
                                        Null, Null, Voc.IsActive);
            if (!SendQuery(query))
                return false;

            var inserted = GetLast(Model.Voc.ToDesc());
            Vocabularies.Add(inserted.Tables[0].AsEnumerable().
                                Select(GetDatarowVocabulary()).First().ToVM());

            return true;
        }

        protected static bool InsertPronunciation(PronModel Pron)
        {
            string query = string.Format(InsertSQL + "'{1}', '{2}', {3})",
                                        Model.Pron.ToDesc(),
                                        Pron.Text, Pron.Phonemes,
                                        Pron.IsActive);
            if (!SendQuery(query))
                return false;

            var inserted = GetLast(Model.Pron.ToDesc());
            Pronunciations.Add(inserted.Tables[0].AsEnumerable().
                                Select(GetDatarowPronunciation()).First().ToVM());

            return true;
        }

        protected static bool InsertSpelling(SpellModel Spell)
        {
            string query = string.Format(InsertSQL + "'{1}', '{2}')",
                                        Model.Spell.ToDesc(),
                                        Spell.Text,
                                        Spell.IsActive);
            if (!SendQuery(query))
                return false;

            var inserted = GetLast(Model.Spell.ToDesc());
            Spellings.Add(inserted.Tables[0].AsEnumerable().
                                Select(GetDatarowSpelling()).First().ToVM());

            return true;
        }

        protected static bool InsertAttempt(AttemptVM att)
        {
            string query = string.Format(InsertSQL + "'{1}', '{2}', '{3}')",
                                         GetDBAttemptName(att.Type),
                                         att.IdQuestion, att.Score, att.When);
            if (!SendQuery(query))
                return false;

            var inserted = GetLast(GetDBAttemptName(att.Type));

            if (att.Type == Model.Voc)
            {
                if (VocabularyAttempts == null)
                    VocabularyAttempts = new List<AttemptVM>();

                VocabularyAttempts.Add(inserted.Tables[0].AsEnumerable().
                                        Select(GetDatarowAttempts(att.Type)).First());
            }
            else if (att.Type == Model.Essay)
            {
                if (EssayAttempts == null)
                    EssayAttempts = new List<AttemptVM>();

                EssayAttempts.Add(inserted.Tables[0].AsEnumerable().
                                        Select(GetDatarowAttempts(att.Type)).First());
            }
            else if (att.Type == Model.SumRetell)
            {
                if (SumRetellAttempts == null)
                    SumRetellAttempts = new List<AttemptVM>();

                SumRetellAttempts.Add(inserted.Tables[0].AsEnumerable().
                                        Select(GetDatarowAttempts(att.Type)).First());
            }
            else if (att.Type == Model.DescImg)
            {
                if (DescImgAttempts == null)
                    DescImgAttempts = new List<AttemptVM>();

                DescImgAttempts.Add(inserted.Tables[0].AsEnumerable().
                                        Select(GetDatarowAttempts(att.Type)).First());
            }
            else if(att.Type == Model.Spell)
            {
                if (SpellingAttempts == null)
                    SpellingAttempts = new List<AttemptVM>();

                SpellingAttempts.Add(inserted.Tables[0].AsEnumerable().
                                        Select(GetDatarowAttempts(att.Type)).First());
            }
            else if(att.Type == Model.P_FIB)
            {
                if (PearsonFIBAttempts == null)
                    PearsonFIBAttempts = new List<AttemptVM>();

                PearsonFIBAttempts.Add(inserted.Tables[0].AsEnumerable().
                                        Select(GetDatarowAttempts(att.Type)).First());
            }
            else if(att.Type == Model.P_Dit)
            {
                if (PearsonDitAttempts == null)
                    PearsonDitAttempts = new List<AttemptVM>();

                PearsonDitAttempts.Add(inserted.Tables[0].AsEnumerable().
                                        Select(GetDatarowAttempts(att.Type)).First());
            }

            return true;
        }

        protected static bool InsertVerb(VerbModel verb)
        {
            string query = string.Format(InsertSQL + "'{1}', '{2}', '{3}', '{4}', '{5}')",
                                                     Model.Verb.ToDesc(),
                                                     verb.Infinitive, verb.Past, verb.PastParticiple, verb.Person, verb.Gerund);
            if (!SendQuery(query))
                return false;

            var inserted = GetLast(Model.Verb.ToDesc());
            Verbs.Add(inserted.Tables[0].AsEnumerable().
                                Select(GetDatarowVerbs()).First());

            return true;
        }

        public static bool InsertSentence(string sentence)
        {
            string query = string.Format(InsertSQL + "'{1}')", Model.Sen.ToDesc(), sentence);

            if (!SendQuery(query))
                return false;

            return true;
        }

        #endregion

        #region Updates

        protected static bool UpdateVocabulary(VocModel Voc)
        {
            var oldVoc = Vocabularies.First(c => c.Id == Voc.Id).ToModel();
            int field = 0;
            string ColumnsToUpdate = string.Empty;

            CheckFieldUpdate("Words", Voc.Text, oldVoc.Text, ref field, ref ColumnsToUpdate);
            CheckFieldUpdate("Answer", Voc.Answer, oldVoc.Answer, ref field, ref ColumnsToUpdate);
            CheckFieldUpdate("Definition", Voc.Definition, oldVoc.Definition, ref field, ref ColumnsToUpdate);
            CheckFieldUpdate("PtBr", Voc.PtBr, oldVoc.PtBr, ref field, ref ColumnsToUpdate);

            CheckQuestionChanges(Voc, oldVoc, ref field, ref ColumnsToUpdate);

            if (ColumnsToUpdate.IsEmpty())
                return Errors.ThrowErrorMsg(ErrorType.NullOrEmpty, ColumnsToUpdate);

            string query = string.Format(UpdateSQL, Model.Voc.ToDesc(), ColumnsToUpdate, Voc.Id);

            if (!SendQuery(query))
                return false;

            return true;
        }

        protected static bool UpdatePronunciation(PronModel Pron)
        {
            var oldPron = Pronunciations.First(c => c.Id == Pron.Id).ToModel();
            int field = 0;
            string ColumnsToUpdate = string.Empty;

            CheckFieldUpdate("Words", Pron.Text, oldPron.Text, ref field, ref ColumnsToUpdate);
            CheckFieldUpdate("Phonemes", Pron.Phonemes, oldPron.Phonemes, ref field, ref ColumnsToUpdate);

            CheckQuestionChanges(Pron, oldPron, ref field, ref ColumnsToUpdate);

            if (ColumnsToUpdate.IsEmpty())
                return Errors.ThrowErrorMsg(ErrorType.NullOrEmpty, ColumnsToUpdate);

            string query = string.Format(UpdateSQL, Model.Pron.ToDesc(), ColumnsToUpdate, Pron.Id);

            if (!SendQuery(query))
                return false;

            return true;
        }

        protected static bool UpdateSpelling(SpellModel spell)
        {
            var oldSpell = Spellings.First(c => c.Id == spell.Id).ToModel();
            int field = 0;
            string ColumnsToUpdate = string.Empty;

            CheckFieldUpdate("Words", spell.Text, oldSpell.Text, ref field, ref ColumnsToUpdate);

            CheckQuestionChanges(spell, oldSpell, ref field, ref ColumnsToUpdate);

            if (ColumnsToUpdate.IsEmpty())
                return Errors.ThrowErrorMsg(ErrorType.NullOrEmpty, ColumnsToUpdate);

            string query = string.Format(UpdateSQL, Model.Spell.ToDesc(), ColumnsToUpdate, spell.Id);

            if (!SendQuery(query))
                return false;

            return true;
        }

        private static void CheckFieldUpdate(string fieldName, object newValue, object oldValue, ref int field, ref string ColumnsToUpdate)
        {
            if (newValue != oldValue)
            {
                var ToInt = newValue is bool || newValue is Int16;
                ColumnsToUpdate += field > 0 ? ", " : string.Empty;
                ColumnsToUpdate += fieldName + " = " + "'" + (ToInt ? Convert.ToInt16(newValue) : newValue) + "'";

                field++;
            }
        }

        private static void CheckQuestionChanges(QuestionModel quest, QuestionModel oldQuest, ref int field, ref string ColumnsToUpdate)
        {
            CheckFieldUpdate("IsActive", quest.IsActive, oldQuest.IsActive, ref field, ref ColumnsToUpdate);
        }

        #endregion

        #region Private Members

        private static Func<DataRow, VocModel> GetDatarowVocabulary()
        {
            return dataRow => new VocModel(
                                Convert.ToInt16(dataRow.Field<Int64>("Id")),
                                dataRow.Field<string>("Words"),
                                dataRow.Field<string>("Answer"),
                                dataRow.Field<string>("PtBr"),
                                dataRow.Field<string>("Definition"),
                                Convert.ToInt16(dataRow.Field<Int64>("IsActive")));
        }

        private static Func<DataRow, PronModel> GetDatarowPronunciation()
        {
            return dataRow => new PronModel(
                                Convert.ToInt16(dataRow.Field<Int64>("Id")),
                                dataRow.Field<string>("Words"),
                                dataRow.Field<string>("Phonemes"),
                                Convert.ToInt16(dataRow.Field<Int64>("IsActive")));
        }

        private static Func<DataRow, SpellModel> GetDatarowSpelling()
        {
            return dataRow => new SpellModel(
                                Convert.ToInt16(dataRow.Field<Int64>("Id")),
                                dataRow.Field<string>("Words"),
                                Convert.ToInt16(dataRow.Field<Int64>("IsActive")));
        }

        private static Func<DataRow, AttemptVM> GetDatarowAttempts(Model type)
        {
            return dataRow => new AttemptVM(
                        Convert.ToInt16(dataRow.Field<Int64>("Id")),
                        Convert.ToInt16(dataRow.Field<Int64>("Id" + type.ToDesc())),
                        Convert.ToInt16(dataRow.Field<Int64>("Score")),
                        Convert.ToDateTime(dataRow.Field<string>("When")),
                        type
                    );
        }

        private static Func<DataRow, P_FIB_Model> GetDatarowP_FIB()
        {
            return dataRow => new P_FIB_Model(
                                Convert.ToInt16(dataRow.Field<Int64>("Id")),
                                dataRow.Field<string>("Text"),
                                Convert.ToInt16(dataRow.Field<Int64>("Type")),
                                Convert.ToInt16(dataRow.Field<Int64>("IsActive")));
        }

        private static Func<DataRow, P_Dit_Model> GetDatarowP_Dit()
        {
            return dataRow => new P_Dit_Model(
                                Convert.ToInt16(dataRow.Field<Int64>("Id")),
                                dataRow.Field<string>("Text"),
                                Convert.ToInt16(dataRow.Field<Int64>("Type")),
                                Convert.ToInt16(dataRow.Field<Int64>("IsActive")));
        }

        private static Func<DataRow, VerbModel> GetDatarowVerbs()
        {
            return dataRow => new VerbModel(
                        dataRow.Field<string>("Infinitive"),
                        dataRow.Field<string>("Past"),
                        dataRow.Field<string>("PP"),
                        dataRow.Field<string>("Gerund"),
                        dataRow.Field<string>("Person")
                    );
        }

        #endregion

        #region Removes

        protected static bool RemoveVocabulary(VocVM Voc)
        {
            string query = string.Format(RemoveSQL, Model.Voc.ToDesc(), Voc.Id);

            if (!SendQuery(query))
                return false;

            Vocabularies.Remove(Voc);

            return true;
        }

        protected static bool RemovePronunciation(PronVM Pron)
        {
            string query = string.Format(RemoveSQL, Model.Pron.ToDesc(), Pron.Id);

            if (!SendQuery(query))
                return false;

            Pronunciations.Remove(Pron);

            return true;
        }

        protected static bool RemoveSpelling(SpellVM Spell)
        {
            string query = string.Format(RemoveSQL, Model.Spell.ToDesc(), Spell.Id);

            if (!SendQuery(query))
                return false;

            Spellings.Remove(Spell);

            return true;
        }

        protected static bool RemoveQuestionAttempt(AttemptVM att)
        {
            string query = string.Format(RemoveSQL, GetDBAttemptName(att.Type), att.Id);

            if (!SendQuery(query))
                return false;

            if (att.Type == Model.Voc)
                VocabularyAttempts.Remove(att);
            else if (att.Type == Model.Spell)
                SpellingAttempts.Remove(att);

            return true;
        }

        #endregion

        private static bool CreateIfEmptyDB()
        {
            if (!SendQuery("CREATE TABLE IF NOT EXISTS 'Vocabulary' " +
                "( 'Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "'Words' TEXT NOT NULL, " +
                "'Answer' TEXT NOT NULL, " +
                "'Definition' TEXT, " +
                "'PtBr' TEXT, " +
                "'IsActive' INTEGER NOT NULL )"))
                return false;

            if (!SendQuery("CREATE TABLE IF NOT EXISTS 'VocabularyAttempt' " +
                "( 'Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "'IdVocabulary' INTEGER NOT NULL, " +
                "'Score' INTEGER NOT NULL, " +
                "'When' TEXT NOT NULL, " +
                "FOREIGN KEY(`IdVocabulary`) REFERENCES `Vocabulary`(`Id`) )"))
                return false;

            if (!SendQuery("CREATE TABLE IF NOT EXISTS 'Pronunciation' " +
                "( 'Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "'Words' TEXT NOT NULL, " +
                "'Phonemes' TEXT NOT NULL, " +
                "'IsActive' INTEGER NOT NULL )"))
                return false;

            if (!SendQuery("CREATE TABLE IF NOT EXISTS 'Spelling' " +
                "( 'Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "'Words' TEXT NOT NULL, " +
                "'IsActive' INTEGER NOT NULL )"))
                return false;

            if (!SendQuery("CREATE TABLE IF NOT EXISTS 'SpellingAttempt' " +
                "( 'Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "'IdSpelling' INTEGER NOT NULL, " +
                "'Score' INTEGER NOT NULL, " +
                "'When' TEXT NOT NULL, " +
                "FOREIGN KEY(`IdSpelling`) REFERENCES `Spelling`(`Id`) )"))
                return false;

            if (!SendQuery(GetCreatingQueryForTemplate(Model.Essay)))
                return false;

            if (!SendQuery(GetCreatingQueryForTemplate(Model.SumRetell)))
                return false;

            if (!SendQuery(GetCreatingQueryForTemplate(Model.DescImg)))
                return false;

            if (!SendQuery("CREATE TABLE IF NOT EXISTS 'Verb' " +
                "( 'Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "'Infinitive' TEXT NOT NULL, " +
                "'Past' TEXT NOT NULL, " +
                "'PP' TEXT NOT NULL, " +
                "'Person' TEXT NOT NULL, " +
                "'Gerund' TEXT NOT NULL )"))
                return false;

            InsertStaticValuesIfEmpty(Model.Verb, CakePaths.ScriptVerbs);
            InsertStaticValuesIfEmpty(Model.Voc, CakePaths.ScriptVocabulary);

            return true;
        }

        private static void InsertStaticValuesIfEmpty(Model type, string scriptFile)
        {
            if (GetTable(type).Tables[0].Rows.Count == 0)
                SendQuery(ScriptFileCommands.GetStringFromScriptFile(scriptFile));
        }

        private static string GetDBAttemptName(Model type)
        {
            return type.ToDesc() + "Attempt";
        }

        private static string GetCreatingQueryForTemplate(Model type)
        {
            return "CREATE TABLE IF NOT EXISTS '" + type.ToDesc() + "Attempt' " +
                "( 'Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "'Id" + type.ToDesc() + "' INTEGER NOT NULL, " +
                "'Score' INTEGER NOT NULL, " +
                "'When' TEXT NOT NULL )";
        }
    }
}