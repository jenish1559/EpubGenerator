using EPubGenerator.Model;
using NHunspell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebSupergoo.ABCpdf10;
using WebSupergoo.ABCpdf10.Operations;

namespace EPubGenerator.Provider
{
    public class IncorrectWordDetailsProvider
    {
        private readonly string _filePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Dictionary\CustomDictionary.txt";
        private readonly List<string> _correctWordList = new List<string>();
        private readonly object _thisLock = new object();

        public IncorrectWordDetailsProvider()
        {
            //read custom dictinary
            if (File.Exists(_filePath))
            {
                using (StreamReader fileReader = new StreamReader(_filePath))
                {
                    var fileData = fileReader.ReadToEnd();
                    fileReader.Close();
                    fileReader.Dispose();
                    _correctWordList = fileData.Split(new[] { "\r\n" }, StringSplitOptions.None).ToList();

                }
            }
        }

        //check book word and return word details collection that contains incorrect word and replace word list.
        public List<WordDetail> getIncorrectWordList(List<string> bookwordsList)
        {
            List<string> WrongWordList = new List<string>();
            List<WordDetail> WordDetails = new List<WordDetail>();
            bookwordsList = bookwordsList.Where(a => !String.IsNullOrWhiteSpace(a)).GroupBy(a => a).Select(b => b.First()).ToList();

            WrongWordList = GetWrongWordList(bookwordsList, "English (American)");

            if (WrongWordList != null)
            {
                WrongWordList = GetWrongWordList(WrongWordList, "English (Australian)");
            }

            if (WrongWordList != null)
            {
                WrongWordList = GetWrongWordList(WrongWordList, "English (British)");
            }

            if (WrongWordList != null)
            {
                WrongWordList = GetWrongWordList(WrongWordList, "English (Canadian)");
            }

            WrongWordList = WrongWordList.Where(a => !String.IsNullOrWhiteSpace(a)).GroupBy(a => a).Select(b => b.First()).ToList();

            List<WordDetail> newWordDetails = new List<WordDetail>();

            foreach (var word in WrongWordList)
            {
                newWordDetails.Add(new WordDetail() { IncorrectWord = word });
            }
            WrongWordList = null;

            newWordDetails = checkWord(newWordDetails, "English (Canadian)");
            var replaceWordList = newWordDetails.Where(a => a.CorrectWord != null);
            WordDetails.AddRange(replaceWordList);

            if (newWordDetails != null)
            {
                newWordDetails = checkWord(newWordDetails, "English (Australian)");
                replaceWordList = newWordDetails.Where(a => a.CorrectWord != null);
                WordDetails.AddRange(replaceWordList);
            }

            if (newWordDetails != null)
            {
                newWordDetails = checkWord(newWordDetails, "English (British)");
                replaceWordList = newWordDetails.Where(a => a.CorrectWord != null);
                WordDetails.AddRange(replaceWordList);
            }
            if (newWordDetails != null)
            {
                newWordDetails = checkWord(newWordDetails, "English (American)");
                replaceWordList = newWordDetails.Where(a => a.CorrectWord != null);
                WordDetails.AddRange(replaceWordList);
            }
            if (newWordDetails != null)
            {
                newWordDetails = getCorrectWordsListFromSuggestion(newWordDetails, "English (British)");
                replaceWordList = newWordDetails.Where(a => a.CorrectWord != null);
                WordDetails.AddRange(replaceWordList);
            }

            if (newWordDetails != null)
            {
                var replaceChar = new char[] { '\\', '/', ':', '*', '?', '"', '<', '>', '|', '-' };
                WrongWordList = newWordDetails.Where(a => a.CorrectWord == null).Select(a => a.IncorrectWord).ToList();
                foreach (var word in WrongWordList)
                {
                    string saveName = replaceChar.Aggregate(word, (current, character) => current.Replace(character.ToString(), string.Empty));

                    WordDetails.Add(new WordDetail { IncorrectWord = word, CorrectWord = null, WordImageName = saveName });
                }
            }

            using (StreamWriter fileWriter = new StreamWriter(_filePath))
            {
                lock (_thisLock)
                {
                    foreach (var word in _correctWordList)
                        fileWriter.WriteLine(word);
                }
                fileWriter.Close();
            }

            return WordDetails;
        }

