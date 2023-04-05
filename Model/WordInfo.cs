using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPubGenerator.Model
{
    public class WordInfo
    {
        public string IncorrectWord { get; set; } 
        public string CorrectWord { get; set; }
        public string WordImageName { get; set; }
        public string OcrText { get; set; }
        public int FileNo { get; set; }
        public int ImgHeight { get; set; }
        public int ImgWidth { get; set;}
        public int index { get; set; }
        public int LineNo { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double Confidence { get; set; }
    }
}
