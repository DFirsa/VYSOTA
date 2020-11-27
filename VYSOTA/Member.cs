using System;
using System.Xml;

namespace VYSOTA
{
    public class Member
    {
        private string name;
        private readonly int lifeTreshhold = -30;
        
        private bool dthStatus = false;
        
        private int rsst;
        private int tch;
        private int strng;
        private int psh;
        private bool hl;
        
        private int statBonus;
        private int rsstBonus;
        private int strngBonus;

        private string cls; //class of memeber 
        
        public Member(int rsst, int tch, int strng, int psh, string cls, string name)
        {
            hl = false;
            this.rsst = rsst;
            this.tch = tch;
            this.strng = strng;
            this.psh = psh;
            this.cls = cls;
            this.name = name;
            statBonus = 0;
        }

//        getters
        public bool DthStatus
        {
            get { return dthStatus; }
        }

        public int Rsst
        {
            get { return rsst; }
        }

        public int Tch
        {
            get { return tch; }
        }

        public int Strng
        {
            get { return strng; }
        }

        public int Psh
        {
            get { return psh; }
            set { psh = value; }
        }

        public string Cls
        {
            get { return cls; }
        }

        public bool Hl
        {
            get {return hl;}
        }

        public int StatBonus
        {
            set { statBonus = value; }
            get {return statBonus;}
        }
        
        public int RsstBonus
        {
            set { rsstBonus = value; }
        }

        public int StrngBonus
        {
            set { strngBonus = value; }
        }

        public void kill()
        {
            dthStatus = true;
        }

//        setter
        public void updStats(int rsstDelta, int tchDelta, int strngDelta, int pshDelta)
        {
            rsst += rsstDelta + statBonus + rsstBonus;
            tch += tchDelta + statBonus;
            strng += strngDelta + statBonus + strngBonus;
            psh += pshDelta + statBonus;
            
            checkAlive();
        }

        private void checkAlive()
        {
            if ((rsst < lifeTreshhold) || (tch < lifeTreshhold) || (strng < lifeTreshhold) || (psh < lifeTreshhold))
                kill();
        }

        public override string ToString()
        {
            return string.Format("{0} stats : rsst={1} tch= {2} strng={3} psh={4}", name, rsst, tch, strng, psh);
        }

        public string Name
        {
            get { return name; }
        }

        public void getHl()
        {
            hl = true;
        }
    }
}