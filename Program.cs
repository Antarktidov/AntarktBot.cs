using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace AntarktBot
{
    class Program
    {
        DiscordSocketClient client;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            client = new DiscordSocketClient();
            client.MessageReceived += CommandsHandler;
            //client.Log += Log;

            var token = "MTAzODE0NTA4NDc0NDU1MjQ1OA.GhwlKA.b-k2SmP0_niFUpLmBr86uuuLsw44BidDSfrrmk";

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            Console.Read();

        }


        /*private Task Log(LogMessage msg)
        {
            return msg.ToString();
        }*/

        private Task CommandsHandler(SocketMessage msg)
        {
            //Значения по умолчанию
            string wikiName = "losyash-library";
            string wikiLang = "ru";
            string wikiHosting = "fandom";
            string wikiOutput = "";
            string leftMark = "<";
            string rightMark = ">";
            string wikiLink = "";
            string Template = "";
            bool isTemplate = false;


            if (!msg.Author.IsBot)
            {
                switch (msg.Content) 
                {
                    case "!привет":
                    {
                        msg.Channel.SendMessageAsync($"Привет, {msg.Author}");
                        break;
                    }

                    case "!рандом":
                    {
                        Random rnd = new Random();
                        msg.Channel.SendMessageAsync($"Выпало число {rnd.Next(-1000, 1000)}");
                        break;
                    }
                }

                if (msg.Content.StartsWith("!рандом "))
                {
                    int num = int.Parse( msg.Content.Substring(7) );
                    Random rnd = new Random();
                    int rndNum = rnd.Next(-10, 10);

                    if (num == rndNum)
                    {
                        msg.Channel.SendMessageAsync($"Вы угадали, выпало число {rndNum}");
                    }

                    else
                    {
                        msg.Channel.SendMessageAsync($"А вот и нет, я загадал число {rndNum}");
                    }

                }

                if (msg.Content.StartsWith("!кнб "))
                {
                    string useritem = msg.Content.Substring(5);
                    Console.WriteLine(useritem);

                    var listBotItems = new List<string>{"камень", "ножницы", "бумага"};
                    
                    Random rnd = new Random();
                    var botitemIndex = rnd.Next(listBotItems.Count);
                    string botitem = listBotItems[botitemIndex];
                    Console.WriteLine(botitem);

                    if (botitem == useritem)
                    {
                        msg.Channel.SendMessageAsync($"У меня {botitem}, и у тебя {useritem}. Ничья");
                    }

                    else if (useritem == "камень" && botitem == "ножницы")
                    {
                        msg.Channel.SendMessageAsync($"У меня {botitem}, а у тебя {useritem}. Ты выйграл");
                    }

                    else if (useritem == "ножницы" && botitem == "камень")
                    {
                        msg.Channel.SendMessageAsync($"У меня {botitem}, а у тебя {useritem}. Я выйграл");
                    }

                    else if (useritem == "ножницы" && botitem == "бумага")
                    {
                        msg.Channel.SendMessageAsync($"У меня {botitem}, а у тебя {useritem}. Ты выйграл");
                    }

                    else if (useritem == "бумага" && botitem == "ножницы")
                    {
                        msg.Channel.SendMessageAsync($"У меня {botitem}, а у тебя {useritem}. Я выйграл");
                    }

                    else if (useritem == "камень" && botitem == "бумага")
                    {
                        msg.Channel.SendMessageAsync($"У меня {botitem}, а у тебя {useritem}. Я выйграл");
                    }

                    else if (useritem == "бумага" && botitem == "камень")
                    {
                        msg.Channel.SendMessageAsync($"У меня {botitem}, а у тебя {useritem}. Ты выйграл");
                    }
                }
                if ((msg.Content.Contains("[[") && msg.Content.Contains("]]")) || (msg.Content.Contains("{{") && msg.Content.Contains("}}")))
                {

                    Console.WriteLine($"Текст {msg.Content} соддержит вики-ссылку");
                    string  Content = msg.Content;
                    Console.WriteLine($"Content: {Content}");

                    if (msg.Content.Contains("[[") && msg.Content.Contains("]]"))
                    {
                        wikiLink = Content.Substring(Content.IndexOf("[") + 2 , Content.IndexOf("]") - Content.IndexOf("[") - 2);
                        Console.WriteLine($"Текст вики-ссылки: {wikiLink}");
                    }
                    else if (msg.Content.Contains("{{") && msg.Content.Contains("}}"))
                    {
                        wikiLink = Content.Substring(Content.IndexOf("{") + 2 , Content.IndexOf("}") - 2);
                        Console.WriteLine($"Текст вики-ссылки: {wikiLink}");

                        isTemplate = true;

                    }

                    wikiLink = wikiLink.Replace(" ", "_");

                    if (wikiLink.StartsWith("w:c:"))
                    {
                        wikiHosting = "fandom";
                        wikiLink = wikiLink.Substring(4);
                        Console.WriteLine($"Текст вики-ссылки (2):{wikiLink}");

                        string lUrl = wikiLink.Substring(0, wikiLink.IndexOf(":"));
                        Console.WriteLine($"Lurl: {lUrl}");
 
                        if (lUrl.Contains("."))
                        {
                            wikiLang = lUrl.Substring(0, 2);
                            Console.WriteLine($"wikiLang: {wikiLang}");

                            wikiName = lUrl.Substring(3);
                            Console.WriteLine($"wikiName: {wikiName}");
                        }
                        else
                        {
                            wikiLang = "en";
                            wikiName = lUrl;
                        }

                        wikiLink = wikiLink.Substring(wikiLink.IndexOf(":") + 1);
                        Console.WriteLine($"Текст вики-ссылки (3):{wikiLink}");
                    }

                    if (wikiLink.StartsWith("ruwikipedia:"))
                    {
                        wikiHosting = "wikipedia";
                        wikiLang = "ru";
                        wikiLink = wikiLink.Substring(12);
                    }

                    if (wikiLink.StartsWith("commons:"))
                    {
                        wikiHosting = "commons";
                        wikiLang = "en";
                        wikiLink = wikiLink.Substring(8);
                    }

                    if (wikiLink.StartsWith("mw:"))
                    {
                        wikiHosting = "mediawiki";
                        wikiLang = "en";
                        wikiLink = wikiLink.Substring(3);
                    }

                    if (wikiLink.StartsWith("mh:"))
                    {
                        wikiHosting = "miraheze";
                        wikiLang = "en";

                        wikiLink = wikiLink.Substring(3);
                        Console.WriteLine($"Текст вики-ссылки (2):{wikiLink}");
                        
                        wikiName = wikiLink.Substring(0, wikiLink.IndexOf(":"));
                        Console.WriteLine($"wikiName:{wikiName}");
                        Console.WriteLine($"Текст вики-ссылки (3):{wikiLink}");

                        wikiLink = wikiLink.Substring(wikiLink.IndexOf(":") + 1);
                        Console.WriteLine($"Текст вики-ссылки (4):{wikiLink}");

                        /*
                        wikiHosting = "fandom";
                        wikiLink = wikiLink.Substring(4);
                        Console.WriteLine($"Текст вики-ссылки (2):{wikiLink}");

                        string lUrl = wikiLink.Substring(0, wikiLink.IndexOf(":"));
                        Console.WriteLine($"Lurl: {lUrl}");
                        */
                    }

                    if (wikiLink.StartsWith("File:") || wikiLink.StartsWith("Файл:"))
                    {
                        wikiLink = wikiLink.Substring(5);
                        wikiLink = $"Special:Redirect/file?wpvalue={wikiLink}";
                        leftMark = "";
                        rightMark = "";
                    }
                    
                    switch(wikiHosting)
                    {
                        case "fandom":
                            if (wikiLang != "en")
                            {
                                wikiLang = wikiLang + "/";
                            }
                            else{
                                wikiLang = "";
                            }

                            if (isTemplate)
                            {
                                if (wikiLang == "ru/")
                                {
                                    Template = "Шаблон:";
                                }
                                else
                                {
                                    Template = "Template:";
                                }
                            }

                            wikiOutput = $"{leftMark}https://{wikiName}.fandom.com/{wikiLang}wiki/{Template}{wikiLink}{rightMark}";
                            Console.WriteLine("Фэндом");
                            break;

                        case "wikipedia":
                            if (isTemplate)
                            {
                                if (wikiLang == "ru")
                                {
                                    Template = "Шаблон:";
                                }
                                else
                                {
                                    Template = "Template:";
                                }
                            }

                            wikiOutput = $"{leftMark}https://{wikiLang}.wikipedia.org/wiki/{Template}{wikiLink}{rightMark}";
                            Console.WriteLine(wikiLink);
                            Console.WriteLine("Википедия");
                            break;

                        case "mediawiki":
                            if (isTemplate)
                            {
                                Template = "Template:";
                            }

                            wikiOutput = $"{leftMark}https://mediawiki.org/wiki/{Template}{wikiLink}{rightMark}";
                            Console.WriteLine(wikiLink);
                            Console.WriteLine("MediaWiki.org");
                            break;

                        case "commons":
                            if (isTemplate)
                            {
                                Template = "Template:";
                            }

                            wikiOutput = $"{leftMark}https://commons.wikimedia.org/wiki/{Template}{wikiLink}{rightMark}";
                            Console.WriteLine(wikiLink);
                            Console.WriteLine("Wikimedia Commons");
                            break;

                        case "miraheze":
                            if (isTemplate)
                            {
                                Template = "Template:";
                            }

                            wikiOutput = $"{leftMark}https://{wikiName}.miraheze.org/wiki/{Template}{wikiLink}{rightMark}";
                            Console.WriteLine("Мирахез");
                            break;
                    }

                    msg.Channel.SendMessageAsync(wikiOutput);
                }
            }
            return Task.CompletedTask;
        }

    }
}
/*намерение Message Content на портале разработчиков Discord2 и добавили его к свойству GatewayIntents в DiscordSocketConfig2.*/