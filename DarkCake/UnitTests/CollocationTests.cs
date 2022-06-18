using AussieCake.Challenge;
using AussieCake.Question;
using AussieCake.Util;
using AussieCake.Verb;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AussieCake.UnitTests
{
    [TestClass()]
    public class VocabularyTests
    {
        [TestMethod()]
        public void CheckTimeSpentToValidSentenceOnSimpleVocabulary()
        {
            //var model = new VocModel("", "experience", 1, "", "difficulties", 0, "", "", "", 0, 1);
            //var vm = model.ToVM();
            //var sen = "Most Parties are likely to experience difficulties in implementing measures.";

            //var added = new List<string>();

            //var maxSecondsToSpend = 1;
            //var watcher = new Stopwatch();
            //watcher.Start();

            //var result = Sentences.DoesSenContainsVoc(vm, sen) && !added.Contains(sen);

            //watcher.Stop();

            //Assert.IsTrue(result);
            //Assert.IsTrue(maxSecondsToSpend >= watcher.Elapsed.TotalSeconds);
        }

        [TestMethod()]
        public void CheckTimeSpentToValidSentenceOnFullVocabulary()
        {
            //var model = new VocModel("", "take", 1, "up the", "role", 0, "of;as", "", "", 0, 1);
            //var vm = model.ToVM();
            //var sen = "The commission may also take up the role of the City Council and rules governing land use.";

            //var added = new List<string>();

            //var maxSecondsToSpend = 1;
            //var watcher = new Stopwatch();
            //watcher.Start();

            //var result = Sentences.DoesSenContainsVoc(vm, sen) && !added.Contains(sen);

            //watcher.Stop();

            //Assert.IsTrue(result);
            //Assert.IsTrue(maxSecondsToSpend >= watcher.Elapsed.TotalSeconds);
        }

        [TestMethod()]
        public void TestCheckVocabularyInSentenceMethodIntegraty()
        {
            //var model = new VocModel("", "take", 1, "up the", "role", 0, "of;as", "", "", 0, 1);
            //var vm = model.ToVM();
            //var sen = "The commission may also take up the role of the City Council and rules governing land use.";
            //Assert.IsTrue(Sentences.DoesSenContainsVoc(vm, sen));

            //model = new VocModel("", "experience", 1, "", "difficulties", 0, "", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "Most Parties are likely to experience difficulties in implementing measures.";
            //Assert.IsTrue(Sentences.DoesSenContainsVoc(vm, sen));

            //model = new VocModel("", "take", 1, "up the", "role", 0, "of;as", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "The commission may also should have taken up the role of the City Council and rules governing land use.";
            //Assert.IsTrue(Sentences.DoesSenContainsVoc(vm, sen));

            //model = new VocModel("cute;adorable;the", "dog", 0, "and the;together with", "cat", 0, "fat;stupid;malicious", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "The fat dog together with the stupid cat made a adorable malicious party.";
            //Assert.IsTrue(Sentences.DoesSenContainsVoc(vm, sen));

            //model = new VocModel("cute;adorable;the", "dog", 0, "and the;together with; and a", "cat", 0, "fat;stupid;malicious", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "Nowadays it is amazing to get a cute dog and a cat at a cute, but malicious home.";
            //Assert.IsTrue(Sentences.DoesSenContainsVoc(vm, sen));

            //model = new VocModel("cute;adorable;the", "dog", 0, "and the;together with", "cat", 0, "fat;stupid;malicious", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "An adorable cow together with a dog and the cat are the most malicious pets.";
            //Assert.IsTrue(Sentences.DoesSenContainsVoc(vm, sen));

            //model = new VocModel("", "experience", 1, "", "difficulties", 0, "", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "Most difficulty Parties experienced difficulties in their experiences.";
            //Assert.IsFalse(Sentences.DoesSenContainsVoc(vm, sen));
            //// Fails because there're two Component1

            //model = new VocModel("cute;adorable;the", "dog", 0, "and the;together with", "cat", 0, "fat;stupid;malicious", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "Adorable dogs together with cats that are fat can be the moment to your realise how cute dogs are.";
            //Assert.IsFalse(Sentences.DoesSenContainsVoc(vm, sen));
            //// Fails because there're two Component1

            //model = new VocModel("", "experience", 1, "", "difficulties", 0, "", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "Most Parties have difficulties to experienced difficulty in their experiences.";
            //Assert.IsFalse(Sentences.DoesSenContainsVoc(vm, sen));
            //// Fails because the Component2 requires plural. 
            //// Algorithm just change form when it is singular to plural, not the opposite.

            //model = new VocModel("", "take", 1, "up the", "role", 0, "of;as", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "The commission may also role up the take of the City Council and rules governing land use.";
            //Assert.IsFalse(Sentences.DoesSenContainsVoc(vm, sen));
            //// Fails because Componenet2 comes before Component1

            //model = new VocModel("cute;adorable;the", "dog", 0, "and the;together with", "cat", 0, "fat;stupid;malicious", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "An adorable cat together with dogs that are fat can be the moment to your realise how cute they are.";
            //Assert.IsFalse(Sentences.DoesSenContainsVoc(vm, sen));
            //// Fails because Component2 comes before Component1

            //model = new VocModel("cute;adorable;the", "dog", 0, "and the;together with; and a", "cat", 0, "fat;stupid;malicious", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "Nowadays it is amazing to get a cute dog and a stupid cat at a cute home.";
            //Assert.IsFalse(Sentences.DoesSenContainsVoc(vm, sen));
            //// Fails because Suffixe comes before Component2

            //model = new VocModel("cute;adorable;the", "dog", 0, "and the;together with", "cat", 0, "fat;stupid;malicious", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "A cow together with a dog can be the best pet ever, much better than a malicious cat.";
            //Assert.IsFalse(Sentences.DoesSenContainsVoc(vm, sen));
            //// Fails because Link comes before Component1

            //model = new VocModel("cute;adorable;the", "dog", 0, "and the;together with", "cat", 0, "fat;stupid;malicious", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "An adorable cow together with a dog can be the best pets ever, and the cat the most stupid one.";
            //Assert.IsFalse(Sentences.DoesSenContainsVoc(vm, sen));
            //// Fails because Link comes before Component1

            //model = new VocModel("cute;adorable;the", "dog", 0, "and the;together with", "cat", 0, "fat;stupid;malicious", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "The very smart and fat animal that was named as dog, together with the stupid cat made a adorable malicious party.";
            //Assert.IsFalse(Sentences.DoesSenContainsVoc(vm, sen));
            //// Fails because Prefix is too far away from Component1

            //model = new VocModel("cute;adorable;the", "dog", 0, "and the;together with;that", "cat", 0, "fat;stupid;malicious", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "That cute dog, because of its long corpse, is causing trouble together with the cat again, so that, I need to kill it.";
            //Assert.IsFalse(Sentences.DoesSenContainsVoc(vm, sen));
            //// Fails because Component1 is too far away from Component2

            //model = new VocModel("cute;adorable;the", "dog", 0, "and the;together with", "cat", 0, "fat;stupid;malicious", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "The fat dog together with the stupid cat made a adorable dance which seemed like a malicious party.";
            //Assert.IsFalse(Sentences.DoesSenContainsVoc(vm, sen));
            //// Fails because Suffix is too far away from Component2

            //model = new VocModel("cute;adorable;the", "dog", 0, "and the;together with", "cat", 0, "fat;stupid;malicious", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "Adorable dogs together with wolfs are the perfect picture of the example that cats are stupid.";
            //Assert.IsFalse(Sentences.DoesSenContainsVoc(vm, sen));
            //// Fails because Link is too far away from Component2

            //model = new VocModel("cute;adorable;the", "dog", 0, "and the;together with", "cat", 0, "fat;stupid;malicious", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "The fat dog was stupid and with the stupid cat made a adorable malicious party.";
            //Assert.IsFalse(Sentences.DoesSenContainsVoc(vm, sen));
            //// Fails because there's no Link

            //model = new VocModel("cute;adorable;the", "dog", 0, "and the;together with", "cat", 0, "fat;stupid;malicious", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "The dog and the rat are the perfect example why you should never have a tremendous stupid cat which is also fat.";
            //Assert.IsFalse(Sentences.DoesSenContainsVoc(vm, sen));
            //// Fails because there's no Link

            //model = new VocModel("cute;adorable;the", "dog", 0, "and the;together with", "cat", 0, "fat;stupid;malicious", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "Who would like to have a fat dog these days when you can have a fat cat instead?";
            //Assert.IsFalse(Sentences.DoesSenContainsVoc(vm, sen));
            //// Fails because there's no Prefix

            //model = new VocModel("cute;adorable;the", "dog", 0, "and the;together with", "cat", 0, "fat;stupid;malicious", "", "", 0, 1);
            //vm = model.ToVM();
            //sen = "The fat dog together with the stupid cat made a adorable party.";
            //Assert.IsFalse(Sentences.DoesSenContainsVoc(vm, sen));
            //// Fails because there's no Suffix
        }

        [TestMethod()]
        public void TestIsVerbIntegraty()
        {
            foreach (VocVM Voc in QuestControl.Get(Model.Voc))
            {
                //if (Voc.IsComp1Verb && !Voc.Component1.IsVerb())
                //    Debug.WriteLine(Voc.Component1 + " is set as Verb, but the method said no");

                //if (!Voc.IsComp1Verb && Voc.Component1.IsVerb())
                //    Debug.WriteLine(Voc.Component1 + " is NOT set as Verb, but the method said yes");

                //if (Voc.IsComp2Verb && !Voc.Component2.IsVerb())
                //    Debug.WriteLine(Voc.Component2 + " is set as Verb, but the method said no");

                //if (!Voc.IsComp2Verb && Voc.Component2.IsVerb())
                //    Debug.WriteLine(Voc.Component2 + " is NOT set as Verb, but the method said yes");
            }
        }
    }
}