        //check book word  using hunspell dictionary and return wrong word list.
        private List<string> GetWrongWordList(List<string> wordList, string dictionary)
        {
            List<string> wrongWordList = new List<string>();

            var files = Directory.GetFiles(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "Dictionary"));
            string affFilePath = files.FirstOrDefault(a => a.Contains(dictionary + ".aff")).ToString();
            string dicFilePath = files.FirstOrDefault(a => a.Contains(dictionary + ".dic")).ToString();

            using (Hunspell hunspell = new Hunspell(affFilePath, dicFilePath))
            {
                foreach (var word in wordList)
                {
                    if (_correctWordList.Contains(word))
                    {
                        continue;
                    }

                    char[] trimcharacter = { '?', '.', ',', '-', ';', '!', ':', '—', '(', ')', '[', ']', '{', '}', '^', '/', '\\', '"', '*'
                                                   ,'•','⁎','&', '=',  '†',  '%'  ,'■'  ,'|', '~' ,'„'  ,'«',  '_','”'
                                                   ,'>','+', '#', '^' ,'»', '§', '<' ,'¥','―',' ','“'};
                    string newWord = word.Trim('\'').TrimEnd('$').Trim('\'').TrimStart('$').Trim(trimcharacter);

                    if (newWord.Contains("—") || newWord.Contains("-") || newWord.Contains(",")
                        || newWord.Contains("."))
                    {
                        List<string> wordarray = new List<string>();
                        if (newWord.Contains("—"))
                        {
                            wordarray = newWord.Split('—').ToList();
                        }
                        else if (newWord.Contains("-"))
                        {
                            wordarray = newWord.Split('-').ToList();
                        }
                        else if (newWord.Contains(","))
                        {
                            wordarray = newWord.Split(',').ToList();
                        }
                        else if (newWord.Contains("."))
                        {
                            wordarray = newWord.Split('.').ToList();
                        }

                        foreach (var splitword in wordarray)
                        {
                            var splitnewWord = splitword.Trim('\'').TrimEnd('$').Trim('\'').TrimStart('$').Trim(trimcharacter).Replace("⠀", "");

                            if (!hunspell.Spell(splitnewWord))
                            {
                                wrongWordList.Add(newWord);
                                break;
                            }

                            lock (_thisLock)
                            {
                                _correctWordList.Add(word);
                                break;
                            }
                        }
                    }
                    else
                    {
                        newWord = newWord.Replace("⠀", "");

                        if (!hunspell.Spell(newWord))
                        {
                            wrongWordList.Add(newWord);
                        }
                        else
                        {
                            lock (_thisLock)
                                _correctWordList.Add(word);
                        }
                    }
                }
                hunspell.Dispose();
            }
            return wrongWordList;
        }

        //code for handling the common problem of f being recognised as an s.
        private List<WordDetail> checkWord(List<WordDetail> wordList, string dictionary)
        {
            List<WordDetail> newWordList = new List<WordDetail>();
            var files = Directory.GetFiles(Path.Combine((Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName), "Dictionary"));
            string affFilePath = files.FirstOrDefault(a => a.Contains(dictionary + ".aff"));
            string dicFilePath = files.FirstOrDefault(a => a.Contains(dictionary + ".dic"));
            var WrongWordList = wordList.Where(a => a.CorrectWord == null).Select(a => a.IncorrectWord).ToList();

            using (var hunspell = new Hunspell(affFilePath, dicFilePath))
            {
                foreach (var word in WrongWordList)
                {
                    var newword = word.Replace('f', 's');
                    if (!hunspell.Spell(newword))
                    {
                        newword = word.Replace('s', 'f');
                        if (!hunspell.Spell(newword))
                        {
                            newWordList.Add(new WordDetail() { IncorrectWord = word });
                        }
                        else
                        {
                            newWordList.Add(new WordDetail()
                            {
                                IncorrectWord = word,
                                CorrectWord = newword
                            });
                        }
                    }
                    else
                    {
                        newWordList.Add(new WordDetail()
                        {
                            IncorrectWord = word,
                            CorrectWord = newword
                        });
                    }
                }
                hunspell.Dispose();
            }
            return newWordList;
        }

