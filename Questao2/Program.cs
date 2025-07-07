using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

public class Program{
    public static async Task Main(){
        await ShowTeamGoals("Paris Saint-Germain", 2013);
        await ShowTeamGoals("Chelsea", 2014);
    }

    public static async Task ShowTeamGoals(string teamName, int year){
        int totalGoals = await GetTotalScoredGoals(teamName, year);
        Console.WriteLine($"Team {teamName} scored {totalGoals} goals in {year}");
    }

    public static async Task<int> GetTotalScoredGoals(string team, int year){
        int totalGoals = 0;

        using (HttpClient client = new HttpClient()){
            totalGoals += await GetGoalsByRole(client, team, year, "team1", "team1goals");

            totalGoals += await GetGoalsByRole(client, team, year, "team2", "team2goals");
        }

        return totalGoals;
    }

    private static async Task<int> GetGoalsByRole(HttpClient client, string team, int year, string teamParam, string goalParam){
        int page = 1;
        int totalGoals = 0;
        int totalPages;

        do{
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&{teamParam}={Uri.EscapeDataString(team)}&page={page}";
            string response = await client.GetStringAsync(url);

            var result = JsonConvert.DeserializeObject<ApiResponse>(response);
            totalPages = result.total_pages;

            foreach (var match in result.data){
                int goals = int.Parse(goalParam == "team1goals" ? match.team1goals : match.team2goals);
                totalGoals += goals;
            }

            page++;
        } while (page <= totalPages);

        return totalGoals;
    }
}

public class ApiResponse{
    public int page { get; set; }
    public int per_page { get; set; }
    public int total { get; set; }
    public int total_pages { get; set; }
    public List<Match> data { get; set; }
}

public class Match{
    public string competition { get; set; }
    public int year { get; set; }
    public string round { get; set; }
    public string team1 { get; set; }
    public string team2 { get; set; }
    public string team1goals { get; set; }
    public string team2goals { get; set; }
}
