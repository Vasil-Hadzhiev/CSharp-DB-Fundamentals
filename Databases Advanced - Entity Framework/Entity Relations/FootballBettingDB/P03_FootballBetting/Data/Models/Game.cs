namespace P03_FootballBetting.Data.Models
{
    using P03_FootballBetting.Data.Enums;
    using System;
    using System.Collections.Generic;

    public class Game
    {
        public Game()
        {
            this.Bets = new List<Bet>();
            this.PlayerStatistics = new List<PlayerStatistic>();
        }

        public int GameId { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
        public DateTime Date { get; set; }
        public float HomeTeamBetRate { get; set; }
        public float AwayTeamBetRate { get; set; }
        public float DrawBetRate { get; set; }
        public GameResult Result { get; set; }

        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; }

        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; }

        public ICollection<Bet> Bets { get; set; }
        public ICollection<PlayerStatistic> PlayerStatistics { get; set; }
    }
}