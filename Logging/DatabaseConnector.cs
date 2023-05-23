using Newtonsoft.Json;
using Npgsql;

namespace Bezdzione.Logging
{
    internal class DatabaseConnector
    {
        static string connectionString = Configuration.GetSetting("DB_CONNECTION_STRING");

        public static void SaveData(Result result)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            string query = "INSERT INTO Tests (category, success, timeout, test_time, deployment_http_status, error_message, request_data, result_data, created_at) " +
                           "VALUES (@category, @success, @timeout, @testTime, @status, @message, @requestData, @resultData, @createdAt)";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("category", result.Category);
                command.Parameters.AddWithValue("success", result.IsSuccessful);
                command.Parameters.AddWithValue("timeout", result.TimeoutMins);
                command.Parameters.AddWithValue("testTime", result.TestTime);
                command.Parameters.AddWithValue("status", result.HTTPStatusCode);
                command.Parameters.AddWithValue("message", result.DeploymentMessage);
                command.Parameters.AddWithValue("requestData", NpgsqlTypes.NpgsqlDbType.Jsonb, result.RequestData);
                command.Parameters.AddWithValue("resultData", NpgsqlTypes.NpgsqlDbType.Jsonb, result.ResponseData);
                command.Parameters.AddWithValue("createdAt", DateTime.Now);

                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
        
}
