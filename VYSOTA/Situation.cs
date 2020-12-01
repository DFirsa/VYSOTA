using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Security;

namespace VYSOTA
{
    public class Situation
    {
        private Expedition expedition;

        private readonly int hl = 20;

        private bool check(int val, int extra)
        {
            Random random = new Random();
            int generated = random.Next(0, 100);
            Console.WriteLine("Generate: " + generated + " but ur: " + (val + extra));
            return (val + extra) > generated;
        }

        private int chooseMember()
        {
            Random random = new Random();
            return random.Next(0, expedition.Crew.Count - 1);
        }

        public Situation(Expedition expedition)
        {
            this.expedition = expedition;
        }

        public void sit1()
        {
            Console.WriteLine(expedition.ToString());

            Console.WriteLine(
                "У нас впереди участок с ледяными расщелинами! Джеймс говорит, что чтобы обойти его нам понадобится целый день!");
            Console.WriteLine("1. Попробуйте обойти его");
            Console.WriteLine("2. Продвигайтесь очень осторожно");
            Console.Write("-> ");

            string choose = Console.ReadLine();
            if (choose.Equals("1"))
            {
                expedition.dayUpdStats();
            }
            else
            {
                if (!check(expedition.TchSum / expedition.Crew.Count, 20))
                {
                    Member mate = expedition.Crew[0];
                    foreach (var crewmate in expedition.Crew)
                    {
                        if (crewmate.Tch < mate.Tch) mate = crewmate;
                    }

                    Console.WriteLine(
                        string.Format(
                            "{0} провалился в расщелину, черт! По моему он сломал себе ногу. Ему становится хуже!",
                            mate.Name));
                    Console.WriteLine("1. Попробуйте оказать ему первую помощь на месте.");
                    Console.WriteLine(
                        "2. Пусть 2 человека попробуют его вернуть в лагерь, а 2 других продолжат маршрут");
                    Console.Write("-> ");

                    choose = Console.ReadLine();
                    if (choose.Equals("1"))
                    {
                        int count = 0;
                        foreach (var crewmate in expedition.Crew)
                        {
                            if (crewmate.Hl) count++;
                        }

                        if (!check(count * hl, 30))
                        {
                            mate.kill();
                            expedition.kill();
                            Console.WriteLine(
                                "У него был открытый перелом, он потерял много крови. К сожалению он умер.");

                        }
                        else
                        {
                            Console.WriteLine(
                                "Все оказалось не так страшно, он получил сильный ушиб, мы можем двигаться дальше");
                            expedition.dayUpdStats();
                        }
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Кто будет сопровождать {0}", mate.Name));
                        int i = -1;
                        foreach (var crewmate in expedition.Crew)
                        {
                            i++;
                            if (crewmate.Equals(mate)) continue;
                            Console.WriteLine(string.Format("{0}. {1}", i, crewmate.Name));
                        }

                        Console.Write("first is -> ");
                        int first = Convert.ToInt32(Console.ReadLine());
                        Console.Write("second is -> ");
                        int second = Convert.ToInt32(Console.ReadLine());

                        Member f1 = expedition.Crew[first];
                        Member f2 = expedition.Crew[second];
                        expedition.Crew.Remove(f1);
                        expedition.Crew.Remove(f2);
                        expedition.Crew.Remove(mate);
                        expedition.tick();
                    }
                }
            }
        }

        public void sit2()
        {

            //INPUT
            Console.WriteLine(expedition.ToString());
            Console.WriteLine(
                "Мне кажется намечается сильная непогода!  Ветер усиливается, да и снег валит так, что становится плохо различать ориентиры. В принципе, пока мы можем двигаться и по компасу.");
            Console.WriteLine("1. Двигайтесь дальше!");
            Console.WriteLine("2. Вам нужно разбить лагерь и переждать!");
            Console.Write("-> ");

            string choose = Console.ReadLine();

            if (choose.Equals("1"))
            {
                if (check(expedition.RsstSum / expedition.Crew.Count, 20))
                    Console.WriteLine(
                        "Мы вышли из зоны с сильным ветром, правда Томми и Стэн продрогли до костей, мне кажется это кончится простудой ");
                else
                {
                    if (expedition.Crew.Count > 2)
                    {
                        Member m1 = new Member(0, 0, 0, 0, "", "");
                        Member m2 = new Member(0, 0, 0, 0, "", "");
                        if (expedition.Crew[0].Rsst > expedition.Crew[1].Rsst)
                        {
                            m1 = expedition.Crew[1];
                            m2 = expedition.Crew[0];
                        }
                        else
                        {
                            m1 = expedition.Crew[1];
                            m2 = expedition.Crew[0];
                        }

                        for (int i = 2; i < expedition.Crew.Count; i++)
                        {
                            if (expedition.Crew[i].Rsst < m2.Rsst)
                            {
                                if (expedition.Crew[i].Rsst < m1.Rsst)
                                {
                                    m2 = m1;
                                    m1 = expedition.Crew[i];
                                }
                                else m2 = expedition.Crew[i];
                            }
                        }
                    }
                    else
                    {
                        expedition.Crew[0].StatBonus = -5;
                        expedition.Crew[1].StatBonus = -5;
                    }
                }

                expedition.tick();
            }
            else expedition.dayUpdStats();
        }

