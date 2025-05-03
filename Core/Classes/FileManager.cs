using Npgsql;
using System;
using System.Data;

namespace Ranked.Core.Classes
{
    public static class FileManager
    {
        private static string ConnectionString = "Host=localhost;Username=your_user;Password=your_password;Database=ranked";

        // Add rank points to a player
        public static void AddRankPoints(string userId, int points, string eventType)
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                // Update player's rank points
                using (var cmd = new NpgsqlCommand(
                    @"INSERT INTO player (user_id, rank_points) 
                    VALUES (@user_id, @points) 
                    ON CONFLICT (user_id) DO UPDATE SET 
                    rank_points = player.rank_points + @points;", conn))
                {
                    cmd.Parameters.AddWithValue("user_id", userId);
                    cmd.Parameters.AddWithValue("points", points);
                    cmd.ExecuteNonQuery();
                }

                // Log the event
                using (var cmd = new NpgsqlCommand(
                    @"INSERT INTO events (user_id, event_type, points) 
                    VALUES (@user_id, @event_type, @points);", conn))
                {
                    cmd.Parameters.AddWithValue("user_id", userId);
                    cmd.Parameters.AddWithValue("event_type", eventType);
                    cmd.Parameters.AddWithValue("points", points);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Get rank points for a player
        public static int GetRankPoints(string userId)
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(
                    @"SELECT rank_points FROM player WHERE user_id = @user_id;", conn))
                {
                    cmd.Parameters.AddWithValue("user_id", userId);
                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        // Update player's rank level
        public static void UpdateRankLevel(string userId, string rankLevel)
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(
                    @"UPDATE player SET rank_level = @rank_level WHERE user_id = @user_id;", conn))
                {
                    cmd.Parameters.AddWithValue("user_id", userId);
                    cmd.Parameters.AddWithValue("rank_level", rankLevel);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}