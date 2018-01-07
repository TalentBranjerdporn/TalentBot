﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TalentBot.Common.API
{
    class OpenDotaAPI
    {
        public static async Task<PlayerData> GetPlayerDataAsync(string playerID)
        {
            string result = await RequestHandler.GET($"https://api.opendota.com/api/players/{playerID}");

            if (result != null)
                return JsonConvert.DeserializeObject<PlayerData>(result);
            else
                Console.WriteLine("Failed");
                return null;
        }

        public static async Task<MatchData[]> GetPlayerMatches(string playerID, int limit, int days, int lobbyType)
        {
            string requestString = $@"https://api.opendota.com/api/players/{playerID}/matches?limit={limit}";

            if (days > 0)
                requestString += $@"&date={days}";
            if (lobbyType > 0)
                requestString += $@"&lobby_type={lobbyType}";

            requestString += $@"&project[]=hero_id&project[]=kills&project[]=deaths&project[]=assists&project[]=xp_per_min&project[]=gold_per_min&project[]=hero_damage&project[]=tower_damage&project[]=hero_healing&project[]=last_hits";
            string result = await RequestHandler.GET(requestString);

            if (result != null)
                return JsonConvert.DeserializeObject<MatchData[]>(result);
            else
                return null;
        }

        public static string GetLastLobby(string filePath)
        {
            var ServerLog = new List<string>();

            using (var fs = OpenStream(filePath, FileAccess.Read, FileShare.Read, 3))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                while (!sr.EndOfStream)
                {
                    ServerLog.Add(sr.ReadLine());
                }
            }

            return ServerLog.Last(x => x.Contains("Lobby"));
        }

        static private FileStream OpenStream(String fileName, FileAccess fileAccess, FileShare fileShare, int retryCount)
        {
            FileStream fs = null;
            for (int i = 1; i <= 3; ++i)
            {
                try
                {
                    fs = new FileStream(fileName, FileMode.Open, fileAccess, fileShare);
                    break;
                }
                catch (Exception e)
                {
                    if (i == 3)
                        throw e;

                    Thread.Sleep(200);
                }
            }

            if (fs.Length > 0)
            {
                return fs;
            }
            if (retryCount > 0)
            {
                Thread.Sleep(50);
                return OpenStream(fileName, fileAccess, fileShare, retryCount--);
            }
            else
                throw new Exception("File can't be read");
        }

        public static List<string> GetPlayerIDs()
        {

            var GameInfo = GetLastLobby("F:\\Program Files (x86)\\Steam\\steamapps\\common\\dota 2 beta\\game\\dota\\server_log.txt");

            var playerStartIndex = GameInfo.IndexOf('(') + 1;
            var playerEndIndex = GameInfo.IndexOf(')');
            var PlayerSection = GameInfo.Substring(playerStartIndex, playerEndIndex - playerStartIndex);

            var Players = PlayerSection.Split(' ').Where(x => x.Contains("[U:")).Take(10).ToList();

            var Results = new List<string>();

            foreach (var item in Players)
            {
                var startIndex = item.LastIndexOf(':') + 1;
                var endIndex = item.IndexOf(']');
                var length = endIndex - startIndex;

                Results.Add(item.Substring(startIndex, length));
            }

            return Results;
        }
    }
}