        // provide correct word of incorrect words using hunspell dictionary.
        private List<WordDetail> getCorrectWordsListFromSuggestion(List<WordDetail> wordList, string dictionary)
        {
            List<WordDetail> WordList = new List<WordDetail>();
            var files = Directory.GetFiles(Path.Combine((Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName), "Dictionary"));
            string affFilePath = files.FirstOrDefault(a => a.Contains(dictionary + ".aff")).ToString();
            string dicFilePath = files.FirstOrDefault(a => a.Contains(dictionary + ".dic")).ToString();
            var wrongWordList = wordList.Where(a => a.CorrectWord == null).Select(a => a.IncorrectWord).ToList();

            using (var hunspell = new Hunspell(affFilePath, dicFilePath))
            {
                foreach (var word in wrongWordList)
                {
                    var suggestion = hunspell.Suggest(word);
                    List<SuggestionWordDetail> suggestionList = new List<SuggestionWordDetail>();
                    foreach (var suggestionword in suggestion)
                    {
                        suggestionList.Add(new SuggestionWordDetail()
                        {
                            SuggestionWord = suggestionword,
                            computeWordResult = CheckWordSimilarity(word, suggestionword)
                        });
                    }

                    var result = suggestionList.Where(a => a.computeWordResult == 1).ToList();
                    if (!result.Any())
                    {
                        result = suggestionList.Where(a => a.computeWordResult == 2).ToList();
                    }

                    if (result.Count() > 3)
                    {
                        List<SuggestionWordDetail> newSuggestionWordDetail = new List<SuggestionWordDetail>();
                        for (int index = 0; index < 3; index++)
                        {
                            newSuggestionWordDetail.Add(result[index]);
                        }

                        result = newSuggestionWordDetail;
                    }

                    if (result.Count() == 1)
                    {
                        WordList.Add(new WordDetail()
                        {
                            IncorrectWord = word,
                            CorrectWord = result.First().SuggestionWord
                        });
                    }
                    else if (result.Count() > 1)
                    {
                        var samelenghtword = result.Where(a => a.SuggestionWord.Length == word.Length);
                        var spaceword = result.Where(a => (a.SuggestionWord.Length - word.Length == 1 ||
                                                     a.SuggestionWord.Length - word.Length == -1) &&
                                                     a.SuggestionWord.Contains(' ') ||
                                                     (a.SuggestionWord.Length + 1 == word.Length
                                                     && word.Contains('\'')));
                        if (samelenghtword.Count() == 1)
                        {
                            WordList.Add(new WordDetail()
                            {
                                IncorrectWord = word,
                                CorrectWord = samelenghtword.First().SuggestionWord
                            });
                        }
                        else if (spaceword.Count() == 1)
                        {
                            WordList.Add(new WordDetail()
                            {
                                IncorrectWord = word,
                                CorrectWord = spaceword.First().SuggestionWord
                            });
                        }
                        else if (spaceword.Count() > 1)
                        {
                            List<string> newWordList = new List<string>();
                            foreach (var oldword in spaceword)
                            {
                                var newword = oldword.SuggestionWord.Replace(" ", "");
                                newWordList.Add(newword);
                            }
                            var newSuggestionWord = newWordList.Where(a => a.ToLower() == word.ToLower());

                            if (newSuggestionWord.Any())
                            {
                                WordList.Add(new WordDetail()
                                {
                                    IncorrectWord = word,
                                    CorrectWord = spaceword.First().SuggestionWord
                                });
                            }
                            else
                            {
                                WordList.Add(new WordDetail() { IncorrectWord = word });
                            }
                        }
                        else
                        {
                            WordList.Add(new WordDetail() { IncorrectWord = word });
                        }
                    }
                    else
                    {
                        WordList.Add(new WordDetail() { IncorrectWord = word });
                    }
                }
                hunspell.Dispose();
            }
            return WordList;
        }

        //provide orignal word and suggestion word similarity.
        public int CheckWordSimilarity(string orignalWord, string suggestionWord)
        {
            int orignalWordLength = orignalWord.Length;
            int suggestionWordLength = suggestionWord.Length;
            int[,] dictionary = new int[orignalWordLength + 1, suggestionWordLength + 1];

            // Step 1
            if (orignalWordLength == 0)
            {
                return suggestionWordLength;
            }

            if (suggestionWordLength == 0)
            {
                return orignalWordLength;
            }

            // Step 2
            for (int index = 0; index <= orignalWordLength; dictionary[index, 0] = index++)
            {
            }

            for (int index = 0; index <= suggestionWordLength; dictionary[0, index] = index++)
            {
            }

            // Step 3
            for (int index = 1; index <= orignalWordLength; index++)
            {
                //Step 4
                for (int innerIndex = 1; innerIndex <= suggestionWordLength; innerIndex++)
                {
                    // Step 5
                    int cost = (suggestionWord[innerIndex - 1] == orignalWord[index - 1]) ? 0 : 1;

                    // Step 6
                    dictionary[index, innerIndex] = Math.Min(
                        Math.Min(dictionary[index - 1, innerIndex] + 1, dictionary[index, innerIndex - 1] + 1),
                        dictionary[index - 1, innerIndex - 1] + cost);
                }
            }
            // Step 7
            return dictionary[orignalWordLength, suggestionWordLength];
        }

