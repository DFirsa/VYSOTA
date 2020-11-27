using System;
using System.Collections.Generic;

namespace VYSOTA
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Member mem1 = new Member(30, 50, 60, 90, "sntst", "Biba");
            Member mem2 = new Member(45, 80, 75, 20, "mchn", "Boba");
            Member mem3 = new Member(30, 20, 20, 45, "sst", "Pupa");
            Member mem4 = new Member(30, 50, 40, 40, "sst", "Lupa");
            Member mem5 = new Member(45, 60, 90, 90, "mltr", "Arcadiy");
            mem5.getHl();
            
            List<Member> crew = new List<Member>() {mem1, mem2, mem3, mem4, mem5};
            Expedition exp = new Expedition(crew);
            
            Situation sit = new Situation(exp);
            sit.run();
        }
    }
}