        public void sit3()
        {
            //INPUT
            Console.WriteLine(expedition.ToString());
            Console.WriteLine("По моему с гор надвигается сильный туман, надеюсь мы успеем проскочить его!");
            Console.WriteLine("1. Продвигайтесь дальше! Удачи вам!");
            Console.WriteLine("2. Разбейте лагерь и переждите!");
            Console.Write("-> ");

            string choose = Console.ReadLine();
            if (choose.Equals("1"))
            {
                if (check(expedition.TchSum / expedition.Crew.Count, 40)) expedition.tick();
                else
                {
                    Console.WriteLine(
                        "Мы немного потерялись, уже все в порядке, но сил двигаться дальше у нас уже нет!");
                    expedition.dayUpdStats();
                }
            }
            else expedition.dayUpdStats();
        }

        public void sit4()
        {
            //INPUT
            Console.WriteLine(expedition.ToString());
            Console.WriteLine(
                "У нас впереди какой-то утес, мы практически у его подножья. Обходить будет довольно долго, у нас достаточно оборудования, чтобы подняться на него, и двигаться далше по маршруту.");
            Console.WriteLine("1. Поднимайтесь!");
            Console.WriteLine("2. Обойдите этот утес!");
            Console.Write("-> ");

            string choose = Console.ReadLine();
            if (choose.Equals("2"))
                expedition.dayUpdStats();
                Console.WriteLine(
                    "Мы двигались целый день, но он не заканчивается, видимо все равно прийдется подняться на него!");
            if (check(expedition.TchSum / expedition.Crew.Count, 30)) expedition.tick();
            else
            {
                Member m1 = new Member(0, 0, 0, 0, "", "");
                Member m2 = new Member(0, 0, 0, 0, "", "");
                if (expedition.Crew[0].Tch > expedition.Crew[1].Tch)
                {
                    m1 = expedition.Crew[1];
                    m2 = expedition.Crew[0];
                }
                else
                {
                    m1 = expedition.Crew[1];
                    m2 = expedition.Crew[0];
                }

                for (int i = 2; i < expedition.Crew.Count; i++)
                {
                    if (expedition.Crew[i].Rsst < m2.Tch)
                    {
                        if (expedition.Crew[i].Rsst < m1.Tch)
                        {
                            m2 = m1;
                            m1 = expedition.Crew[i];
                        }
                        else m2 = expedition.Crew[i];
                    }
                }
                
                Console.WriteLine(string.Format(
                    "{0} и {1} были в связке и сорвались во время подъема. Мы спускаемся вниз, помочь им.", m1.Name,
                    m2.Name));
                Console.WriteLine("1. Если с ними что-то не так возращайтесь в лагерь!");
                Console.WriteLine("2. Надеюсь с ними все в порядке!");
                Console.Write("-> ");

                choose = Console.ReadLine();

                if (choose.Equals("1"))
                {
                    int count = 0;
                    foreach (var crewmate in expedition.Crew)
                    {
                        if (crewmate.Hl) count++;
                    }

                    if (check(count * hl, 50))
                    {
                        Console.WriteLine("Несколько синяков и царапин, удивительно крепкие парни!");
                        expedition.dayUpdStats();
                    }
                    else
                    {
                        Console.WriteLine("У них серъезные травмы, мы возращаемся в лагерь.");
                        throw new Exception("GAME OVER");
                    }
                }
                else
                {
                    int count = 0;
                    foreach (var crewmate in expedition.Crew)
                    {
                        if (crewmate.Hl) count++;
                    }
                    if (check(count * hl, 50))
                    {
                        Console.WriteLine("Все оказалось не так страшно, они получили сильные ушибы, мы можем двигаться дальше");
                        expedition.dayUpdStats();
                    }
                    else
                    {
                        Console.WriteLine("Черт, они оба погибли!");
                        m1.kill();
                        m2.kill();
                        expedition.dayUpdStats();
                    }
                }
            }
        }