        public bool GenerateIncorrectWordImage(string pdfPath, List<WordDetail> wrongWordList, string outputPath)
        {
            using (Doc pdf = new Doc())
            {
                pdf.Read(pdfPath);

                //MainWindowVM.log.Info(Path.GetFileNameWithoutExtension(pdfPath) + " book TextOpertaion Start.");

                TextOperation textOperation = new TextOperation(pdf);
                textOperation.PageContents.AddPages();

                string plainText = textOperation.GetText();

                //MainWindowVM.log.Info(Path.GetFileNameWithoutExtension(pdfPath) + " book TextOpertaion Completed.");

                var allRows = plainText.Split('\n').ToArray();

                var wrongWordsDetails = wrongWordList.Where(a => a.CorrectWord == null);

                foreach (var word in wrongWordsDetails)
                {
                    int position = 0;
                    position = plainText.IndexOf(word.IncorrectWord, position, StringComparison.CurrentCultureIgnoreCase);
                    if (position > 0)
                    {
                        IList<TextFragment> theSelection = textOperation.Select(position, word.IncorrectWord.Length);
                        IList<TextGroup> theGroups = textOperation.Group(theSelection);

                        foreach (TextGroup theGroup in theGroups)
                        {
                            if (theGroup.Text.ToLower() != word.IncorrectWord.ToLower())
                                continue;

                            pdf.Page = theGroup.PageID;

                            pdf.Rendering.DotsPerInchX = Constants.DotsPerInchX;
                            pdf.Rendering.DotsPerInchY = Constants.DotsPerInchY;

                            var height = theGroup.Rect.Height;

                            var currentWordRow = Array.IndexOf(allRows, allRows.First(a => a.ToLower().Contains(word.IncorrectWord.ToLower())));
                            if (currentWordRow > 0)
                            {
                                var previousWordRow = allRows[currentWordRow - 1];

                                //if previous row is blank then this code can check upto 10 previous row. 
                                if (previousWordRow.Trim() == "")
                                {
                                    for (int index = 2; index <= 10; index++)
                                    {
                                        previousWordRow = allRows[currentWordRow - index];
                                        if (previousWordRow.Trim() != "")
                                            break;
                                    }
                                }

                                var previousRowFirstword = previousWordRow.ToString().Split(' ')[0].ToString();
                                int previousWordPosition = 0;
                                var newPlaintext = plainText.Substring(0, position);
                                previousWordPosition = newPlaintext.LastIndexOf(previousRowFirstword, StringComparison.CurrentCultureIgnoreCase);

                                if (previousWordPosition < 0)
                                    break;

                                IList<TextFragment> previousRowtheSelection = textOperation.Select(previousWordPosition, previousRowFirstword.Length);
                                IList<TextGroup> previousRowtheGroups = textOperation.Group(previousRowtheSelection);
                                foreach (TextGroup thegroup in previousRowtheGroups)
                                {
                                    var previousWordBottom = Convert.ToDouble(thegroup.Rect.Bottom);
                                    var currentWordTop = Convert.ToDouble(theGroup.Rect.Top);

                                    if (previousWordBottom < currentWordTop)
                                    {
                                        theGroup.Rect.Height = height - (currentWordTop - previousWordBottom);
                                    }
                                }
                                previousRowtheSelection.Clear();
                                previousRowtheGroups.Clear();
                                previousRowtheSelection = null;
                                previousRowtheGroups = null;
                            }

                            pdf.Rect.Left = theGroup.Rect.Left - Constants.ImageLeftMargin;
                            pdf.Rect.Bottom = theGroup.Rect.Bottom;
                            pdf.Rect.Width = theGroup.Rect.Width + Constants.ImageRightMargin;
                            if (theGroup.Rect.Height > 5)
                                pdf.Rect.Height = theGroup.Rect.Height;
                            else
                                pdf.Rect.Height = height;

                            pdf.Rendering.Save(outputPath + @"\" + word.WordImageName + ".png");
                        }

                        theGroups.Clear();
                        theSelection.Clear();

                        theGroups = null;
                        theSelection = null;
                    }
                }


                plainText = null;
                allRows = null;

                textOperation = null;
                pdf.Clear();
                return true;
            }
        }
    }
}
