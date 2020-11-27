using System;
using System.Collections.Generic;
using System.Security.Permissions;

namespace VYSOTA
{
    public class Expedition
    {
        private List<Member> crew;
        
        private int rsstSum;
        private int tchSum;
        private int strngSum;
        private int pshSum;
        
        private int ds;
        private int stp;
        private int fd;
        private string[] tp;

        private int delta;

        public Expedition(List<Member> crew)
        {
            this.crew = crew;
//            memberCount = crew.Length;
            updTeamStats();
        }

        private void updTeamStats()
        {
            rsstSum = 0;
            tchSum = 0;
            strngSum = 0;
            pshSum = 0;
            delta = -5;
            
            foreach (var member in crew)
            {
                rsstSum += member.Rsst;
                tchSum += member.Tch;
                strngSum += member.Strng;
                pshSum += member.Psh;
            }
        }
        
        public void dayUpdStats()
        {
            ds++;
            foreach (var member in crew)
            {
                member.updStats(delta, delta, delta, delta);
            }
            kill();
            updTeamStats();

//            fd -= memberCount;
            stp = 0;
//            Console.WriteLine(this.ToString());
        }

        public void tick()
        {
            stp++;
            if (stp % 3 == 0) dayUpdStats();
            else
            {
                tickUPD();
                Console.WriteLine(this.ToString());
            }
        }

        private void tickUPD()
        {
            kill();
            updTeamStats();
        }

        public void kill()
        {
            int j = 0;
            for (int i = 0; i < crew.Count; i++)
            {
                if (crew[j].DthStatus) crew.Remove(crew[j]);
                else j++;
            }
            foreach (var crewmate in crew)
            {
                if (crewmate.DthStatus) crew.Remove(crewmate);
            }
        }

        public override string ToString()
        {
            return string.Format(
                "====== DAY {0} ======\n------ STEP {6} ------\n team rsst = {1}\n team tch = {2}\n team strng =  {3}\n team psh = {4}\n in the crew = {5}\n",
                ds, rsstSum, tchSum, strngSum, pshSum, crew.Count, stp);
        }

        public List<Member> Crew
        {
            get { return crew; }
        }

        public int RsstSum
        {
            get { return rsstSum; }
        }

        public int TchSum
        {
            get { return tchSum; }
        }

        public int StrngSum
        {
            get { return strngSum; }
        }

        public int PshSum
        {
            get { return pshSum; }
        }
    }
}