        public void sit5()
        {
            //INPUT
            Console.WriteLine(expedition.ToString());
            Console.WriteLine("Мы сделали небольшой привал, а когда отошли от места, где останавливались, поняли что забыли часть баллонов с газом для горелки!");
            Console.WriteLine("1. Попробуйте отыскать их");
            Console.WriteLine("2. Ладно, нет времени возвращаться и искать их!");
            Console.Write("-> ");

            string choose = Console.ReadLine();
            if (choose.Equals("1"))
            {
                int sntst = 0;
                int sst = 0;
                foreach (var mate in expedition.Crew)
                {
                    if (mate.Cls.Equals("sntst")) sntst++;
                    if (mate.Cls.Equals("sst")) sst++;
                }

                if (check(sntst * 30 + sst * 20, 0))
                {
                    Console.WriteLine("Вы нашли пятиконечные камни... Теперь живите с этим фактом.");
                }
                else
                {
                    Console.WriteLine("Мы нихрена не нашли эти балоны, но есть несколько зеленых камней на которые наткнулся Курт, они немного смахивают на то, что описывал Лейк, но я не уверен, они очень сильно повреждены.");
                }
            }
            expedition.tick();
        }

        public void sit6()
        {
            //INPUT
            Console.WriteLine(expedition.ToString());
            Console.WriteLine("Во время стоянки поднялся шквальный ветер, снесло одну из палаток, когда Том пытался поставить ее. Мы можем сделать небольшое укрытие в одном из сугробов.");
            Console.WriteLine("1. Да, отличная идея!");
            Console.WriteLine("2. Попробуйте отыскать ее!");
            Console.Write("-> ");

            string choose = Console.ReadLine();
            if (choose.Equals("1"))
            {
                if (!check(expedition.RsstSum / expedition.Crew.Count, 20))
                {
                    foreach (var mate in expedition.Crew)
                    {
                        mate.RsstBonus = 5;
                    }
                }
                expedition.tick();
            }
            else
            {
                expedition.dayUpdStats();
                int sntst = 0;
                int sst = 0;
                foreach (var mate in expedition.Crew)
                {
                    if (mate.Cls.Equals("sntst")) sntst++;
                    if (mate.Cls.Equals("sst")) sst++;
                }

                if (!check(sntst * 30 + sst * 20, 0))
                {
                    Member m = expedition.Crew[0];
                    foreach (var mate in expedition.Crew)
                    {
                        if (mate.Rsst < m.Rsst) m = mate;
                    }

                    m.StatBonus = m.StatBonus - 5;
                    Console.WriteLine(string.Format("{0} продрог до костей, мне кажется это кончится простудой. ",
                        m.Name));
                }
                expedition.tick();
            }
        }

