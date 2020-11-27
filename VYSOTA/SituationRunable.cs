using System;

namespace VYSOTA
{
    public class SituationRunable
    {
        private string desc;
        private string opt1;
        private string opt2;
        private bool badIsFirst;

        public SituationRunable(string desc, string opt1, string opt2, bool badIsFirst)
        {
            this.desc = desc;
            this.opt1 = opt1;
            this.opt2 = opt2;
            this.badIsFirst = badIsFirst;
        }

        void run()
        {
            Console.WriteLine(desc);
            Console.WriteLine("A. " + opt1 + "\nB. " + opt2);
            Console.Write("-> ");
            
            string choose = Console.ReadLine();
            
        }
    }
}