        public void sit7()
        {

            Member target = expedition.Crew[chooseMember()];
            
            //INPUT
            Console.WriteLine(expedition.ToString());
            Console.WriteLine(string.Format(
                "{0} плохо просушил ботинки, черт! Мне кажется он обморозил себе ноги! Мы остановимся чтобы все проверить!",
                target.Name));
            Console.WriteLine("1. Окажите ему первую помощь!");
            Console.WriteLine("2. Парни, у нас нет времени на это, пусть он попробует дотянуть до следующей стоянки!");
            Console.Write("-> ");

            string choose = Console.ReadLine();
            if (choose.Equals("1"))
            {
                expedition.dayUpdStats();
                if (!check(target.Rsst, 40))
                {
                    Console.WriteLine(string.Format("Кто будет сопровождать {0}", target.Name));
                    int i = -1;
                    foreach (var crewmate in expedition.Crew)
                    {
                        i++;
                        if (crewmate.Equals(target)) continue;
                        Console.WriteLine(string.Format("{0}. {1}", i, crewmate.Name));
                    }

                    Console.Write("first is -> ");
                    int first = Convert.ToInt32(Console.ReadLine());
                    int second = 0;
                    if (expedition.Crew.Count > 2)
                    {
                        Console.Write("second is -> ");
                        second = Convert.ToInt32(Console.ReadLine());
                    }
                    

                    Member f1 = expedition.Crew[first];
                    expedition.Crew.Remove(f1);
                    if (expedition.Crew.Count > 2)
                    {
                        Member f2 = expedition.Crew[second];
                        expedition.Crew.Remove(f2);
                    }
                    expedition.Crew.Remove(target);
                    expedition.tick();
                }
                else expedition.tick();
            }
            else
            {
                if (check(target.Rsst, 10))
                {
                    Console.WriteLine(string.Format("Кто будет сопровождать {0}", target.Name));
                    int i = -1;
                    foreach (var crewmate in expedition.Crew)
                    {
                        i++;
                        if (crewmate.Equals(target)) continue;
                        Console.WriteLine(string.Format("{0}. {1}", i, crewmate.Name));
                    }

                    Console.Write("first is -> ");
                    int first = Convert.ToInt32(Console.ReadLine());
                    int second = 0;
                    if (expedition.Crew.Count > 2)
                    {
                        Console.Write("second is -> ");
                        second = Convert.ToInt32(Console.ReadLine());
                    }
                    
                    Member f1 = expedition.Crew[first];
                    if (expedition.Crew.Count > 2)
                    {
                        Member f2 = expedition.Crew[second];
                        expedition.Crew.Remove(f2);
                    }
                    expedition.Crew.Remove(f1);
                    expedition.Crew.Remove(target);
                    expedition.tick();
                }
                else expedition.tick();
            }
        }

        public void sit8()
        {
            Member target = expedition.Crew[chooseMember()];
            //INPUT
            Console.WriteLine(expedition.ToString());
            Console.WriteLine(string.Format(
                "У {0} симптомы обезвоживания! Он держится вполне бодро, но я переживаю за него!", target.Name));
            Console.WriteLine("1. Отправьте его обратно в лагерь, пока ему не стало хуже, пусть кто-то пойдет с ним!");
            Console.WriteLine("2. Попробуйте как-то помочь ему, может что-то из аптечки сможет улучшить ситуацию!");
            Console.Write("-> ");

            string choose = Console.ReadLine();
            if (choose.Equals("1"))
            {
                Console.WriteLine(string.Format("Кто будет сопровождать {0}", target.Name));
                int i = -1;
                foreach (var crewmate in expedition.Crew)
                {
                    i++;
                    if (crewmate.Equals(target)) continue;
                    Console.WriteLine(string.Format("{0}. {1}", i, crewmate.Name));
                }

                Console.Write("first is -> ");
                int first = Convert.ToInt32(Console.ReadLine());
                expedition.Crew.Remove(target);
                if(expedition.Crew.Count == 0) throw new Exception("GAME OVER");
                Member f1 = expedition.Crew[first];
                expedition.Crew.Remove(f1);
            }
            else
            {
                int count = 0;
                foreach (var crewmate in expedition.Crew)
                {
                    if (crewmate.Hl) count++;
                }

                if (!check(hl * count, 70))
                {
                    target.kill();
                }
            }
            expedition.tick();
        }

        public void sit9()
        {
            //INPUT
            Console.WriteLine(expedition.ToString());
            Console.WriteLine("Мы наткнулись на какие-то странные камни, возьмем с собой несколько из них!");
            Console.WriteLine("1. Отличная новость!");
            Console.WriteLine("2. Нам пригодятся эти образцы!");
            Console.Write("-> ");
            Console.ReadLine();
            int sntst = 0;
            int sst = 0;
            foreach (var mate in expedition.Crew)
            {
                if (mate.Cls.Equals("sntst")) sntst++;
                if (mate.Cls.Equals("sst")) sst++;
            }

            if (check(sntst * 30 + sst * 20, 30))
            {
                Console.WriteLine("STONKS камушки за 500$");
            }
            else Console.WriteLine("Вы нашли просто камни! Это осколки вулканической породы и ничего больше.");
            expedition.tick();
        }

        public void sit10()
        {
            //INPUT
            Console.WriteLine(expedition.ToString());
            Console.WriteLine("Тут немного прояснилась погода, и мы можем видеть хребты гор далеко впереди. Майлз наблюдал за ними в бинокль, и ему показалось что он заметил какие-то неестественные формы!");
            Console.WriteLine("1. Попробуйте сфотографировать их!");
            Console.WriteLine("2. Понаблюдайте за ними подольше!");
            Console.Write("-> ");

            string choose = Console.ReadLine();
            if (choose.Equals("1"))
            {
                int sntst = 0;
                int sst = 0;
                foreach (var mate in expedition.Crew)
                {
                    if (mate.Cls.Equals("sntst")) sntst++;
                    if (mate.Cls.Equals("sst")) sst++;
                }

                if (!check(sntst * 30 + sst * 20, 10))
                {
                    foreach (var mate in expedition.Crew)
                    {
                        mate.Psh = mate.Psh - 10;
                    }
                    Console.WriteLine("Не удалось сделать снимки");
                }
                else Console.WriteLine("Удалось сделать снимки");
                expedition.tick();
            }
            else
            {
                if (!check(expedition.PshSum / expedition.Crew.Count, 15))
                {
                    foreach (var mate in expedition.Crew)
                    {
                        mate.Psh = mate.Psh - 10;
                    }
                    Console.WriteLine("Мы наблюдали за ними целый день! В конце концов нам кажется что это простые искажения, или облака, которые приняли причудливую форму. Слишком далеко, чтобы сказать что-то конкретное.");
                    expedition.dayUpdStats();
                }
                else
                {
                    Console.WriteLine("Удалось сделать снимки");
                    expedition.tick();
                }
            }
        }

        public void sit11()
        {
            Member target = expedition.Crew[chooseMember()];
            //INPUT
            Console.WriteLine(expedition.ToString());
            Console.WriteLine(string.Format(
                "{0} выглядит очень уставшим, мы сели немного отдохнуть, если он не поправится, нам придется разбить лагерь прямо тут.", target.Name));
            Console.WriteLine("1. Немного передохните и двигайтесь дальше у нас нет времени на это.");
            Console.WriteLine("2. Останьтесь и отдохните");
            Console.Write("-> ");

            string choose = Console.ReadLine();
            if (choose.Equals("1"))
            {
                if (!check(target.Strng, 20))
                {
                    Console.WriteLine(string.Format(
                        "Черт, {0} еле волочит ноги. Мы немного разгрузили его рюкзак , но сильно это не помогает.",
                        target.Name));
                    target.StrngBonus = -5;
                }
                expedition.tick();
            }
            else expedition.dayUpdStats();
        }

        public void sit12()
        {
            Member target = expedition.Crew[chooseMember()];
            //INPUT
            Console.WriteLine(expedition.ToString());
            Console.WriteLine("Мы наткнулись на большое скопление пингвинов, они выглядят больше, чем мы думали!");
            Console.WriteLine("1. Не пытайтесь накормить их (смех)!");
            Console.WriteLine("2. Сделайте пару фотографий для моего сына, он обожает этих зверей!");
            Console.Write("-> ");

            string choose = Console.ReadLine();
            if (choose.Equals("1"))Console.WriteLine(
                string.Format("{0} укусил пингвин, когда он пытался погладить его!", target.Name));
            else Console.WriteLine("Легко, тут получатся отличные кадры");
            expedition.tick();
        }

        public void run()
        {
            if (expedition.Crew.Count > 0) sit1();
            else
            {
                Console.WriteLine("GG");
                return;
            }

            if (expedition.Crew.Count > 0) sit2();
            else
            {
                Console.WriteLine("GG");
                return;
            }

            if (expedition.Crew.Count > 0) sit3();
            else
            {
                Console.WriteLine("GG");
                return;
            }

            if (expedition.Crew.Count > 0) sit4();
            else
            {
                Console.WriteLine("GG");
                return;
            }

            if (expedition.Crew.Count > 0) sit5();
            else
            {
                Console.WriteLine("GG");
                return;
            }

            if (expedition.Crew.Count > 0) sit6();
            else
            {
                Console.WriteLine("GG");
                return;
            }

            if (expedition.Crew.Count > 0) sit7();
            else
            {
                Console.WriteLine("GG");
                return;
            }

            if (expedition.Crew.Count > 0) sit8();
            else
            {
                Console.WriteLine("GG");
                return;
            }

            if (expedition.Crew.Count > 0) sit9();
            else
            {
                Console.WriteLine("GG");
                return;
            }

            if (expedition.Crew.Count > 0) sit10();
            else
            {
                Console.WriteLine("GG");
                return;
            }
            
            if (expedition.Crew.Count > 0) sit11();
            else
            {
                Console.WriteLine("GG");
                return;
            }
            
            if (expedition.Crew.Count > 0) sit12();
            else
            {
                Console.WriteLine("GG");
                return;
            }
        }
    }
}